using SatLib.JsonModel;
using SGPdotNET.CoordinateSystem;
using SGPdotNET.Observation;
using SGPdotNET.Util;
using System.Net.Http.Json;

namespace SatLib
{
    public static class Connection
    {
        /// <summary>
        /// Метод для получения TLE и короткой информации о спутнике средствами API
        /// </summary>
        /// <param name="norad">NORAD-id</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        internal static SatelliteJson RequestSatelliteJson(long norad)
        {
            // добавить функционал проверки кэша из БД, а потом только делать запрос если в кэше нет
            return new HttpClient().GetFromJsonAsync<SatelliteJson>($"{Configurator.MainConfig.BaseUrl}" +
                $"/tle/{norad}" +
                $"&apiKey={Configurator.MainConfig.ApiToken}").Result
                ?? throw new Exception("bad parse tleJson:\n");
        }

        internal static Satellite RequestSatellite(long norad)
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
        public static Coordinate CalculateCoordinates(long norad)
        {
            return CalculateCoordinates(norad, DateTime.UtcNow);
        }

        /// <summary>
        /// Функция для получения координат спутника в определенный момент времени
        /// </summary>
        /// <param name="norad">NORAD-id спутника</param>
        /// <param name="prefferedDateTime">время по UTC</param>
        /// <returns>Долгота и широта</returns>
        public static Coordinate CalculateCoordinates(long norad, DateTime prefferedDateTime)
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
        public static AboveJson? RequestNearestSatellites(Coordinate observerCoordinates, double searchDegrees, SatelliteCategory category)
        {
            return new HttpClient().GetFromJsonAsync<AboveJson>($"{Configurator.MainConfig.BaseUrl}" +
                $"/above/{observerCoordinates.ToGeodetic().Latitude.Degrees}" +
                $"/{observerCoordinates.ToGeodetic().Longitude.Degrees}" +
                $"/{observerCoordinates.ToGeodetic().Altitude}" +
                $"/{searchDegrees}/{(int)category}" +
                $"&apiKey={Configurator.MainConfig.ApiToken}").Result;
        }

        /// <summary>
        /// Функция для получения угла между Ecif-спутника и зенитом точки
        /// </summary>
        /// <param name="earthPoint">точка на земле</param>
        /// <param name="norad">NORAD-id спутника</param>
        /// <param name="time">момент времени по UTC</param>
        /// <returns>Угол</returns>
        public static double CalculateAngle(Coordinate earthPoint, long norad, DateTime time)
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
        public static bool CalculateIsPointInCone(Coordinate earthPoint, long norad, DateTime time)
        {
            return CalculateAngle(earthPoint, norad, time) < Configurator.MainConfig.ConeActivationAngle;
        }

        /// <summary>
        /// Функция поиска решения для фотографии заданной местности
        /// </summary>
        /// <param name="earthPoint">заданная местность</param>
        public static long SearchSolution(Coordinate earthPoint)
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
                    if (currentAngle <= Configurator.MainConfig.ConeActivationAngle)
                    {
                        long requiredSatelliteId = GetFirstRelevantSatelliteId(aboveWithCurrentAngle, earthPoint);
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

        static long GetFirstRelevantSatelliteId(AboveJson above, Coordinate earthPoint)
        {
            foreach (AboveJson.SatelliteJson satelliteJson in above.Above)
            {
                Console.WriteLine("scanning: " + satelliteJson.SatId);
                // проверяем каждый спутник, будет ли он через время съёмки все ещё в зоне сканирования (успеет ли засканировать)
                bool isSuccess = CalculateIsPointInCone(earthPoint, satelliteJson.SatId, DateTime.UtcNow.AddSeconds(Configurator.MainConfig.RecordingTime));
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
            Brighttest = 1,
            ISS = 2,
            Weather = 3,
            NOAA = 4,
            GOES = 5,
            Earth_resources = 6
        }
    }
}