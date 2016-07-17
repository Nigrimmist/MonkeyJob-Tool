using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using HelloBotCommunication;
using HelloBotCommunication.Interfaces;
using HelloBotModuleHelper;
using HtmlAgilityPack;
using Nigrimmist.Modules.Helpers;

namespace Nigrimmist.Modules.Modules
{
    public class ItHappens : ModuleCommandBase
    {
        public List<string> _jokes = new List<string>();
        private Random _r = new Random();
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
                    new CallCommandInfo("ithappens.ru", new List<string>(){"ithap","it"} )
                });
            }
        }

        
        public override DescriptionInfo ModuleDescription
        {
            get
            {
                return new DescriptionInfo()
                {
                    Description = "Случайная IT история с ithappens.me"
                };
            }
        }

        public override string Title
        {
            get { return "История ItHappens"; }
        }

        public override void HandleMessage(string command, string args, Guid commandToken)
        {
            if (!_jokes.Any())
            {
                HtmlReaderManager hrm = new HtmlReaderManager();
                
                hrm.Get("http://ithappens.me/random");
                string html = hrm.Html;
                HtmlDocument htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);
                var divs = htmlDoc.DocumentNode.SelectNodes(@"//./div[@class='text']");

                foreach (var div in divs)
                {
                    _jokes.Add(div.InnerHtml.Replace("<p>", "").Replace("</p>", Environment.NewLine + Environment.NewLine).RemoveAllTags().Trim());
                }
            }
            int rPos = _r.Next(0, _jokes.Count );
            string joke = _jokes[rPos];
            _jokes.RemoveAt(rPos);


            joke = Regex.Replace(joke, "[\r\n]{3,}", "\r\n\r\n");


            _client.ShowMessage(commandToken, CommunicationMessage.FromString(joke));
        }
    }
}
