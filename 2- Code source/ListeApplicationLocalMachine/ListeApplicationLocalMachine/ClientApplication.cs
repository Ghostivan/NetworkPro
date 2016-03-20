using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListeApplicationLocalMachine
{
    public class ClientApplication
    {
        public String Name { get; set; }
        public String Version { get; set; }
        public String Editor { get; set; }
        public DateTime CreationDate { get; set; }

        public ClientApplication(String _name, String _editor, String _version)
        {
            this.Name = _name;
            this.Version = _version;
            this.Editor = _editor;
        }
        public ClientApplication(String _name, String _editor, String _version, DateTime _creationdate)
        {
            this.Name = _name;
            this.Version = _version;
            this.Editor = _editor;
            this.CreationDate = _creationdate;
        }
    }
}
