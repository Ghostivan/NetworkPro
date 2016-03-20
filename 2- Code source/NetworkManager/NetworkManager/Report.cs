using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkManager {
    /// <summary>
    /// Report for an user
    /// </summary>
    public class ReportUser {
        public string login { get; set; }
        public int duration { get; set; }
    }

    /// <summary>
    /// Report for an application
    /// </summary>
    public class ReportApp {
        public string name { get; set; }
        public string editor { get; set; }
        public string version { get; set; }
        public DateTime date { get; set; }
    }

    /// <summary>
    /// Status of a job
    /// </summary>
    public enum ReportJobStatus {
        DONE, FAILED
    }

    /// <summary>
    /// Report for a job
    /// </summary>
    public class ReportJob {
        public string id { get; set; }
        public ReportJobStatus status { get; set; }
        public string extra { get; set; }
    }

    /// <summary>
    /// Report send by all the clients
    /// </summary>
    public class Report {

        /// <summary>
        /// Unique ID of the client
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// Mac address
        /// </summary>
        public string macAddr { get; set; }

        /// <summary>
        /// IP address
        /// </summary>
        public string ipAddr { get; set; }

        /// <summary>
        /// OS Version
        /// </summary>
        public string osVersion { get; set; }

        /// <summary>
        /// List of logged in users
        /// </summary>
        public List<ReportUser> users { get; set; }

        /// <summary>
        /// List of installed apps
        /// </summary>
        public List<ReportApp> apps { get; set; }

        /// <summary>
        /// List of jobs reported
        /// </summary>
        public List<ReportJob> jobs { get; set; }
    }
}
