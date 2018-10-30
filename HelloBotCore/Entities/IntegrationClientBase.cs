using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelloBotCommunication;
using HelloBotCore.Manager;

namespace HelloBotCore.Entities
{
    public abstract class IntegrationClientBase : ComponentInfoBase
    {
        private ClientInstanceToModuleCommunication _instanceCommunication;

        protected IntegrationClientBase(StorageManager storageManager) : base(storageManager)
        {
        }

        public ClientInstanceToModuleCommunication InstanceCommunication
        {
            get { return _instanceCommunication??ClientInstanceToModuleCommunication.GetDefault(); }
            set { _instanceCommunication = value; }
        }

        public abstract void SendMessageToClient(Guid token, CommunicationClientMessage message);       

    }
}
