using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SatLib.JsonModel
{
    public class ConfigJson
    {
        [JsonPropertyName("n2apitoken")]
        public string N2ApiToken { get; set; } = null!;

        [JsonPropertyName("snapitoken")]
        public string SNApiToken { get; set; } = null!;

        [JsonPropertyName("n2baseurl")]
        public string N2BaseUrl { get; set; } = null!;

        [JsonPropertyName("snbaseurl")]
        public string SNBaseUrl { get; set; } = null!;

        [JsonPropertyName("coneactivationangle")]
        public double ConeActivationAngle { get; set; }

        [JsonPropertyName("recordingtime")]
        public long RecordingTime { get; set; }
    }
}
