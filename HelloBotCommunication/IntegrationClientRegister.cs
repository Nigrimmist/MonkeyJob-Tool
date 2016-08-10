using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelloBotCommunication.Interfaces;

namespace HelloBotCommunication
{
    public abstract class IntegrationClientRegisterBase
    {
        public abstract AuthorInfo AuthorInfo { get; }
        public abstract List<IntegrationClientBase> GetIntegrationClients();
        
    }
}
