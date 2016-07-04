using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelloBotCommunication;
using HelloBotCommunication.Attributes.SettingAttributes;
using HelloBotCommunication.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Nigrimmist.TelegramIntegrationClient
{
    public class TelegramClient : IntegrationClientBase
    {
        private IIntegrationClient _client;
        public override void Init(IIntegrationClient integrationClient)
        {
            _client = integrationClient;
        }

        private TelegramBotClient bot;
        private bool botInited;
        private long chatId = -1;
        private int offset = 0;


        public override void HandleMessage(Guid token, CommunicationMessage message)
        {
            if (message == null) return;
            
            bool settingsShouldBeSave = false;
            TelegramSettings settings = null;
            if (bot == null && !botInited)
            {
                settings = _client.GetSettings<TelegramSettings>();
                if (settings != null && !string.IsNullOrEmpty(settings.Token))
                {
                    try
                    {
                        bot = new Api(settings.Token);
                    }
                    catch (Exception ex)
                    {
                        _client.ShowMessage(token, ex.Message);
                        return;
                    }
                    
                    offset = settings.Offset;
                    chatId = settings.ChatId;
                }
                else
                {
                    _client.ShowMessage(token,"Please, fill TOKEN to use telegram client or disable it");
                    return;
                }
                botInited = true;
            }
            if (botInited && bot!=null)
            {
                if (chatId == -1 || chatId == 0)
                {
                    var updates = bot.GetUpdatesAsync(offset).Result;
                    if (updates.Any())
                    {
                        chatId = updates.First().Message.Chat.Id;
                        settingsShouldBeSave = true;
                        SendMessage(token,"MonkeyJob connected! Thanks for using!");
                    }
                    else
                    {
                       _client.ShowMessage(token,"Please, send any message to bot in telegram if you would like to use it as client in MonkeyJob");
                        return;
                    }
                }

                if (settingsShouldBeSave)
                {
                    if(settings==null)
                        settings = _client.GetSettings<TelegramSettings>();
                    settings.ChatId = chatId;
                    settings.Offset = offset;
                    _client.SaveSettings(settings);
                }
                string answer = message.FromModule + " :" + Environment.NewLine;
                SendMessage(token,answer+message);
            }
        }

        private void SendMessage(Guid clientToken, string message)
        {
            try
            {
                var result = bot.SendTextMessageAsync(chatId, message).Result;
            }
            catch (Exception ex)
            {
                if (ex.InnerException.ToString().Contains("Invalid token"))
                    _client.ShowMessage(clientToken, "Invalid token. Check Telegram module settings");
                    
            }
            
        }

        public override string Title
        {
            get { return "Telegram"; }
        }

        public override string ModuleDescription { get { return "Пересылка уведомлений в Telegram. Для включения этого клиента вам необходимо создать своего собственного Telegram-бота. Посетите [url] для более детальной инструкции."; }  }
    }

    


    [ModuleSettingsFor(typeof(TelegramClient))]
    public class TelegramSettings
    {
        [SettingsNameField("Телеграм токен, который отдаёт BotFather")]
        public string Token { get; set; }

        public long ChatId { get; set; }
        public int Offset { get; set; }

        public TelegramSettings()
        {
                
        }
    }
}
