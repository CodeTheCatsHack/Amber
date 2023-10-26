using CoreLibrary.EFContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLibrary
{
    public class DataAccessLayer
    {
        private static DataAccessLayer _instance;

        private static object _instanceLock = new object();

        public static DataAccessLayer Instance
        {
            get
            {
                lock (_instanceLock)
                {
                    if (_instance == null)
                    {
                        _instance = new DataAccessLayer();
                    }
                    return _instance;
                }
            }
        }
    }
}
