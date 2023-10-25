using Newtonsoft.Json;
using SatLib.JsonModel;
using SGPdotNET.CoordinateSystem;
using SGPdotNET.Observation;
using SGPdotNET.Propagation;
using SGPdotNET.TLE;
using System.Net.Http.Json;
using System.Text;

namespace SatLib
{
    public static class Connection
    {        
        internal static SatelliteJson RequestJson(long norad)
        {
            return new HttpClient().GetFromJsonAsync<SatelliteJson>($"{Configurator.MainConfig.BaseUrl}" +
                $"/tle/{norad}" +
                $"&apiKey={Configurator.MainConfig.ApiToken}").Result
                ?? throw new Exception("bad parse tleJson:\n");
        }

        internal static string CalculateOrbit(SatelliteJson satJson)
        {
            Satellite satellite = new Satellite(
                satJson.Info.SatName,
                satJson.GetTleLine(SatelliteJson.TleLine.first),
                satJson.GetTleLine(SatelliteJson.TleLine.second));

            EciCoordinate eci = satellite.Predict();
            StringBuilder builder = new StringBuilder();

            builder.AppendLine(eci.ToString());
            builder.AppendLine(eci.ToGeodetic().ToString());
            builder.AppendLine("LAT " + eci.ToGeodetic().Latitude.ToString());
            builder.AppendLine("LONG " + eci.ToGeodetic().Longitude.ToString());
            return builder.ToString();
        }
        

        /// <summary>
        /// Функция для получения текущих координат спутника
        /// </summary>
        /// <param name="norad">NORAD-id спутника</param>
        /// <returns>Долгота и широта</returns>
        public static Model.Coordinate CalculateCoordinates(long norad)
        {
            return CalculateCoordinates(norad, DateTime.UtcNow);
        }

        /// <summary>
        /// Функция для получения координат спутника в определенный момент времени
        /// </summary>
        /// <param name="norad">NORAD-id спутника</param>
        /// <param name="prefferedDateTime">время по UTC</param>
        /// <returns>Долгота и широта</returns>
        public static Model.Coordinate CalculateCoordinates(long norad, DateTime prefferedDateTime)
        {
            SatelliteJson satJson = RequestJson(norad);
            Satellite satellite = new Satellite(
                satJson.Info.SatName,
                satJson.GetTleLine(SatelliteJson.TleLine.first),
                satJson.GetTleLine(SatelliteJson.TleLine.second));

            GeodeticCoordinate coord = satellite.Predict(prefferedDateTime).ToGeodetic();
            return new Model.Coordinate(coord.Latitude.Degrees, coord.Longitude.Degrees);
        }        
    }
}