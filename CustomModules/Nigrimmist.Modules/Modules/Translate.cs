using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Web;
using HelloBotCommunication;
using HelloBotCommunication.Interfaces;
using HelloBotModuleHelper;

namespace Nigrimmist.Modules.Modules
{
    public class Translate : ModuleCommandBase
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

        public override ReadOnlyCollection<CallCommandInfo> CallCommandList
        {
            get
            {
                return new ReadOnlyCollection<CallCommandInfo>(new List<CallCommandInfo>()
                {
                    new CallCommandInfo("translate",new List<string>(){"перевод","переведи"}),
                    
                });
            }
        }

        public override DescriptionInfo ModuleDescription
        {
            get
            {
                return new DescriptionInfo()
                {
                    Description = "Переводчик. Язык определяет автоматически, на данный момент поддерживаются только русский и английский",
                    SamplesOfUsing = new List<string>()
                    {
                        "перевод house",
                        "переведи дом"
                    },
                    CommandScheme = "переведи <фраза или слово>"
                };
            }
        }

        public override string Title
        {
            get { return "Переводчик"; }
        }

        public override void HandleMessage(string command, string args, Guid commandToken)
        {
            
            HtmlReaderManager hrm = new HtmlReaderManager();

            Regex r = new Regex("[а-яА-ЯЁё]+");
            bool isRu = r.IsMatch(args);
            string fromLang = isRu ? "ru" : "en";
            string toLang = isRu ? "en" : "ru";
            //https://translate.google.com/translate_a/single?client=t&sl=en&tl=ru&hl=ru&dt=at&dt=bd&dt=ex&dt=ld&dt=md&dt=qca&dt=rw&dt=rm&dt=ss&dt=t&ie=UTF-8&oe=UTF-8&otf=2&ssel=0&tsel=0&kc=5&tk=121871.516795&q=привет
            hrm.Get("https://translate.google.com/#en/ru/%D0%BF%D1%80%D0%B8%D0%B2%D0%B5%D1%82");
            hrm.Get(string.Format("https://translate.google.com/translate_a/single?client=t&sl={0}&tl={1}&hl=ru&dt=at&dt=bd&dt=ex&dt=ld&dt=md&dt=qca&dt=rw&dt=rm&dt=ss&dt=t&ie=UTF-8&oe=UTF-8&otf=2&ssel=0&tsel=0&kc=5&tk=121871.516795&q=ет", fromLang, toLang) + HttpUtility.UrlEncode(args));
            string html = hrm.Html;
            string anwser = html.Substring(4, html.IndexOf(@""",""") - 4);
            _client.ShowMessage(commandToken, CommunicationMessage.FromString(anwser));
        }

       
    }
}
