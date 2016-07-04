using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelloBotCommunication;

namespace HelloBotCore.Entities
{
    public class IntegrationClientInfo : IntegrationClientBase
    {
        private HelloBotCommunication.IntegrationClientBase baseClient = null;
        public IntegrationClientInfo(string settingsFolderAbsolutePath, string logsFolderAbsolutePath) : base(settingsFolderAbsolutePath, logsFolderAbsolutePath)
        {

        }

        public override string ToString(bool includingAuthorInfo = true)
        {
            return base.ToString(includingAuthorInfo);
        }

        public override string GetTypeDescription()
        {
            return "";
        }

        public override void SendMessageToClient(Guid token, CommunicationMessage message)
        {
            baseClient.HandleMessage(token,message);
        }

        public void Init(string dllName, HelloBotCommunication.IntegrationClientBase integrationClientBase, IModuleClientHandler moduleClientHandler, AuthorInfo author)
        {
            baseClient = integrationClientBase;
            base.Init(dllName, baseClient, author);
            HelloBotCommunication.Interfaces.IIntegrationClient client = new ModuleToClientAdapter(moduleClientHandler, this);
            integrationClientBase.Init(client);
        }
    }
}
