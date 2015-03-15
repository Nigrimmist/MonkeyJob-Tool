using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using HelloBotCommunication;
using HelloBotModuleHelper;

namespace Nigrimmist.Modules.Modules
{
    /// <summary>
    /// Generate short link for argument url
    /// </summary>
    public class ShortLink : ModuleBase
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
       
        public override string CommandDescription { get { return @"Сокращалка ссылок"; } }

        public override void HandleMessage(string command, string args, Guid commandToken)
        {
            string answer = args.ToShortUrl();
            _bot.ShowMessage(commandToken,answer);
        }
    }
}
