using System;
using System.Collections.Generic;
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

        

        public CallCommandInfo(string command)
        {
            Command = command;
            Aliases = new List<string>();
        }
        
        public CallCommandInfo(string command, string description)
        {
            Command = command;
            Description = description;
            Aliases = new List<string>();
            
        }

        public CallCommandInfo(string command, List<string> aliases)
        {
            Command = command;
            Aliases = aliases;
        }

        
    }
}
