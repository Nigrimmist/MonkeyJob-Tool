using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelloBotCore.Entities
{
    public class CallCommandInfo
    {
        
        public string Command { get; set; }
        public string Description { get; set; }
        public List<string> Aliases { get; set; }

        public List<CommandArgumentSuggestionInfo> CommandArgumentSuggestions { get; set; }

        public CallCommandInfo(HelloBotCommunication.CallCommandInfo moduleCommandInfo)
        {
            Command = moduleCommandInfo.Command;
            Description = moduleCommandInfo.Description;
            Aliases = moduleCommandInfo.Aliases;

            if (moduleCommandInfo.CommandArgumentSuggestions != null)
                CommandArgumentSuggestions = moduleCommandInfo.CommandArgumentSuggestions.Select(x => new CommandArgumentSuggestionInfo(x)).ToList();
        }

        public CallCommandInfo(string command, string description)
        {
            Command = command;
            Description = description;
            Aliases = new List<string>();
        }
        
    }
}
