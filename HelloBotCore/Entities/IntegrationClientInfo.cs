using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelloBotCommunication;
using Newtonsoft.Json;

namespace HelloBotCore.Entities
{
    public class IntegrationClientInfo : IntegrationClientBase
    {
        private HelloBotCommunication.IntegrationClientBase baseClient = null;

        public Func<int?, IntegrationClientInfo> CreateNewInstanceFunc { get; set; }


        public List<IntegrationClientBase> Instances { get; set; }

        public IntegrationClientInfo(string settingsFolderAbsolutePath, string logsFolderAbsolutePath) : base(settingsFolderAbsolutePath, logsFolderAbsolutePath)
        {
            Instances = new List<IntegrationClientBase>();
        }
        
        public override ModuleType ModuleType
        {
            get { return ModuleType.IntegrationClient; }
        }

        public override string ToString(bool includingAuthorInfo = true)
        {
            return base.ToString(includingAuthorInfo);
        }

        public override string GetTypeDescription()
        {
            return "";
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
            var settingsFilePath = GetSettingFileFullPath();
            if (File.Exists(settingsFilePath))
            {
                File.Delete(settingsFilePath);
            }
        }

        public IntegrationClientSettings GetSettings()
        {
            return GetSettings<IntegrationClientSettings>();
        }
    }
}
