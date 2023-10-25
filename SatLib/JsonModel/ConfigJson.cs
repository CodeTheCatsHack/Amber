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
        [JsonPropertyName("apitoken")]
        public string ApiToken { get; set; } = null!;

        [JsonPropertyName("baseurl")]
        public string BaseUrl { get; set; } = null!;
    }
}
