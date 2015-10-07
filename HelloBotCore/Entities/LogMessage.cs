using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelloBotCore.Entities
{
    public class LogMessage
    {
        public string Message { get; set; }
        public DateTime Date { get; set; }

        public LogMessage()
        {
            Date = DateTime.Now;
        }
    }
}
