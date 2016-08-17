using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloBotCore.Entities
{
    public class IntegrationClientSettings
    {
        public List<int> Instances { get; set; }
        
        public IntegrationClientSettings()
        {
            Instances = new List<int>();
        }
    }
}
