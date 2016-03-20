using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkManager {

    public class ClientStationUser {
        public string login { get; set; }
        public int duration { get; set; }
    }

    /// <summary>
    /// A client station is a computer reporting to the server, and identifier by its unique ID based
    /// on some local information of the computer
    /// </summary>
    public class ClientStation {
        /// <summary>
        /// Unique ID based on the client motherboard serial number
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// The mac address of the client, may change
        /// </summary>
        public string macAddr { get; set; }

        /// <summary>
        /// Current IP address of the client, may change
        /// </summary>
        public string ipAddr { get; set; }

        /// <summary>
        /// The os used
        /// </summary>
        public string osVersion { get; set; }

        /// <summary>
        /// Date of the last report made by this client
        /// </summary>
        public DateTime lastReport { get; set; }

        public List<ClientStationUser> users { get; set; }
    }

}
