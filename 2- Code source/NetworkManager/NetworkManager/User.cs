using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkManager {

    /// <summary>
    /// Front user, only used for accessing the admin endpoints and the admin website
    /// </summary>
    public class User {

        /// <summary>
        /// Mongodb id, also used as the client unique ID
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        
        /// <summary>
        /// Login of the user
        /// </summary>
        public string login { get; set; }
    }
}
