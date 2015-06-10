using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace MonkeyJobTool.Entities
{
    public class ApplicationConfiguration
    {
        public AppConfHotkeys HotKeys { get; set; }
        public List<CommandReplace> CommandReplaces { get; set; }
        public bool AllowUsingGoogleAnalytics { get; set; }
        public bool AllowSendCrashReports { get; set; }
        public bool ShowDonateButton { get; set; }
        public SystemData SystemData { get; set; }
        public Language Language { get; set; }

        public ApplicationConfiguration()
        {
            CommandReplaces = new List<CommandReplace>();
        }

        public void Save()
        {
            string json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(AppConstants.Paths.MainConfFileName, json);
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

        public SystemData()
        {
            DisabledModules = new List<string>();
        }
    }
}
