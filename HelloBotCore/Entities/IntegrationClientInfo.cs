using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelloBotCommunication;
using HelloBotCore.Manager;
using Newtonsoft.Json;

namespace HelloBotCore.Entities
{
    public class IntegrationClientInfo : IntegrationClientBase
    {
        private HelloBotCommunication.IntegrationClientBase baseClient = null;

        public Func<int?, IntegrationClientInfo> CreateNewInstanceFunc { get; set; }

         

        public IntegrationClientInfo(StorageManager storageManager) : base(storageManager)
        {
        }
        
        public override ModuleType ModuleType
        {
            get { return ModuleType.IntegrationClient; }
        }

              

        public override void SendMessageToClient(Guid token, CommunicationClientMessage message)
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

        public override void Dispose()
        {
           DeleteSettings();
        }

        public new MainComponentInstanceSettings GetSettings()
        {
            return GetSettings<MainComponentInstanceSettings>();
        }

        public override string GetTypeDescription()
        {
            return "Клиент";
        }
    }
}
