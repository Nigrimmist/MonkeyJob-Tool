using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using HelloBotCommunication;
using HelloBotCommunication.Interfaces;
using HelloBotModuleHelper;
using HtmlAgilityPack;

namespace Nigrimmist.Modules.Modules
{
    public class Boobs : ModuleBase
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
                    new CallCommandInfo("сиськи",new List<string>(){"boobs"}),
                });
            }
        }

        public override string CommandDescription { get { return "Ну а что тут объяснять. Сиськи."; } }
        private Random _r = new Random();
        private List<string> _images = new List<string>();

        public override void HandleMessage(string command, string args, Guid commandToken)
        {
            if (!_images.Any())
            {
                HtmlReaderManager hrm = new HtmlReaderManager();
                hrm.Get("http://boobs-selfshots.tumblr.com/page/" + _r.Next(1, 600));
                string html = hrm.Html;
                HtmlDocument htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);
                var divs = htmlDoc.DocumentNode.SelectNodes(@"//./div[@class='photo_post']/a/img");
                StringBuilder sb = new StringBuilder();
                foreach (var div in divs)
                {
                    _images.Add(div.Attributes["src"].Value);
                }
            }
            int rPos = _r.Next(0, _images.Count);
            string url = _images[rPos];
            _images.RemoveAt(rPos);
            _client.ShowMessage(commandToken,url,answerType: AnswerBehaviourType.OpenLink);
        }


    }
}
