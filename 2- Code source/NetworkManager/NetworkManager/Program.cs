using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Hosting.Self;
using System;
using System.Collections.Generic;
using System.Linq;
using SuperWebSocket;
using SuperSocket.SocketBase;
using Nancy.Security;
using Nancy.Authentication.Forms;
using Nancy.Extensions;

namespace NetworkManager {

    // Used to enable coockies sessions
    public class Bootstrapper : DefaultNancyBootstrapper {
        protected override void ApplicationStartup(Nancy.TinyIoc.TinyIoCContainer container, Nancy.Bootstrapper.IPipelines pipelines) {
            FormsAuthentication.Enable(pipelines, new FormsAuthenticationConfiguration() {
                RedirectUrl = "/login", // On auth error, redirect to the login page
                UserMapper = container.Resolve<IUserMapper>(),
            });
        }
        
    }

    /// <summary>
    /// User mapper, TODO use MongoDB
    /// </summary>
    public class UserDatabase : IUserMapper {
        private static List<Tuple<string, string, Guid>> users = new List<Tuple<string, string, Guid>>();

        static UserDatabase() {
            users.Add(new Tuple<string, string, Guid>("admin", "password", new Guid("55E1E49E-B7E8-4EEA-8459-7A906AC4D4C0")));
            users.Add(new Tuple<string, string, Guid>("user", "password", new Guid("56E1E49E-B7E8-4EEA-8459-7A906AC4D4C0")));
        }

        public static Guid? ValidateUser(string username, string password) {
            var userRecord = users.FirstOrDefault(u => u.Item1 == username && u.Item2 == password);

            if (userRecord == null) {
                return null;
            }

            return userRecord.Item3;
        }

        IUserIdentity IUserMapper.GetUserFromIdentifier(Guid identifier, NancyContext context) {
            // Fetch the user
            var userRecord = users.FirstOrDefault(u => u.Item3 == identifier);

            // Return its ID
            return new UserIdentity(userRecord.Item1, new List<string>() { "user" });
        }
    }

    /// <summary>
    /// User identity, TODO use as user model ?
    /// </summary>
    internal class UserIdentity : IUserIdentity {

        public UserIdentity(string userName, IEnumerable<string> claims) {
            this.UserName = userName;
            this.Claims = claims;
        }

        public IEnumerable<string> Claims {
            get;
        }

        public string UserName {
            get;
        }
    }

    /// <summary>
    /// Nancy module, test for now
    /// </summary>
    public class HelloModule : NancyModule {

        // List of web socket sessions
        private static List<WebSocketSession> m_Sessions = new List<WebSocketSession>();
        private static object m_SessionSyncRoot = new object();

        /// <summary>
        /// Websocket : send a message to all users
        /// </summary>
        /// <param name="message"></param>
        static void SendToAll(string message) {
            lock (m_SessionSyncRoot) {
                foreach (var s in m_Sessions) {
                    s.Send(message);
                }
            }
        }

        /// <summary>
        /// Message received
        /// </summary>
        /// <param name="session"></param>
        /// <param name="e"></param>
        static void socketServer_NewMessageReceived(WebSocketSession session, string e) {
            SendToAll(session.SessionID + ": " + e);
        }

        /// <summary>
        /// New user connected
        /// </summary>
        /// <param name="session"></param>
        static void socketServer_NewSessionConnected(WebSocketSession session) {
            lock (m_SessionSyncRoot)
                m_Sessions.Add(session);

            SendToAll("System: " + session.SessionID + " connected");
        }

        /// <summary>
        /// Session closed
        /// </summary>
        /// <param name="session"></param>
        /// <param name="reason"></param>
        static void socketServer_SessionClosed(WebSocketSession session, CloseReason reason) {
            lock (m_SessionSyncRoot)
                m_Sessions.Remove(session);

            if (reason == CloseReason.ServerShutdown)
                return;

            SendToAll("System: " + session.SessionID + " disconnected");
        }

