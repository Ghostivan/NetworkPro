using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkManager {

    public enum ReportResponseJobAction {
        CREATE, UPDATE, CANCEL
    }

    /// <summary>
    /// Report on a job to performs for the client
    /// </summary>
    public class ReportResponseJob {
        /// <summary>
        /// Unique ID of the job
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// The type of job
        /// </summary>
        public JobType type { get; set; }

        /// <summary>
        /// The date to performs the job (null means now)
        /// </summary>
        public DateTime when { get; set; }

        /// <summary>
        /// Action to performs to the job
        /// </summary>
        public ReportResponseJobAction action { get; set; }

        public bool needsConfirmation { get; set; }

        public string extra { get; set; }
    }

    /// <summary>
    /// Reporting response passed from the server
    /// to any reporting client
    /// </summary>
    class ReportResponse {
        /// <summary>
        /// If not null, the client should download the
        /// updated configuration file
        /// </summary>
        public string pushConf { get; set; }

        /// <summary>
        /// List of jobs that the client should performs
        /// </summary>
        public List<ReportResponseJob> jobs { get; set; }
    }
}
