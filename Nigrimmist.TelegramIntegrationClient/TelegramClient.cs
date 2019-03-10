using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HelloBotCommunication;
using HelloBotCommunication.Attributes.SettingAttributes;
using HelloBotCommunication.Interfaces;
using HelloBotModuleHelper;
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

        public override double ActualSettingsVersion
        {
            get { return 1.1; }
        }

        private List<ClientTelegramWrapper> _bots = null; 

        public override void HandleMessage(Guid token, CommunicationClientMessage message)
        {
            if (message == null) return;
            TelegramSettings settings = null;

            if (_bots == null)
            {
                _bots = new List<ClientTelegramWrapper>();
                settings = _client.GetSettings<TelegramSettings>();
                foreach (var bot in settings.Bots)
                {
                    _bots.Add(new ClientTelegramWrapper()
                    {
                        Token = bot.Token,
                        ChatId = bot.ChatId,
                        ShowTitle = bot.ShowTitle
                    });
                }
            }

            bool settingsShouldBeSave = false;
            if (_bots.Any())
            {
                foreach (var bot in _bots)
                {
                    if (bot.Bot == null && !bot.BotInited)
                    {
                        _client.LogTrace("Init start");
                        if (settings == null)
                            settings = _client.GetSettings<TelegramSettings>();
                        if (!string.IsNullOrEmpty(bot.Token))
                        {
                            try
                            {
                                bot.Bot = new Api(bot.Token);
                                
                            }
                            catch (Exception ex)
                            {
                                _client.LogTrace("bot init exception : " + ex);
                                _client.ShowMessage(token, ex.Message);
                                return;
                            }

                            _client.LogTrace("offset : " + bot.Offset + " chatId" + bot.ChatId);
                        }
                        else
                        {
                            _client.ShowMessage(token, "Please, fill TOKEN to use telegram client or disable it");
                            return;
                        }
                        bot.BotInited = true;
                    }
                    if (bot.BotInited && bot.Bot != null)
                    {
                        if (string.IsNullOrEmpty(bot.ChatId))
                        {
                            var updates = bot.Bot.GetUpdatesAsync(bot.Offset).Result;
                            _client.LogTrace("updates received");
                            if (updates.Any())
                            {
                                bot.ChatId = updates.First().Message.Chat.Id.ToString();
                                _client.LogTrace("chatId found " + bot.ChatId);
                                settingsShouldBeSave = true;
                                SendMessage(bot.Bot, bot.ChatId, token, "MonkeyJob connected! Thanks for using!", null);
                            }
                            else
                            {
                                _client.ShowMessage(token, "Please, send any message to bot in telegram from you telegram client if you would like to use it as a client in MonkeyJob. And try again");
                                return;
                            }
                        }

                        if (settingsShouldBeSave)
                        {
                            if (settings == null)
                                settings = _client.GetSettings<TelegramSettings>();
                            var foundBot = settings.Bots.SingleOrDefault(x => x.Token == bot.Token);
                            foundBot.ChatId = bot.ChatId;
                            foundBot.Offset = bot.Offset;
                            _client.SaveSettings(settings);
                        }
                        string answer = "";
                        if (bot.ShowTitle)
                            answer = message.FromModule + " :" + Environment.NewLine;
                        SendMessage(bot.Bot,bot.ChatId,token, answer, message);
                    }
                }
                
            }
            
        }


        private void SendMessage(TelegramBotClient bot, string chatId, Guid clientToken,string msgPrefix, CommunicationClientMessage message)
        {
            try
            {
                if (message!=null && message.MessageParts.Any(x => x.MessageFormat == CommunicationMessageFormat.ImageUrl))
                {
                    Exception ex;
                    using (var str = new MemoryStream())
                    {
                        string urlToDownload = message.MessageParts.SingleOrDefault(x => x.MessageFormat == CommunicationMessageFormat.ImageUrl).Value.ToString();
                        var downloadRes =
                            new HtmlReaderManager().TryDownloadFileByUrl(urlToDownload, str,
                                out ex);
                        str.Flush();
                        str.Seek(0, SeekOrigin.Begin);
                        if (downloadRes)
                        {
                            string caption = message.GetTextContent();
                            long id;
                            if (!chatId.StartsWith("@") && long.TryParse(chatId, out id))
                            {
                                var result = bot.SendPhotoAsync(id, new FileToSend("moto.jpeg", str), caption).Result;
                            }
                            else
                            {
                                var result = bot.SendPhotoAsync(chatId, new FileToSend("moto.jpeg", str), caption).Result;
                                
                            }
                        }
                        else
                        {
                            _client.LogTrace("download file exception " + ex);
                        }

                    }

                }
                else
                {
                    long id;
                    if (!chatId.StartsWith("@") && long.TryParse(chatId, out id))
                    {
                        var result = bot.SendTextMessageAsync(id, message==null?msgPrefix:msgPrefix + message).Result;
                    }
                    else
                    {
                        var result = bot.SendTextMessageAsync(chatId, message == null ? msgPrefix : msgPrefix + message).Result;
                    }
                }

            }
            catch (Exception ex)
            {
                _client.LogTrace("send msg exception "+ex);
                if (ex.InnerException.ToString().Contains("Invalid token"))
                    _client.ShowMessage(clientToken, "Invalid token. Check Telegram module settings");
                    
            }
            
        }

        public override string Title
        {
            get { return "Telegram"; }
        }

        public override string ModuleDescription { get { return "Пересылка уведомлений в Telegram. Для включения этого клиента вам необходимо создать своего собственного Telegram-бота. Посетите https://core.telegram.org/bots#3-how-do-i-create-a-bot для более детальной инструкции."; } }
    }

    


    [ModuleSettingsFor(typeof(TelegramClient))]
    public class TelegramSettings
    {
        [SettingsNameField("Боты")]
        public List<TelegramBotSettings> Bots { get; set; } 

        public TelegramSettings()
        {
            Bots = new List<TelegramBotSettings>();
        }
    }

    public class TelegramBotSettings
    {
        [SettingsNameField("Телеграм токен, который отдаёт BotFather")]
        public string Token { get; set; }

        [SettingsNameField("Показывать системный TITLE перед каждым сообщением?")]
        public bool ShowTitle { get; set; }

        [SettingsNameField("Если да, то введите id канала")]
        public string ChatId { get; set; }
        public int Offset { get; set; }

        public TelegramBotSettings()
        {
            
        }
    }

    class ClientTelegramWrapper
    {
        public bool BotInited { get; set; }
        public string Token { get; set; }
        public int Offset { get; set; }
        public string ChatId { get; set; }
        public TelegramBotClient Bot { get; set; }
        public bool ShowTitle { get; set; }

        public ClientTelegramWrapper()
        {
            
        }

        
    }
}
