using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HelloBotCommunication;
using HelloBotCore.Entities;
using CallCommandInfo = HelloBotCore.Entities.CallCommandInfo;

namespace MonkeyJobTool.Entities.Autocomplete
{
    public class DataFilterInfo
    {
        public List<CallCommandInfo> FoundCommands { get; set; }
        public string FoundByTerm { get; set; }
    }
}
