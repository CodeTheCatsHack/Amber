using CoreLibrary.EFContext;
using Microsoft.Extensions.DependencyInjection;
using SatLib.JsonModel;
using SGPdotNET.CoordinateSystem;
using SGPdotNET.Observation;
using SGPdotNET.Util;
using System.Net.Http.Json;
using ZstdSharp.Unsafe;

namespace SatLib
{
    /// <summary>
    /// Модуль работы с высчитыванием орбит и поиска оптимального спутника и информации о спутниках
    /// </summary>
    public class SatelliteApi
    {
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
            using (var scope = _serviceProvider.CreateScope())
            {
                ServiceMonitoringContext ctx = scope.ServiceProvider.GetRequiredService<ServiceMonitoringContext>();
                CoreLibrary.Models.EFModels.Satellite? satellite = ctx.Satellites.FirstOrDefault(s => s.IdSatellite == norad);

                if (satellite is not null)
                {
                    Console.WriteLine($"From cache {satellite.IdSatellite}");
                    return new SatelliteJson()
                    {
                        Info = new SatelliteJson.InfoJson()
                        {
                            SatId = satellite.IdSatellite,
                            SatName = satellite.SatName,
                        },
                        Tle = satellite.TleLine1 + "\n" + satellite.TleLine2
                    };
                }

                
                SatelliteJson satJson = new HttpClient().GetFromJsonAsync<SatelliteJson>($"{_configurator.MainConfig.N2BaseUrl}" +
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
                    IsOffiicial = 1
                });
                ctx.SaveChanges();
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
        public AboveJson? RequestNearestSatellites(Coordinate observerCoordinates, double searchDegrees, SatelliteCategory category)
        {
            return new HttpClient().GetFromJsonAsync<AboveJson>($"{_configurator.MainConfig.N2BaseUrl}" +
                $"/above/{observerCoordinates.ToGeodetic().Latitude.Degrees}" +
                $"/{observerCoordinates.ToGeodetic().Longitude.Degrees}" +
                $"/{observerCoordinates.ToGeodetic().Altitude}" +
                $"/{searchDegrees}/{(int)category}" +
                $"&apiKey={_configurator.MainConfig.N2ApiToken}").Result;
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
            EciCoordinate modifiedEciSatellitePoint = new EciCoordinate(DateTime.UtcNow, satellitePoint.ToSphericalEcef() - modificator);

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
        public int SearchSolution(Coordinate earthPoint)
        {
            double step = 5.0;
            double currentAngle = 0;
            AboveJson? aboveWithCurrentAngle;

            do
            {
                currentAngle += step;
                Console.WriteLine("currentAngle:" + currentAngle);

                aboveWithCurrentAngle = RequestNearestSatellites(
                    earthPoint,
                    currentAngle,
                    SatelliteCategory.All);

                if (aboveWithCurrentAngle is not null)
                {
                    if (currentAngle <= _configurator.MainConfig.ConeActivationAngle)
                    {
                        int requiredSatelliteId = GetFirstRelevantSatelliteId(aboveWithCurrentAngle, earthPoint);
                        if (requiredSatelliteId > 0)
                        {
                            return requiredSatelliteId;
                        }
                    }
                    else
                    {

                    }
                }
            }
            while (currentAngle < 90.0);


            return -1;
        }

        int GetFirstRelevantSatelliteId(AboveJson above, Coordinate earthPoint)
        {
            foreach (AboveJson.SatelliteJson satelliteJson in above.Above)
            {
                Console.WriteLine("scanning: " + satelliteJson.SatId);
                // проверяем каждый спутник, будет ли он через время съёмки все ещё в зоне сканирования (успеет ли засканировать)
                bool isSuccess = CalculateIsPointInCone(earthPoint, satelliteJson.SatId, DateTime.UtcNow.AddSeconds(_configurator.MainConfig.RecordingTime));
                if (isSuccess)
                {
                    return satelliteJson.SatId;
                }
            }
            return -1;
        }

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
    }
}