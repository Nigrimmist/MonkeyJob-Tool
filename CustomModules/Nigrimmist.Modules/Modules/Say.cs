using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using HelloBotCommunication;

namespace Nigrimmist.Modules.Modules
{
    public class Say : ModuleBase
    {
        private Random _r = new Random();
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
                    new CallCommandInfo("скажи",new List<string>(){"выведи"})
                });
            }
        }


        public override string CommandDescription { get { return @"Говорит что прикажете"; } }
        public override void HandleMessage(string command, string args, Guid commandToken)
        {
            _bot.ShowMessage(commandToken,args);
        }
    }
}
