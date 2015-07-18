using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelloBotCore.Entities
{
    public class BotContextBase 
    {
        public ModuleType ModuleType { get; set; }
        public Guid ModuleId { get; set; }
    }
}
