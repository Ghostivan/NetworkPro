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
            
            
            TaskS ts = new TaskS();
            //ts.plannifAction("SpotifyInstaller", @"C:\Users\Damien\Downloads\SpotifySetup.exe", "s", DateTime.Now.AddSeconds(2)/*new DateTime(2016, 4, 25, 00, 56, 00)*/, "Pour tester l'installation de spotify");
            //ts.plannifAction("CamStudio", @"C:\Users\Damien\Downloads\camstudio.exe", "", DateTime.Now.AddSeconds(10), "Instalation Cam Studio");


            
            ts.listTask();
            Console.ReadKey();
        }
    }
}

//ts.plannifAction("notepad", @"C:\WINDOWS\system32\notepad.exe", new DateTime(2016, 4, 10, 20, 01, 00), "Pour tester lancement du bloc note");