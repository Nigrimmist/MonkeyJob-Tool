using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HelloBotCommunication;
using HelloBotCommunication.Interfaces;

namespace Nigrimmist.Modules.Modules
{
    public class TestEvent : ModuleEventBase
    {
        private IClient _client;


        public override void Init(IClient client)
        {
            _client = client;
        }

        public override TimeSpan RunEvery
        {
            get { return TimeSpan.FromSeconds(5); }
        }

        public override void OnFire(Guid eventToken)
        {
            //_client.ShowMessage(eventToken, "Any content " + DateTime.Now,"test title");
        }
    }
}
