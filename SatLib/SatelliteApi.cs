using System.Net.Http.Json;
using CoreLibrary.EFContext;
using Microsoft.Extensions.DependencyInjection;
using SatLib.JsonModel;
using SatLib.Model;
using SGPdotNET.CoordinateSystem;
using SGPdotNET.Observation;
using SGPdotNET.Util;

namespace SatLib
{
    /// <summary>
    /// Модуль работы с высчитыванием орбит и поиска оптимального спутника и информации о спутниках
    /// </summary>
    public class SatelliteApi
    {
        public enum SatelliteCategory
        {
            All = 0,
            Brightest = 1,
            GOES = 2,
            EarthResources = 3,
            DisasterMonitoring = 4,
            Geostationary = 5,
            Intelsat = 6,
            Gorizont = 7,
            Iridium = 8,
            Globalstar = 9,
            AmateurRadio = 10,
            GlobalGPSOperational = 11,
            GlonassOperational = 12,
            Galileo = 13,
            Experimental = 14,
            Geodetic = 15,
            Engineering = 16,
            Education = 17,
            CubeSats = 18,
            BeidouNavigationSystem = 19,
            Gonets = 20,
            Flock = 21,
            GlobalGPSConstellation = 22,
            GlonassConstellation = 23,
            Celestis = 24,
            ChineseSpaceStation = 25,
            ISS = 26,
            Lemur = 27,
            Military = 28,
            Molniya = 29,
            NavyNavigationSatelliteSystem = 30,
            NOAA = 31,
            O3BNetworks = 32,
            OneWeb = 33,
            Orbcomm = 34,
            Parus = 35,
            QZSS = 36,
            RadarCalibration = 37,
            Raduga = 38,
            RussianLEONavigation = 39,
            SatelliteBasedAugmentationSystem = 40,
            SearchAndRescue = 41,
            SpaceAndEarthScience = 42,
            Starlink = 43,
            Strela = 44,
            TrackingAndDataRelaySatelliteSystem = 45,
            Tselina = 46,
            Tsikada = 47,
            Tsiklon = 48,
            TV = 49,
            Weather = 50,
            WestfordNeedles = 51,
            XMandSirius = 52,
            Yaogan = 53
        }

        private Dictionary<int, SatelliteJson> _cache = new Dictionary<int, SatelliteJson>();
        private Configurator _configurator;
        private IServiceProvider _serviceProvider;

        public SatelliteApi(Configurator configurator, IServiceProvider provider)
        {
            _configurator = configurator;
            _serviceProvider = provider;
        }

        /// <summary>
        /// Метод для получения TLE и короткой информации о спутнике средствами API
        /// </summary>
        /// <param name="norad">NORAD-id</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        internal SatelliteJson RequestSatelliteJson(int norad)
        {
            if (_cache.ContainsKey(norad))
            {
                return _cache[norad];
            }

            using (var scope = _serviceProvider.CreateScope())
            {
                ServiceMonitoringContext ctx = scope.ServiceProvider.GetRequiredService<ServiceMonitoringContext>();
                CoreLibrary.Models.EFModels.Satellite? satellite =
                    ctx.Satellites.FirstOrDefault(s => s.IdSatellite == norad);

                if (satellite is not null)
                {
                    Console.WriteLine($"From db {satellite.IdSatellite}");
                    SatelliteJson json = new SatelliteJson()
                    {
                        Info = new SatelliteJson.InfoJson()
                        {
                            SatId = satellite.IdSatellite,
                            SatName = satellite.SatName,
                        },
                        Tle = satellite.TleLine1 + "\n" + satellite.TleLine2
                    };
                    _cache[norad] = json;
                    return json;
                }


                SatelliteJson satJson = new HttpClient().GetFromJsonAsync<SatelliteJson>(
                                            $"{_configurator.MainConfig.N2BaseUrl}" +
                                            $"/tle/{norad}" +
                                            $"&apiKey={_configurator.MainConfig.N2ApiToken}").Result
                                        ?? throw new Exception("bad parse tleJson:\n");
                Console.WriteLine($"Caching " + satJson.Info.SatId);

                ctx.Satellites.Add(new CoreLibrary.Models.EFModels.Satellite()
                {
                    IdSatellite = satJson.Info.SatId,
                    SatName = satJson.Info.SatName,
                    TleLine1 = satJson.GetTleLine(SatelliteJson.TleLine.first),
                    TleLine2 = satJson.GetTleLine(SatelliteJson.TleLine.second),
                    Status = "Notbusy",
                    IsOffiicial = 1,
                    Map = 1
                });
                ctx.SaveChanges();
                _cache[satJson.Info.SatId] = satJson;
                return satJson;
            }
        }

