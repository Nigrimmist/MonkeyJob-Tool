using System.Collections.Generic;

namespace MonkeyJobTool.Entities
{
    public class ApplicationConfiguration
    {
        public AppConfHotkeys HotKeys { get; set; }
        public List<CommandReplace> CommandReplaces { get; set; }
        public bool AllowUsingGoogleAnalytics { get; set; }

        public ApplicationConfiguration()
        {
            CommandReplaces = new List<CommandReplace>();
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
}
