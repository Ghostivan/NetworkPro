using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace NetworkManager {
    public partial class NetworkManagerService : ServiceBase {
        public NetworkManagerService() {
            InitializeComponent();
        }

        protected override void OnStart(string[] args) {
            Console.WriteLine("Bonjour le monde");
        }

        protected override void OnStop() {
        }
    }
}
