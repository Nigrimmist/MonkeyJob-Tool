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

        public override string ModuleTitle
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

            hrm.Get(string.Format("https://translate.google.com/translate_a/single?client=t&sl={0}&tl={1}&hl=ru&dt=bd&dt=ex&dt=ld&dt=md&dt=qca&dt=rw&dt=rm&dt=ss&dt=t&dt=at&ie=UTF-8&oe=UTF-8&otf=2&srcrom=1&ssel=0&tsel=0&kc=1&tk=296212|170720&q=", fromLang, toLang) + HttpUtility.UrlEncode(args));
            string html = hrm.Html;
            string anwser = html.Substring(4, html.IndexOf(@""",""") - 4);
            _client.ShowMessage(commandToken,anwser);
        }

       
    }
}
