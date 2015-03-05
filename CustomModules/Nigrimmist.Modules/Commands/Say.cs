using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using HelloBotCommunication;

namespace Nigrimmist.Modules.Commands
{
    public class Say : IActionHandler
    {
        private Random r = new Random();
        

        
        public ReadOnlyCollection<CallCommandInfo> CallCommandList
        {
            get
            {
                return new ReadOnlyCollection<CallCommandInfo>(new List<CallCommandInfo>()
                {
                    new CallCommandInfo("скажи",new List<string>(){"выведи"}),
                    
                });
            }
        }


        public string CommandDescription { get { return @"Говорит что прикажете"; } }
        public void HandleMessage(string command, string args, object clientData, Action<string, AnswerBehaviourType> sendMessageFunc)
        {
            sendMessageFunc(args,AnswerBehaviourType.ShowText);
        }
    }
}
