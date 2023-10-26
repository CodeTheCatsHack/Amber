using SGPdotNET.TLE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SatLib.JsonModel
{
    public class SatelliteJson
    {
        private string[]? tleLines = null;

        [JsonPropertyName("info")]
        public InfoJson Info { get; set; } = null!;

        [JsonPropertyName("tle")]
        public string Tle { get; set; } = null!;

        public string GetTleLine(TleLine line)
        {
            if (tleLines is null)
            {
                tleLines = Tle.Trim().Replace("\r", "").Split('\n');
            }
            
            switch (line)
            {
                case TleLine.first:
                    return tleLines[0];
                case TleLine.second:
                    return tleLines[1];
                default:
                    throw new Exception("unknown TleLine type: " + line.ToString());
            }
        }

        public class InfoJson
        {
            [JsonPropertyName("satid")]
            public int SatId { get; set; }

            [JsonPropertyName("satname")]
            public string SatName { get; set; } = null!;

            [JsonPropertyName("transactioncount")]
            public int TransactionCount { get; set; }
        }

        public enum TleLine
        {
            first,
            second
        }
    }

    public class Line1
    {
        public int LineNumber { get; set; }
        public int SatelliteNumber { get; set; }
        public string Classification { get; set; }
        public int LaunchYear { get; set; }
        public int LaunchNumber { get; set; }
        public string LaunchPiece { get; set; }
        public int EpochYear { get; set; }
        public double EpochDay { get; set; }
        public double FirstDerivativeMeanMotion { get; set; }
        public double SecondDerivativeMeanMotion { get; set; }
        public double DragTerm { get; set; }
        public string EphemerisType { get; set; }
        public int ElementNumber { get; set; }
    }

    public class Line2
    {
        public int LineNumber { get; set; }
        public int SatelliteNumber { get; set; }
        public double Inclination { get; set; }
        public double RightAscension { get; set; }
        public double Eccentricity { get; set; }
        public double ArgumentOfPerigee { get; set; }
        public double MeanAnomaly { get; set; }
        public double MeanMotion { get; set; }
        public int Revolutions { get; set; }
    }
}
