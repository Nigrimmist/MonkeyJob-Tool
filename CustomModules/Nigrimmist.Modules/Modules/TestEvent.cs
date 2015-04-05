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
            _client.ShowMessage(eventToken, string.Format("Тестовое уведомление. Будет появляться раз в час")).OnClick(() =>
            {
                _client.ShowMessage(eventToken, "Левый клик обработан");
            }).OnClosed(() =>
            {
                _client.ShowMessage(eventToken, "Закрытие обработано");
            });
        }

        public override string ModuleTitle
        {
            get { return "Тестовый заголовок"; }
        }
    }
}
