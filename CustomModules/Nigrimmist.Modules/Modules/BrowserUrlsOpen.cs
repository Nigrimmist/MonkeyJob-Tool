using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using HelloBotCommunication;
using HelloBotCommunication.Attributes.SettingAttributes;
using HelloBotCommunication.Interfaces;

namespace Nigrimmist.Modules.Modules
{
    public class BrowserUrlsOpen: ModuleHandlerBase
    {
        private IClient _client;
        private List<CommandKeyValue> _commandUrls;

        private ReadOnlyCollection<CallCommandInfo> _callCommandList;
        public override ReadOnlyCollection<CallCommandInfo> CallCommandList{get { return _callCommandList; }}
        
        public override DescriptionInfo ModuleDescription
        {
            get
            {
                return new DescriptionInfo()
                {
                    Description = @"Открывает вашу ссылку ссылку в браузере. Поддерживаются форматированные урлы вида http://url/?blabla={0}. Любые изменения настроек требуют перезапуска программы."
                };
            }
        }
        public override double ModuleVersion { get { return 1.0; } }

        public override string ModuleTitle
        {
            get { return "Ссылкооткрыватель"; }
        }

        public override void Init(IClient client)
        {
            _client = client;
            var existSettings = _client.GetSettings<BrowserUrlsOpenSettings>();
            if (existSettings == null)
            {
                //let's save default settings
                existSettings = new BrowserUrlsOpenSettings()
                {
                    Commands = new List<CommandKeyValue>()
                    {
                        new CommandKeyValue() {Command = "g", Url = "https://www.google.by/search?q={0}"},
                        new CommandKeyValue() {Command = "y", Url = "http://yandex.ru/yandsearch?text={0}"},
                        new CommandKeyValue() {Command = "кино", Url = "http://www.kinopoisk.ru/index.php?first=yes&what=&kp_query={0}"},
                        new CommandKeyValue() {Command = "you", Url = "https://www.youtube.com/results?search_query={0}"},
                        new CommandKeyValue() {Command = "so", Url = "http://stackoverflow.com/search?q={0}"}
                    }
                };
                _client.SaveSettings(existSettings);
            }
            _commandUrls = existSettings.Commands;
            _callCommandList = new ReadOnlyCollection<CallCommandInfo>(existSettings.Commands.Select(x=> new CallCommandInfo(x.Command)).ToList());
        }

        public override void HandleMessage(string command, string args, Guid commandToken)
        {
            var foundCommand = _commandUrls.SingleOrDefault(x => x.Command == command);
            if (foundCommand!=null)
            {
                var url = foundCommand.Url;
                if (string.IsNullOrEmpty(args.Trim()) && foundCommand.Url.Contains("{0}"))
                {
                    Uri uri;
                    if (Uri.TryCreate(url, UriKind.Absolute, out uri))
                    {
                        url = uri.Scheme + "://" + uri.Host;
                    }
                }
                _client.ShowMessage(commandToken, string.Format(url, args), answerType: AnswerBehaviourType.OpenLink);
            }
        }
    }

    [ModuleSettingsFor(typeof(BrowserUrlsOpen))]
    public class BrowserUrlsOpenSettings
    {
        [SettingsNameField("Команды")]
        public List<CommandKeyValue> Commands { get;set; }

        public BrowserUrlsOpenSettings()
        {
            Commands = new List<CommandKeyValue>();
        }
    }

    public class CommandKeyValue
    {
        [SettingsNameField("Команда")]
        public string Command { get; set; }
        [SettingsNameField("URL")]
        public string Url { get; set; }
    }
}
