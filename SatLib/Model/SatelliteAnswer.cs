using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatLib.Model
{
    public class SatelliteAnswer
    {
        public int Norad { get; set; }
        public int MinutesToArrive { get; set; }
        public ResultEnum Result { get; set; }

        public enum ResultEnum
        {
            Success,
            Takeoff,
            WrongTiming,
            Other
        }
    }
}
