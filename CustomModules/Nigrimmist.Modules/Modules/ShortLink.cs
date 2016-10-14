using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using HelloBotCommunication;
using HelloBotCommunication.Interfaces;
using HelloBotModuleHelper;

namespace Nigrimmist.Modules.Modules
{
    /// <summary>
    /// Generate short link for argument url
    /// </summary>
    public class ShortLink : ModuleCommandBase
    {
        private IClient _client;

        public override void Init(IClient client)
        {
            _client = client;
        }

        public override double Version
        {
            get { return 1.0; }
        }

        public override string Title
        {
            get { return "Сокращатель ссылок"; }
        }

        public override ReadOnlyCollection<CallCommandInfo> CallCommandList
        {
            get
            {
                return new ReadOnlyCollection<CallCommandInfo>(new List<CallCommandInfo>()
                {
                    new CallCommandInfo("сократи",new List<string>(){"short"}),
                });
            }
        }
        public override DescriptionInfo ModuleDescription
        {
            get
            {
                return new DescriptionInfo()
                {
                    Description = "Преобразует длинные ссылки в короткие",
                    CommandScheme = "сократи <url>",
                    SamplesOfUsing = new List<string>()
                    {
                        "сократи http://longlonglongurl.ru/longlonglonglonglonglonglong.html"
                    }
                };
            }
        }

        public override void HandleMessage(string command, string args, Guid commandToken)
        {
            string answer = args.ToShortUrl();
            _client.SendMessage(commandToken, CommunicationMessage.FromString(answer), answerType: AnswerBehaviourType.CopyToClipBoard);
        }
    }
}