        internal Satellite RequestSatellite(int norad)
        {
            SatelliteJson satJson = RequestSatelliteJson(norad);
            return new Satellite(
                satJson.Info.SatName,
                satJson.GetTleLine(SatelliteJson.TleLine.first),
                satJson.GetTleLine(SatelliteJson.TleLine.second));
        }

        /// <summary>
        /// Функция для получения текущих координат спутника
        /// </summary>
        /// <param name="norad">NORAD-id спутника</param>
        /// <returns>Долгота и широта</returns>
        public Coordinate CalculateCoordinates(int norad)
        {
            return CalculateCoordinates(norad, DateTime.UtcNow);
        }

        /// <summary>
        /// Функция для получения координат спутника в определенный момент времени
        /// </summary>
        /// <param name="norad">NORAD-id спутника</param>
        /// <param name="prefferedDateTime">время по UTC</param>
        /// <returns>Долгота и широта</returns>
        public Coordinate CalculateCoordinates(int norad, DateTime prefferedDateTime)
        {
            return RequestSatellite(norad).Predict(prefferedDateTime).ToGeodetic();
        }

        /// <summary>
        /// Функция для поиска спутников находящихся в определенном радиусе относительно надира
        /// </summary>
        /// <param name="observerCoordinates">Координаты надира на земле</param>
        /// <param name="searchDegrees">Угол относительно надира</param>
        /// <param name="category">Категория требуемых спутников</param>
        /// <returns>Список спутников, или null, если нет</returns>
        /// <exception cref="Exception"></exception>
        public AboveJson? RequestNearestSatellites(Coordinate observerCoordinates, double searchDegrees,
            SatelliteCategory category)
        {
            return new HttpClient().GetFromJsonAsync<AboveJson>($"{_configurator.MainConfig.N2BaseUrl}" +
                                                                $"/above/{observerCoordinates.ToGeodetic().Latitude.Degrees}" +
                                                                $"/{observerCoordinates.ToGeodetic().Longitude.Degrees}" +
                                                                $"/{observerCoordinates.ToGeodetic().Altitude}" +
                                                                $"/{searchDegrees}/{(int)category}" +
                                                                $"&apiKey={_configurator.MainConfig.N2ApiToken}")
                .Result;
        }

        /// <summary>
        /// Функция для получения угла между Ecif-спутника и зенитом точки
        /// </summary>
        /// <param name="earthPoint">точка на земле</param>
        /// <param name="norad">NORAD-id спутника</param>
        /// <param name="time">момент времени по UTC</param>
        /// <returns>Угол</returns>
        public double CalculateAngle(Coordinate earthPoint, int norad, DateTime time)
        {
            Satellite satellite = RequestSatellite(norad);
            EciCoordinate satellitePoint = satellite.Predict(time);

            // поправка на ecif точки на земле
            Vector3 modificator = earthPoint.ToSphericalEcef();

            EciCoordinate modifiedEciEarthPoint = new EciCoordinate(DateTime.UtcNow, earthPoint.ToSphericalEcef());
            EciCoordinate modifiedEciSatellitePoint =
                new EciCoordinate(DateTime.UtcNow, satellitePoint.ToSphericalEcef() - modificator);

            return modifiedEciEarthPoint.AngleTo(modifiedEciSatellitePoint).Degrees;
        }

        /// <summary>
        /// Функция активации нахождения спутника в зоне конуса
        /// </summary>
        /// <param name="earthPoint">точка на земле</param>
        /// <param name="norad">NORAD-id спутника</param>
        /// <param name="time">момент времени по UTC</param>
        /// <returns>Находится ли спутник в зоне конуса</returns>
        public bool CalculateIsPointInCone(Coordinate earthPoint, int norad, DateTime time)
        {
            return CalculateAngle(earthPoint, norad, time) < _configurator.MainConfig.ConeActivationAngle;
        }

        /// <summary>
        /// Функция поиска решения для фотографии заданной местности
        /// </summary>
        /// <param name="earthPoint">заданная местность</param>
        public async Task<List<SatelliteAnswer>?> SearchSolutionAsync(Coordinate earthPoint, SatelliteCategory category)
        {
            double step = 5.0;
            double currentAngle = 0;
            AboveJson? aboveWithCurrentAngle;

            do
            {
                currentAngle += step;
                Console.WriteLine("Текущий угол просмотра конуса:" + currentAngle);

                aboveWithCurrentAngle = RequestNearestSatellites(
                    earthPoint,
                    currentAngle,
                    category);

                if (aboveWithCurrentAngle is not null)
                {
                    if (currentAngle <= _configurator.MainConfig.ConeActivationAngle)
                    {
                        SatelliteAnswer requiredSat =
                            await GetFirstRelevantSatelliteIdAsync(aboveWithCurrentAngle, earthPoint);
                        if (requiredSat.Result == SatelliteAnswer.ResultEnum.Success)
                        {
                            return new List<SatelliteAnswer>() { requiredSat };
                        }
                    }
                    else
                    {
                        List<Task<SatelliteAnswer>> tasks = new List<Task<SatelliteAnswer>>();
                        foreach (AboveJson.SatelliteJson sat in aboveWithCurrentAngle.Above)
                        {
                            tasks.Add(ScanSatelliteWhileApproachingAsync(earthPoint, sat.SatId));
                        }

                        await Task.WhenAll(tasks);
                        List<SatelliteAnswer> answeredList = new List<SatelliteAnswer>();

                        foreach (var task in tasks)
                        {
                            answeredList.Add(task.Result);
                        }

                        answeredList = answeredList.Where(answer => answer.Result == SatelliteAnswer.ResultEnum.Success)
                            .ToList();
                        if (answeredList.Count > 0)
                        {
                            return answeredList;
                        }
                    }
                }
            } while (currentAngle < 90.0);

            return null;
        }

