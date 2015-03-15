using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Web;
using HelloBotCommunication;
using HelloBotModuleHelper;
using HtmlAgilityPack;

namespace Nigrimmist.Modules.Modules
{
    public class Bash : ModuleBase
    {
        private List<string> _jokes = new List<string>();
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
                    new CallCommandInfo("bash.org",new List<string>(){"bash","башорг"})
                });
            }
        }

        

        public override string CommandDescription { get { return @"Случайная цитата с башорга"; } }
        public override void HandleMessage(string command, string args, Guid commandToken)
        {
            if (!_jokes.Any())
            {
                HtmlReaderManager hrm = new HtmlReaderManager();
                hrm.Encoding = Encoding.GetEncoding(1251);
                hrm.Get("http://bash.im/random");
                string html = hrm.Html;
                HtmlDocument htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);
                var divs = htmlDoc.DocumentNode.SelectNodes(@"//./div[@class='text']");

                foreach (var div in divs)
                {
                    _jokes.Add(HttpUtility.HtmlDecode(div.InnerHtml.Replace("<br>", Environment.NewLine)));
                }
            }
            int rPos = _r.Next(0, _jokes.Count );
            string joke = _jokes[rPos];
            _jokes.RemoveAt(rPos);

            _bot.ShowMessage(commandToken,joke); ;
        }
    }
}
