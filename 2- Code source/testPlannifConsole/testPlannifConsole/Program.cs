using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Microsoft.Win32.TaskScheduler;

namespace testPlannifConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            
            //TaskS ts = new TaskS("notepad", @"C:\WINDOWS\system32\notepad.exe", new DateTime(2016, 4, 10, 20, 01, 00), "Pour tester lancement du bloc note");
            TaskS ts = new TaskS("SpotifyInstaller", @"C:\Users\Damien\Downloads\SpotifySetup.exe", "-s -qn -noreeboot", DateTime.Now.AddSeconds(2)/*new DateTime(2016, 4, 25, 00, 56, 00)*/, "Pour tester l'installation de spotify");
            //TaskS ts = new TaskS("shutdown", "shutdown.exe","-s -t 300", DateTime.Now.AddSeconds(2)/*new DateTime(2016, 5, 17, 00, 12, 00)*/, "Pour tester shutdown avec paramètres");
            ts.listTask();
            Console.ReadKey();
        }
    }
}