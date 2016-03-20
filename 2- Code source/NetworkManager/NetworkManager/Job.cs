using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkManager {

    /// <summary>
    /// Status of a job
    /// </summary>
    public enum JobStatus {
        PENDING,        // The job is waiting to be send 
        PENDING_UPDATE, // The have been updated, and should be re-scheduled by the clients
        SCHEDULED,      // The job is scheduled by the client
        CANCELLED,      // The job is cancelled
        DONE,           // The job has terminated
        ERROR           // The job has terminated with errors
    }

    /// <summary>
    /// State of job, for a given client 
    /// </summary>
    public class JobState {
        /// <summary>
        /// Client unique Id
        /// </summary>
        string clientId { get; set; }

        /// <summary>
        /// Job status for the current client
        /// </summary>
        JobState status { get; set; }

        /// <summary>
        /// Date of the last update from the client
        /// </summary>
        DateTime lastUpdate { get; set; }
    }

    /// <summary>
    /// Type of job to performs
    /// </summary>
    public enum JobType {
        INSTALL,         // Install an application
        BASH,            // Run a bash command
        SCHEDULE_BASH,   // Add a bash command to the windows scheduler
        UNSCHEDULE_BASH, // Remove a bash command to the windows scheduler
        SHUTDOWN,        // Shutdown the client
        REBOOT,          // Reboot the client
        BOOT             // Make the client boot (note : this will only try to, and not send anything)
    }

    /// <summary>
    /// A job is the task that one or many client should performs
    /// </summary>
    public class Job {

        /// <summary>
        /// Mongodb id, also used as the client unique ID
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }

        /// <summary>
        /// Type of job to performs
        /// </summary>
        JobType type { get; set; }

        /// <summary>
        /// When the job should be performed on the client. If this value is set to null,
        /// that means that the job should be performed as soon as possible
        /// </summary>
        DateTime when { get; set; }

        /// <summary>
        /// If the server needs a confirmation. If set to false, the job will be completed as soon as send
        /// to every concerned clients
        /// </summary>
        bool needsConfirmation { get; set; }

        /// <summary>
        /// Application name to download and install
        /// </summary>
        string application { get; set; }

        /// <summary>
        /// Batch code to execute
        /// </summary>
        string batchContent { get; set; }

        /// <summary>
        /// Batch code to execute before the task
        /// </summary>
        string batchBefore { get; set; }

        /// <summary>
        /// Batch code to execute after the task
        /// </summary>
        string batchAfter { get; set; }

        /// <summary>
        /// Status of the job, per client. This list is not update when new clients are present, and the job
        /// should manually be edited to add new clients
        /// </summary>
        List<JobState> jobStates { get; set; }

        public Job(JobType type, DateTime when) {
            this.type = type;
            this.when = when;
        }
    }
}
