using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloBotCore.Entities
{
    public class MainComponentInstanceSettings
    {
        public List<int> Instances { get; set; }
        
        public MainComponentInstanceSettings()
        {
            Instances = new List<int>() ;
        }
    }
}
