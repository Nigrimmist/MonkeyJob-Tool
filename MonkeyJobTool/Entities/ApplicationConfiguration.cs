using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using HelloBotCore.Entities;
using MonkeyJobTool.Managers;
using MonkeyJobTool.Managers.Interfaces;
using Newtonsoft.Json;

namespace MonkeyJobTool.Entities
{

    /// <summary>
    /// Adding new settings, be sure that it has been inited in constructor and Migrations for new version was created.
    /// </summary>
    public class ApplicationConfiguration
    {
        private IStorageManager _storageManager;
        public IStorageManager StorageManager{
            set => _storageManager = value;
        }

        public AppConfHotkeys HotKeys { get; set; }
        public List<CommandReplace> CommandReplaces { get; set; }
        public bool AllowUsingGoogleAnalytics { get; set; }
        public bool AllowSendCrashReports { get; set; }
        public bool ShowDonateButton { get; set; }
        public SystemData SystemData { get; set; }
        public Language Language { get; set; }
        public bool DevelopmentModeEnabled { get; set; }
        public bool DebugModeEnabled { get; set; }
        public double InstalledAppVersion { get; set; }
        public bool ShowCommandHelp { get; set; }
        public double ConfigVersion { get; set; }

        

        /// <summary>
        /// Defaults will be used for clear first installation
        /// </summary>
        public ApplicationConfiguration(IStorageManager storageManager=null, bool initDefaults = false)
        {
            _storageManager = storageManager;
            CommandReplaces = new List<CommandReplace>();
            SystemData = new SystemData();
            if (initDefaults)
            {
                HotKeys = new AppConfHotkeys() {ProgramOpen = "CTRL+M"};
                AllowUsingGoogleAnalytics = true;
                AllowSendCrashReports = true;
                ShowDonateButton = true;
                ShowCommandHelp = true;
                Language = Language.ru;
                SystemData = new SystemData() {EnabledModules = new List<string>(), ClearCommandAfterMinOfInactivity = TimeSpan.FromMinutes(5)};
                DevelopmentModeEnabled = true;
                InstalledAppVersion = AppConstants.AppVersion;
                ConfigVersion = AppConstants.ConfigVersion;
            }
        }

        

        public void Save()
        {
            LogManager.Trace("Start AppConf.Save()");
            _storageManager.Save(AppConstants.Paths.MainConfFileName, this);
            LogManager.Trace("End AppConf.Save()");

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
        /// <summary>
        /// For Event Based/tray modules. they are disabled by default
        /// </summary>
        public List<string> EnabledModules { get; set; }

        /// <summary>
        /// For command modules. they are enabled by default
        /// </summary>
        public List<string> DisabledModules { get; set; }

        public bool DoNotNotify { get; set; }
        public TimeSpan ClearCommandAfterMinOfInactivity { get; set; }
        public SystemData()
        {
            EnabledModules = new List<string>();
            DisabledModules = new List<string>();
        }
    }

    
}