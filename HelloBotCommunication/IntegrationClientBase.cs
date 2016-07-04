using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelloBotCommunication.Interfaces;

namespace HelloBotCommunication
{
    public abstract class IntegrationClientBase : ComponentBase
    {
        public virtual void Init(IIntegrationClient integrationClient)
        {
            
        }
        public abstract void HandleMessage(Guid token,CommunicationMessage message);
        public virtual string ModuleDescription { get { return null; } }
    }
}
