using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelloBotCommunication;
using HelloBotCommunication.Interfaces;

namespace Nigrimmist.TelegramIntegrationClient
{
    public class ClientRegisterBase : IntegrationClientRegisterBase
    {
        public override AuthorInfo AuthorInfo
        {
            get { return new AuthorInfo("Nigrimmist", "Nigrimmist@gmail.com", "Nigrimmist+MJ-ModuleLogs@gmail.com"); }
        }

        public override List<IntegrationClientBase> GetIntegrationClients()
        {
            return new List<IntegrationClientBase>()
            {
                new TelegramClient()
            };
        }
    }
}
