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
        protected IntegrationClientBase(string settingsFolderAbsolutePath, string logsFolderAbsolutePath) : base(settingsFolderAbsolutePath, logsFolderAbsolutePath)
        {
        }

        public ClientInstanceToModuleCommunication InstanceCommunication { get; set; }

        public abstract void SendMessageToClient(Guid token, CommunicationClientMessage message);

        

    }
}
