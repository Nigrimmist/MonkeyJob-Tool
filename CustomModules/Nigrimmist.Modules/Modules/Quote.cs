using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using HelloBotCommunication;
using HelloBotCommunication.Interfaces;
using HelloBotModuleHelper;

namespace Nigrimmist.Modules.Modules
{
    /// <summary>
    /// Quote from http://online-generators.ru/
    /// </summary>
    public class Quote : ModuleCommandBase
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
            get { return "Цитатник"; }
        }

        public override ReadOnlyCollection<CallCommandInfo> CallCommandList
        {
            get
            {
                return new ReadOnlyCollection<CallCommandInfo>(new List<CallCommandInfo>()
                {
                    new CallCommandInfo("цитата", new List<string>(){"quote"})
                });
            }
        }

        public override DescriptionInfo ModuleDescription
        {
            get
            {
                return new DescriptionInfo()
                {
                    Description = "Случайная цитата c http://online-generators.ru",
                };
            }
        }

       

        public override void HandleMessage(string command, string args, Guid commandToken)
        {
            HtmlReaderManager hrm = new HtmlReaderManager();
            
            hrm.Post("http://online-generators.ru/ajax.php", "processor=quotes");
            var answerParts = hrm.Html.Split(new string[]{"##"},StringSplitOptions.RemoveEmptyEntries);
            string quote = answerParts[0];
            string author = answerParts[1];
            _client.ShowMessage(commandToken, CommunicationMessage.FromString(string.Format("{0} ©{1}", quote, author)));
        }
    }
}
