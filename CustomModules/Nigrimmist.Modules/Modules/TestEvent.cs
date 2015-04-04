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
            get { return TimeSpan.FromMinutes(10); }
        }

        public override void OnFire(Guid eventToken)
        {
            _client.ShowMessage(eventToken, string.Format("Any content {0}", "s"),"test title");
        }

        public override string ModuleTitle
        {
            get { return "Title"; }
        }
    }
}
