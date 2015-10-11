using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace MonkeyJobTool.Entities
{

    /// <summary>
    /// Adding new settings, be sure that it has been inited in constructor and Migrations for new version was created.
    /// </summary>
    public class ApplicationConfiguration
    {
        public AppConfHotkeys HotKeys { get; set; }
        public List<CommandReplace> CommandReplaces { get; set; }
        public bool AllowUsingGoogleAnalytics { get; set; }
        public bool AllowSendCrashReports { get; set; }
        public bool ShowDonateButton { get; set; }
        public SystemData SystemData { get; set; }
        public Language Language { get; set; }
        public bool DevelopmentModeEnabled { get; set; }
        public double InstalledAppVersion { get; set; }
        public bool ShowCommandHelp { get; set; }
        public double ConfigVersion { get; set; }

        /// <summary>
        /// Defaults will be used for clear first installation
        /// </summary>
        public ApplicationConfiguration(bool initDefaults=false)
        {
            
            CommandReplaces = new List<CommandReplace>();
            SystemData = new SystemData() {DisabledModules = new List<string>()};
            if (initDefaults)
            {
                HotKeys = new AppConfHotkeys()
                {
                    ProgramOpen = "CTRL+M"
                };
                AllowUsingGoogleAnalytics = true;
                AllowSendCrashReports = true;
                ShowDonateButton = true;
                ShowCommandHelp = true;
                Language = Language.ru;
                SystemData = new SystemData()
                {
                    DisabledModules = new List<string>()
                    {
                        "Nigrimmist.Modules.PingModule",
                        "Nigrimmist.Modules.WeatherTrayModule",
                        "Nigrimmist.Modules.MemoryUsageTrayModule"
                    },
                    ClearCommandAfterMinOfInactivity = TimeSpan.FromMinutes(5)
                };
                DevelopmentModeEnabled = true;
                InstalledAppVersion = AppConstants.AppVersion;
                ConfigVersion = AppConstants.ConfigVersion;
            }
        }

        public void Save()
        {
            string json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(App.Instance.ExecutionFolder + AppConstants.Paths.MainConfFileName, json);
        }
    }
    
    public class AppConfHotkeys
    {
        public string ProgramOpen { get; set; }
    }

    public class CommandReplace
    {
        public string From { get; set; }
        public string To { get; set; }
    }

    public class SystemData
    {
        public List<string>  DisabledModules { get; set; }
        public bool DoNotNotify { get; set; }
        public TimeSpan ClearCommandAfterMinOfInactivity { get; set; }
    }
}
