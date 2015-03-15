using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelloBotCore.Entities
{
    public class ClientCommandContext
    {
        public bool IsToBuffer { get; set; }
        public DateTime CreateDate { get; private set; }

        public ClientCommandContext()
        {
            CreateDate = DateTime.Now;
        }
    }
}
