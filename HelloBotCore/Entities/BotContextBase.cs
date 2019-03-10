using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelloBotCore.Entities
{
    public class BotContextBase 
    {
        public ComponentType ComponentType { get; set; }
        public Guid ModuleId { get; set; }
        public string CommandName { get; set; }
    }
}
