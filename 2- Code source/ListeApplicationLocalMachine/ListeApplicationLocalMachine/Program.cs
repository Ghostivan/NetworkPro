using Cassia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Management.Automation.Runspaces;
using System.Management.Automation;
using System.Collections.ObjectModel;
using System.IO;

namespace ListeApplicationLocalMachine
{
    class Program
    {
        private static Runspace runSpace;
        private static Pipeline pipeline;
        private static List<ClientApplication> listApp = new List<ClientApplication>();

        static void Main(string[] args)
        {
            runSpace = RunspaceFactory.CreateRunspace();
            runSpace.Open();
            pipeline = runSpace.CreatePipeline();
            getAllLocalApplication();
        }

        static void getAllLocalApplication()
        {
            Command invokeScript = new Command("Invoke-Command");
            RunspaceInvoke invoke = new RunspaceInvoke();

            ScriptBlock sb = invoke.Invoke("{Get-ItemProperty HKLM:\\SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\* | select DisplayName, Publisher, InstallDate, DisplayVersion}")[0].BaseObject as ScriptBlock;
            invokeScript.Parameters.Add("scriptBlock", sb);
  
            pipeline.Commands.Add(invokeScript);
            Collection<PSObject> output = pipeline.Invoke();
            foreach (PSObject psObject in output)
            {
                
                String appUpdate = (String)psObject.Properties["DisplayName"].Value;
                if (appUpdate != null)
                {
                    if (appUpdate.Contains("Update"))
                    {
                        if (appUpdate.Contains("Google"))
                        {
                            // MISE A JOUR WINDOWS
                        }
                   }else
                    {
                        if (psObject.Properties["InstallDate"].Value != null)
                        {
                            if (Convert.ToString(psObject.Properties["InstallDate"].Value).Substring(0, 1) != "2")
                            {
                                listApp.Add(new ClientApplication((String)psObject.Properties["DisplayName"].Value, (String)psObject.Properties["Publisher"].Value, (String)psObject.Properties["DisplayVersion"].Value));
                            }
                            else
                            {
                                DateTime dt = ConvertToDate(Convert.ToString(psObject.Properties["InstallDate"].Value));
                                listApp.Add(new ClientApplication((String)psObject.Properties["DisplayName"].Value, (String)psObject.Properties["Publisher"].Value, (String)psObject.Properties["DisplayVersion"].Value, ConvertToDate(Convert.ToString(psObject.Properties["InstallDate"].Value))));
                            }
                        }
                    }
                }
            }
            foreach (ClientApplication item in listApp)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("--- APPLICATION '"+item.Name);
                Console.ResetColor();
                Console.WriteLine("' --- \n Version =>" + item.Version + "\n Editeur =>" + item.Editor + "\n Date d'installation =>" + item.CreationDate + "\n\n");
            }
            Console.ReadLine();
        }

        public static DateTime ConvertToDate(string _str)
        {
            int y = Convert.ToInt32(_str.Substring(0, 4));
            int m = Convert.ToInt32(_str.Substring(4, 2));
            int d = Convert.ToInt32(_str.Substring(6, 2));
            return new DateTime(y,m,d);
        }
    }
}
//Invoke-Command -scriptBlock {"D:\" |dir}
//Invoke-Command -cn tsc-dc01, tsc-mail -ScriptBlock {Get-ItemProperty HKLM:\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\* | select DisplayName, Publisher, InstallDate }
//ScriptBlock sb = invoke.Invoke("{\"D:\\\" |dir}")[0].BaseObject as ScriptBlock;