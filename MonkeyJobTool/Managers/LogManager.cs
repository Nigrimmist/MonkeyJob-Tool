using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Management;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using NLog;
using NLog.Internal;

namespace MonkeyJobTool.Managers
{
    public class LogManager
    {
        private static Logger _log = NLog.LogManager.GetCurrentClassLogger();
        public static void Error(Exception ex, string message = "")
        {
            if (Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["DevelopmentModeEnabled"]))
                MessageBox.Show(ex.ToString());



            string errorInfo = string.Format("{0} : \r\n {1} \r\n {2}", message, ex, CollectSystemInfo());

            _log.Error(errorInfo);
        }

        private static string CollectSystemInfo()
        {
            string frameworkInfo = ".net framework info : ";
            try
            {
                RegistryKey installed_versions = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP");
                string[] version_names = installed_versions.GetSubKeyNames();
                double framework = Convert.ToDouble(version_names[version_names.Length - 1].Remove(0, 1), CultureInfo.InvariantCulture);
                int sp = Convert.ToInt32(installed_versions.OpenSubKey(version_names[version_names.Length - 1]).GetValue("SP", 0));

                frameworkInfo += "v" + framework + " service pack:" + sp;
            }
            catch
            {
                frameworkInfo += "none";
            }

            string osInfo = "OS info : ";

            try
            {
                Console.WriteLine("Displaying operating system info....\n");
                //Create an object of ManagementObjectSearcher class and pass query as parameter.
                ManagementObjectSearcher mos = new ManagementObjectSearcher("select * from Win32_OperatingSystem");
                foreach (ManagementObject managementObject in mos.Get())
                {
                    if (managementObject["Caption"] != null)
                    {
                        osInfo += "Operating System Name  :  " + managementObject["Caption"] + Environment.NewLine;
                    }
                    if (managementObject["OSArchitecture"] != null)
                    {
                        osInfo += "Operating System Architecture  :  " + managementObject["OSArchitecture"] + Environment.NewLine;
                    }
                    if (managementObject["CSDVersion"] != null)
                    {
                        osInfo += "Operating System Service Pack   :  " + managementObject["CSDVersion"]+ Environment.NewLine;
                    }
                }
            }
            catch
            {
                osInfo += "none";
            }

            return frameworkInfo + " " + Environment.NewLine + osInfo + " " + Environment.NewLine;
        }
    }
}
