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
    public class ShortLink : ModuleHandlerBase
    {
        private IClient _client;

        public override void Init(IClient client)
        {
            _client = client;
        }

        public override double ModuleVersion
        {
            get { return 1.0; }
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
                    Description = "Сокращалка ссылок"
                };
            }
        }
        

        public override void HandleMessage(string command, string args, Guid commandToken)
        {
            string answer = args.ToShortUrl();
            _client.ShowMessage(commandToken,answer, answerType:AnswerBehaviourType.CopyToClipBoard);
        }
    }
}
