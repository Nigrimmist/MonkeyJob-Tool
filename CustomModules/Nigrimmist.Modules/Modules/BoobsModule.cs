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
    public class Boobs : ModuleCommandBase
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
                    new CallCommandInfo("сиськи",new List<string>(){"boobs"}),
                });
            }
        }

        public override DescriptionInfo ModuleDescription
        {
            get
            {
                return new DescriptionInfo()
                {
                    Description = "Ну а что тут объяснять. Сиськи. Показывает фотку рандомной женской груди."
                };
            }
        }

        public override string Title
        {
            get { return "Сиськи"; }
        }

        public override string IconInBase64
        {
            get { return "iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAYAAADgdz34AAABHklEQVRIie3ULVNCURDG8V8wEG4wEAgEg8FoMBgNBIPBaCAYDESC0RmiwUA0GPwABIPRYDAQDQYigWjkCxjO3pl7z8AMDBgc+c+csrtnX57zwo5/SxFrq/F7GOAL3+iuUaCLWewdRK4aB/jECO01Eue08Ry5DquOd9xukDinh7GY5AZvW0xeMkIfXnD5CwU6ovGZ1XVvxVqFAvN1Czzgad0CyyTaX2D7kG7Iotj8anYiXg+vmbMvvYUqx5jGOs18U+n+VxnhTlQeR6GSVnRQUkTn17jCRH3CM3WZuxHTKA1HYXhEM+vkPDocVmx96exyaYuIm0oT12jgPjZOJP3m0fmiM7oI31xSoPxihlb4l5qSzvk0y2JPbPbF7PjL/ADFWTbx04MzRAAAAABJRU5ErkJggg=="; }
        }

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

            _client.SendMessage(commandToken, CommunicationMessage.FromUrl(url), answerType: AnswerBehaviourType.OpenLink);
        }


    }
}
