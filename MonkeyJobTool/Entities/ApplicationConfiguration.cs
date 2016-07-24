using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using HelloBotCore.Entities;
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
        public ApplicationConfiguration(bool initDefaults = false)
        {

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

        public List<ModuleToModuleCommunication> ClientToModulesCommunications { get; set; }

        public bool ModuleEnabledForModule(string mainModule, string dependentModule, ModuleType dependentModuleType)
        {
            var found = ClientToModulesCommunications.FirstOrDefault(x => x.MainModule == mainModule);

            return (found ?? ModuleToModuleCommunication.GetDefaultForClient(mainModule)).IsEnabledFor(dependentModule, dependentModuleType);
        }

        public ModuleToModuleCommunication GeToModuleCommunicationForClient(string clientName)
        {
            if (ClientToModulesCommunications != null)
            {
                var found = ClientToModulesCommunications.FirstOrDefault(x => x.MainModule == clientName);
                return found ?? ModuleToModuleCommunication.GetDefaultForClient(clientName);
            }
            else
                return ModuleToModuleCommunication.GetDefaultForClient(clientName);
        }

        public SystemData()
        {
            EnabledModules = new List<string>();
            DisabledModules = new List<string>();
            ClientToModulesCommunications = new List<ModuleToModuleCommunication>();
        }
    }

    public class ModuleToModuleCommunication
    {
        private bool _enabledForAll;
        public string MainModule { get; set; }
        public List<string> EnabledModules { get; set; }
        public List<string> DisabledModules { get; set; }
        public ModuleType? EnabledByType { get; set; }
        public ModuleType? DisabledByType { get; set; }

        public bool EnabledForAll
        {
            get { return _enabledForAll; }
            set
            {
                if (value)
                {
                    DisabledByType = null;
                    EnabledByType = null;
                    EnabledModules.Clear();
                    DisabledModules.Clear();
                }
                _enabledForAll = value;
            }
        }

        public bool IsEnabledFor(string moduleSystemName, ModuleType moduleType)
        {
            if (DisabledByType.HasValue && DisabledByType.Value == moduleType) return false;
            if (EnabledByType.HasValue && EnabledByType.Value == moduleType) return true;

            return (EnabledModules == null || EnabledModules.Contains(moduleSystemName)) || (DisabledModules == null || !DisabledModules.Contains(moduleSystemName));
        }

        public ModuleToModuleCommunication()
        {
            EnabledModules = new List<string>();
            DisabledModules = new List<string>();
        }

        public static ModuleToModuleCommunication GetDefaultForClient(string clientModule)
        {
            return new ModuleToModuleCommunication()
            {
                MainModule = clientModule,
                EnabledForAll = false,
                EnabledByType = ModuleType.Event
            };
        }
    }
}