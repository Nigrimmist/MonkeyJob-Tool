using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using HelloBotCommunication;
using HelloBotCommunication.Interfaces;
using HelloBotModuleHelper;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace Nigrimmist.Modules.Modules
{
    public class SpellCheckerModule : ModuleCommandBase
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
                    new CallCommandInfo("как пишется",new List<string>(){"проверка","spellcheck"}),
                    
                });
            }
        }

        public override DescriptionInfo ModuleDescription
        {
            get
            {
                return new DescriptionInfo()
                {
                    Description = "Проверяет и исправляет написанное слово/короткую фразу",
                    SamplesOfUsing = new List<string>()
                    {
                        "проверка как дила",
                        "spellcheck денозавр",
                        "как пишется лукошенко"
                    },
                    CommandScheme = "переведи <фраза или слово>"
                };
            }
        }

        public override string Title
        {
            get { return "Правописание"; }
        }

        public override void HandleMessage(string command, string args, Guid commandToken)
        {
            HtmlReaderManager hrm = new HtmlReaderManager();
            hrm.Get("https://yandex.by/search/?lr=157&msid=1474230209.49634.22903.3782&text=" + HttpUtility.UrlEncode(args+" как пишется"));
            var jsPh = new string[] { @"var title = ", "el.innerHTML = " };
            string res = string.Empty;
            for (int i = 0; i < jsPh.Count(); i++)
            {
                var pos = hrm.Html.IndexOf(jsPh[i]);
                if (pos > 0)
                {
                    res = hrm.Html.Substring(pos + jsPh[i].Length + 1);
                    res = res.Substring(0, res.IndexOf(@"—")).Trim().Replace(" как пишется",string.Empty);
                    break;
                }
                
            }

            
            _client.ShowMessage(commandToken, CommunicationMessage.FromString(res));
        }
    }
}
