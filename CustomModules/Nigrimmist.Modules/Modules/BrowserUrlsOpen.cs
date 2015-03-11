using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using HelloBotCommunication;

namespace Nigrimmist.Modules.Modules
{
    public class BrowserUrlsOpen: ModuleBase
    {

        private IBot _bot;

        public override void Init(IBot bot)
        {
            _bot = bot;
        }

        public override double ModuleVersion
        {
            get { return 1.0; }
        }

        private readonly ReadOnlyCollection<CallCommandInfo> _callCommandList;
        public override ReadOnlyCollection<CallCommandInfo> CallCommandList
        {
            get { return _callCommandList; }
        }

        public override string CommandDescription { get { return "Открывает ссылку в браузере"; } }
        private readonly IDictionary<string, string> _commandUrlDictionary = new Dictionary<string, string>();
        

        public BrowserUrlsOpen()
        {
           var configurationData = File.ReadAllText("ModuleConfiguration/BrowserUrlsOpenModule.txt");
            if (!string.IsNullOrEmpty(configurationData))
            {
                var keyValues = configurationData.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries).Select(line => line.Split(';')).Select(x => new KeyValuePair<string, string>(x[0], x[1]));

                foreach (KeyValuePair<string, string> keyValue in keyValues)
                {
                    _commandUrlDictionary.Add(keyValue);
                }

                _callCommandList = new ReadOnlyCollection<CallCommandInfo>(_commandUrlDictionary.Select(x => new CallCommandInfo(x.Key)).ToList());
            }
        }

        public override void HandleMessage(string command, string args)
        {
            
               // string subCommand = args.Split(' ').First();
                //args = args.Substring(subCommand.Length).TrimStart();
                if (_commandUrlDictionary.ContainsKey(command))
                {
                    var url = _commandUrlDictionary[command];
                    if (string.IsNullOrEmpty(args.Trim()))
                    {
                        Uri uri;
                        if (Uri.TryCreate(url, UriKind.Absolute, out uri))
                        {
                            url = uri.Scheme + "://" + uri.Host;
                        }
                    }
                    _bot.ShowMessage(string.Format(url, args),answerType:AnswerBehaviourType.OpenLink);
                }
            
        }
    }
}
