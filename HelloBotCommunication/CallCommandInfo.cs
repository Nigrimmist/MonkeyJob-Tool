using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;


namespace HelloBotCommunication
{
    public class CallCommandInfo
    {
        /// <summary>
        /// Command name. That name will be displayed to user
        /// </summary>
        public string Command { get; set; }
        /// <summary>
        /// Command description. You can override General description for handler by filling that field.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Command alias word list. using for command alternative search
        /// </summary>
        public List<string> Aliases { get; set; }

        /// <summary>
        /// Specify argument suggestions for your command. Optional
        /// </summary>
        public ReadOnlyCollection<ArgumentSuggestionInfo> CommandArgumentSuggestions { get; set; }

        public CallCommandInfo(string command, ReadOnlyCollection<ArgumentSuggestionInfo> commandArgumentSuggestions=null)
        {
            Command = command;
            Aliases = new List<string>();
            CommandArgumentSuggestions = commandArgumentSuggestions;
        }

        public CallCommandInfo(string command, string description, ReadOnlyCollection<ArgumentSuggestionInfo> commandArgumentSuggestions = null)
        {
            Command = command;
            Description = description;
            Aliases = new List<string>();
            CommandArgumentSuggestions = commandArgumentSuggestions;
        }

        public CallCommandInfo(string command, List<string> aliases, ReadOnlyCollection<ArgumentSuggestionInfo> commandArgumentSuggestions = null)
        {
            Command = command;
            Aliases = aliases;
            CommandArgumentSuggestions = commandArgumentSuggestions;
        }

        
    }
}
