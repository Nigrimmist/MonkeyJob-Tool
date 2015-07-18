using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelloBotCore.Entities
{
    public class BotCommandContext : BotContextBase
    {
        public string CommandName { get; set; }
        public ClientCommandContext ClientCommandContext { get; set; }
        public Action OnClosedAction { get; set; }
        public Action OnClickAction { get; set; }
        
        public void ClearHandlers()
        {
            OnClosedAction = null;
            OnClickAction = null;
        }
    }
}
