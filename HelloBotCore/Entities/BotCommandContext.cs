using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelloBotCore.Entities
{
    public class BotCommandContext
    {
        public string CommandName { get; set; }
        public ClientCommandContext ClientCommandContext { get; set; }
        public Action OnNotifiedAction { get; set; }
        public Action OnClickAction { get; set; }
        public Action OnIgnoreAction { get; set; }
    }
}
