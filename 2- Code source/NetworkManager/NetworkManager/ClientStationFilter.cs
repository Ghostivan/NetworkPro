using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkManager {
    public class ClientStationFilter {
        public int reportedLessThan { get; set; }
        public string hasOsVersion { get; set; }
        public string hasUser { get; set; }
    }
}
