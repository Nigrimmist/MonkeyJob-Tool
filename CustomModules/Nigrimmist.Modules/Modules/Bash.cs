using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Web;
using HelloBotCommunication;
using HelloBotCommunication.Interfaces;
using HelloBotModuleHelper;
using HtmlAgilityPack;

namespace Nigrimmist.Modules.Modules
{
    public class Bash : ModuleHandlerBase
    {
        private List<string> _jokes = new List<string>();
        private Random _r = new Random();

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
                    new CallCommandInfo("bash.org",new List<string>(){"bash","башорг","баш"})
                });
            }
        }

        public override string ModuleTitle
        {
            get { return "bash.org"; }
        }

        public override string IconInBase64
        {
            get { return "iVBORw0KGgoAAAANSUhEUgAAABkAAAAaCAYAAABCfffNAAAAl0lEQVR42u2WUQoAERCGJ+UQTuAajqocwUUcRh4oNkrtbrYVw5O/5snkm2n4gbRB8JdACEkAgAvx3pdN37GkE+fcekhZPJADydJaDx3vbghjrAmoEUKYh7Qq7u1oeib3HCHEOCT715eUUr/FTEN6Oj6QA5mHxBjXQIwx+278sEFSSh+g/O5XSSlxXLiKc950Xmstzr8LQxcV/52FwFbiRAAAAABJRU5ErkJggg=="; }
        }

        public override DescriptionInfo ModuleDescription
        {
            get
            {
                return new DescriptionInfo()
                {
                    Description = "Случайная цитата с bash.org"
                };
            }
        }

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

            _client.ShowMessage(commandToken,joke); ;
        }
    }
}
