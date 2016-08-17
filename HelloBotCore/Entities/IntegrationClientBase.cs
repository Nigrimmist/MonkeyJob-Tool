using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelloBotCommunication;

namespace HelloBotCore.Entities
{
    public abstract class IntegrationClientBase : ComponentInfoBase
    {
        private ClientInstanceToModuleCommunication _instanceCommunication;

        protected IntegrationClientBase(string settingsFolderAbsolutePath, string logsFolderAbsolutePath) : base(settingsFolderAbsolutePath, logsFolderAbsolutePath)
        {
        }

        public ClientInstanceToModuleCommunication InstanceCommunication
        {
            get { return _instanceCommunication??ClientInstanceToModuleCommunication.GetDefault(); }
            set { _instanceCommunication = value; }
        }

        public abstract void SendMessageToClient(Guid token, CommunicationClientMessage message);

        public abstract void Dispose();

    }
}
