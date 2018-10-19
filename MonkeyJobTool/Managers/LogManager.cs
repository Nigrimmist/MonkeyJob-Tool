﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using MonkeyJobTool.Entities;
using MonkeyJobTool.Helpers;
using NLog;
using NLog.Internal;

namespace MonkeyJobTool.Managers
{
    public class LogManager
    {
        private static Logger _log = NLog.LogManager.GetCurrentClassLogger();
        private static string _systemInfo;

        public static void Error(Exception ex, string message = "")
        {
            var logError = true;
            if (ex is WebException || ex is SmtpException)
            {
                if (!InternetChecker.IsInternetEnabled())
                {
                    logError = false;
                }
            }
            if (logError)
            {
                if (App.Instance.AppConf.DevelopmentModeEnabled)
                    MessageBox.Show(ex.ToString());
                else if (App.Instance.AppConf.AllowSendCrashReports)
                {
                    if (_systemInfo == null)
                    {
                        _systemInfo = CollectSystemInfo();
                    }

                    string errorInfo = $"{message} : \r\n {ex} \r\n {_systemInfo}\r\n App version : {AppConstants.AppVersion}\r\n BotCore version : {(App.Instance.Bot != null ? App.Instance.Bot.Version.ToString() : "null")}";
                    if(App.Instance.AppConf.DebugModeEnabled)
                        _log.Warn(errorInfo);
                    else
                        _log.Error(errorInfo);
                }
            }
        }

        public static void Trace(string msg)
        {
            if (App.Instance.AppConf.DebugModeEnabled)
            {
                StackFrame frame = new StackFrame(1);
                var method = frame.GetMethod();
                var type = method.DeclaringType;
                var name = method.Name;
                
                _log.Warn("{0}.{1} : {2}", type, name, msg);
            }
            
        }

        public static string CollectSystemInfo()
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
            string pcId = "pcId : " + App.Instance.UserID;
            return frameworkInfo + " " + Environment.NewLine + osInfo + " " + Environment.NewLine + pcId+Environment.NewLine;
        }
    }
}
