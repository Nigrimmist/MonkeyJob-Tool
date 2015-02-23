using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace HelloBotCommunication
{
    public class CallCommandInfo
    {
        public string Command { get; set; }
        public string CommandDescription { get; set; }

        public CallCommandInfo(string command)
        {
            Command = command;
        }

        public CallCommandInfo(string command, string commandDescription)
        {
            Command = command;
            CommandDescription = commandDescription;
        }
    }
}