        /// <summary>
        /// Nancy module constructor
        /// </summary>
        public HelloModule() {
            // Create the web socket server
            var socketServer = new WebSocketServer();
            socketServer.Setup(8181);
            socketServer.Start();

            // Create handler for every event
            socketServer.NewMessageReceived += new SessionHandler<WebSocketSession, string>(socketServer_NewMessageReceived);
            socketServer.NewSessionConnected += new SessionHandler<WebSocketSession>(socketServer_NewSessionConnected);
            socketServer.SessionClosed += new SessionHandler<WebSocketSession, CloseReason>(socketServer_SessionClosed);

            // Connect to local mongodb
            string conn = "mongodb://localhost/?safe=true";
            MongoClient client = new MongoClient(conn);
            IMongoDatabase db = client.GetDatabase("networkManager");

            // Collections
            IMongoCollection<ClientStation> clientCollections = db.GetCollection<ClientStation>("clients");

            // Define endpoints
            // -- Page endpoints

            Get["/login"] = _ => {
                // User want to log in, return the login view
                return View["login.html"];
            };

            Post["/login"] = _ => {
                var userGuid = UserDatabase.ValidateUser((string)this.Request.Form.Login, (string)this.Request.Form.Password);

                if (userGuid == null) {
                    return this.Context.GetRedirect("~/login");
                }

                DateTime? expiry = null;
                if (this.Request.Form.RememberMe.HasValue) {
                    expiry = DateTime.Now.AddDays(7);
                }

                return this.LoginAndRedirect(userGuid.Value, expiry);
            };

            Get["/logout"] = x => {
                return this.LogoutAndRedirect("~/");
            };

            Get["/"] = _ => {
                this.RequiresAuthentication();

                return View["index.html", this.Context.CurrentUser];
            };

            // TODO
            // real authentication
            // make any endpoint not for reporting protected by authentication
            // get and insert user from the database

            // API endpoints
            Get["/api"] = _ => {
                return "Hello World";
            };

            // -- Client endpoints
            // Return a filtered list of clients (unfiltered if no filter is given)
            Post["/api/clients"] = p => {
                ClientStationFilter clientFilter = this.Bind<ClientStationFilter>();

                // Build the filter
                var builder = Builders<ClientStation>.Filter;
                FilterDefinition<ClientStation> filter = null;

                // Date
                if (clientFilter.reportedLessThan > 0) {
                    filter = builder.Gte("lastReport", DateTime.Now.AddSeconds(-clientFilter.reportedLessThan));
                }

                // Os version
                if (clientFilter.hasOsVersion != null) {
                    var osFilter = builder.Eq("osVersion", clientFilter.hasOsVersion);
                    filter = filter != null ? filter & osFilter : osFilter;
                }

                // User connected
                if (clientFilter.hasUser != null) {
                    var userFilter = builder.Eq("users.login", clientFilter.hasUser);
                    filter = filter != null ? filter & userFilter : userFilter;
                }

                // Return the filtered elements
                if (filter == null)
                    return Response.AsJson(ApiResponse<List<ClientStation>>.Success(clientCollections.Find(_ => true).ToList()));
                else
                    return Response.AsJson(ApiResponse<List<ClientStation>>.Success(clientCollections.Find(filter).ToList()));
            };

            // Get a specified client
            Get["/api/client/{id}"] = p => {
                string id = p.id;
                var clientStation = clientCollections.Find(Builders<ClientStation>.Filter.Eq("id", id)).FirstOrDefault();

                if(clientStation == null) {
                    // 404 error
                    return Response.AsJson(ApiResponse<String>.Error("No client found with the provided id", 404), HttpStatusCode.NotFound);
                } else
                    return Response.AsJson(ApiResponse<ClientStation>.Success(clientStation));
            };

            // Delete a specified client (note that as soon as a client report, it will be re-created)
            Delete["/api/client/{id}"] = p => {
                string id = p.id;
                var clientStationDeleted = clientCollections.DeleteOne(Builders<ClientStation>.Filter.Eq("id", id)).DeletedCount;

                if(clientStationDeleted <= 0) {
                    // 404 error
                    return Response.AsJson(ApiResponse<String>.Error("No client found with the provided id", 404), HttpStatusCode.NotFound);
                } else
                    return Response.AsJson(ApiResponse<Object>.Success(null));
            };

            // -- Jobs endpoints
            // TODO create a job, edit a job, cancel a job, and get list of jobs with filtering

            // -- Reporting endpoints
            // Report endpoint. Every client should report to this endpoint
            Post["/api/report"] = _ => {
                Report report = this.Bind<Report>();

                // Get the client station
                var clientStation = clientCollections.Find(Builders<ClientStation>.Filter.Eq("id", report.id)).FirstOrDefault();

                if(clientStation == null) {
                    clientStation = new ClientStation(); // If the client doesn't exist
                    clientStation.id = report.id;
                }
                
                clientStation.macAddr = report.macAddr;
                clientStation.ipAddr = report.ipAddr;
                clientStation.osVersion = report.osVersion;
                clientStation.lastReport = DateTime.Now;
                clientStation.users = new List<ClientStationUser>();

                foreach (ReportUser user in report.users)
                    clientStation.users.Add(
                        new ClientStationUser() {
                            login = user.login,
                            duration = user.duration
                        });

                // Insert (or replace) the new client
                clientCollections.ReplaceOne(Builders<ClientStation>.Filter.Eq("id", report.id), clientStation, new UpdateOptions { IsUpsert = true });

                // TODO notify any listener for this station

                /// TODO get client job reports, and update
                /// jobs reported for the client

                /// TODO get client waiting jobs, and pass
                /// the order to performs those jobs

                return Response.AsJson(clientStation);
            };
        }
    }

    static class Program {
        /// <summary>
        /// Entry point
        /// </summary>
        static void Main() {
            // Allow nancy to register itself it URL
            HostConfiguration hostConfigs = new HostConfiguration();
            hostConfigs.UrlReservations.CreateAutomatically = true;

            using (var host = new NancyHost(new Uri("http://localhost:8080"))) {
                host.Start();

                Console.WriteLine("Nancy started !");
                Console.ReadLine();

                while(true) { }

                host.Stop();
            }

            //ServiceBase[] ServicesToRun;
            //ServicesToRun = new ServiceBase[] {
            //    new NetworkManagerService()
            //};
            //ServiceBase.Run(ServicesToRun);
        }
    }
}
