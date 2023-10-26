using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatLib.JsonModel
{
    public class AboveJson
    {        
        public InfoJson Info { get; set; }
        public List<SatelliteJson> Above { get; set; }

        public class InfoJson
        {
            public string Category { get; set; }
            public int SatCount { get; set; }
        }

        public class SatelliteJson
        {
            public int SatId { get; set; }
            public string SatName { get; set; }
            public string IntDesignator { get; set; }
            public string LaunchDate { get; set; }
            public double SatLat { get; set; }
            public double SatLng { get; set; }
            public double SatAlt { get; set; }
        }
    }
}