        /// <summary>
        /// Получить релевантный спутник из купола
        /// </summary>
        /// <param name="above"></param>
        /// <param name="earthPoint"></param>
        /// <returns></returns>
        async Task<SatelliteAnswer> GetFirstRelevantSatelliteIdAsync(AboveJson above, Coordinate earthPoint)
        {
            foreach (AboveJson.SatelliteJson satelliteJson in above.Above)
            {
                Console.WriteLine("scanning: " + satelliteJson.SatId);
                // проверяем каждый спутник, будет ли он через время съёмки все ещё в зоне сканирования (успеет ли засканировать)
                bool isSuccess = CalculateIsPointInCone(earthPoint, satelliteJson.SatId,
                    DateTime.UtcNow.AddSeconds(_configurator.MainConfig.RecordingTime));
                if (isSuccess)
                {
                    return new SatelliteAnswer()
                    {
                        Norad = satelliteJson.SatId,
                        MinutesToArrive = 0,
                        Result = SatelliteAnswer.ResultEnum.Success
                    };
                }
            }

            return new SatelliteAnswer()
            {
                MinutesToArrive = 0,
                Result = SatelliteAnswer.ResultEnum.Other
            };
        }

        /// <summary>
        /// Сканировать спутник пока он приближается
        /// </summary>
        /// <param name="earthPoint"></param>
        /// <param name="norad"></param>
        /// <returns>Минуты, Норад</returns>
        public Task<SatelliteAnswer> ScanSatelliteWhileApproachingAsync(Coordinate earthPoint, int norad)
        {
            Satellite satellite = RequestSatellite(norad);
            int modMins = 1;

            GeodeticCoordinate prevGeo, nextGeo = satellite.Predict().ToGeodetic();

            while (!CalculateIsPointInCone(earthPoint, norad, DateTime.UtcNow.AddMinutes(modMins)))
            {
                prevGeo = nextGeo;
                nextGeo = satellite.Predict(DateTime.UtcNow.AddMinutes(modMins)).ToGeodetic();

                double prevDist = earthPoint.DistanceTo(prevGeo);
                double nextDist = earthPoint.DistanceTo(nextGeo);

                Console.WriteLine(prevDist + " > " + nextDist + "?");
                if (prevDist < nextDist)
                {
                    Console.WriteLine("слетел (отдалется от точки): " + norad);
                    return Task.FromResult(new SatelliteAnswer()
                    {
                        Result = SatelliteAnswer.ResultEnum.Takeoff
                    });
                }

                modMins += 5;
            }

            if (CalculateIsPointInCone(earthPoint, norad, DateTime.UtcNow.AddMinutes(modMins)))
            {
                if (CheckTiming(earthPoint, norad))
                {
                    return Task.FromResult(new SatelliteAnswer()
                    {
                        Result = SatelliteAnswer.ResultEnum.Success,
                        Norad = norad,
                        MinutesToArrive = modMins
                    });
                }

                Console.WriteLine("не поместился в тайминг: " + norad);
                return Task.FromResult(new SatelliteAnswer()
                {
                    Result = SatelliteAnswer.ResultEnum.WrongTiming,
                    Norad = norad,
                    MinutesToArrive = modMins
                });
            }

            return Task.FromResult(new SatelliteAnswer()
            {
                Result = SatelliteAnswer.ResultEnum.Other,
                Norad = norad,
                MinutesToArrive = modMins
            });
        }

        /// <summary>
        /// Проверяет спутник, будет ли он через время съёмки все ещё в зоне конуса (успеет ли засканировать)
        /// </summary>
        /// <param name="earthPoint"></param>
        /// <param name="norad"></param>
        /// <returns></returns>
        public bool CheckTiming(Coordinate earthPoint, int norad)
        {
            return CalculateIsPointInCone(earthPoint, norad,
                DateTime.UtcNow.AddSeconds(_configurator.MainConfig.RecordingTime));
        }
    }
}