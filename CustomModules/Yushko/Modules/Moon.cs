using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using HelloBotCommunication;
using HelloBotCommunication.Interfaces;
using HelloBotModuleHelper;
using HtmlAgilityPack;

namespace Yushko.Modules
{
    public class Moon : ModuleCommandBase
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
                    new CallCommandInfo("луна", new List<string>(){"moon"} ),
                });
            }
        }

        public override string IconInBase64
        {
            get { return "iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAYAAADgdz34AAABMklEQVRIibXWIUsDYRyA8Z+wsDCwyUDjugaRhQWbIgY/woIfQFBkwSrGBYNxQcEwwWAwyYJBm4hxYUFFxCoYX8PtYIzb7tydD/zLHe/z3MG9L0c6K2jjHgPUM6xJpYQmHhBGpotyXnkF12PigFZeMazhKkHeLkJeQT9B3h/ey81xgjxgvwj5Bt4T5J9YLiJwkyAPok8zNwt4mxC4KyJQnyAP6BURaE0JPGIub6AzJRBQ++/AXt7AWUrgC0t5ArspgYALzM8aqGYIBFzKfmSUcIKD+EIvY2SARop8VbRBg+jIB+sZA/G84hTbw7WbOBoRx4dkabR8+8dI2jSNUcVHQfLu+NPHNAqITJTH1PAyo7yTJo9ZxCGeM0i/h+KtLOJxytjB+fCtvvCDJ9G+aIl+aybyCyzO4UDqYWU/AAAAAElFTkSuQmCC"; }
        }

        public override string Title
        {
            get { return "Лунный календарь"; }
        }

        public override DescriptionInfo ModuleDescription
        {
            get
            {
                return new DescriptionInfo()
                {
                    Description = "Лунный календарь"
                };
            }
        }

        public override void HandleMessage(string command, string args, Guid commandToken)
        {
            StringBuilder result = new StringBuilder();
            HtmlReaderManager hrm = new HtmlReaderManager();
            hrm.Encoding = Encoding.GetEncoding(1251);
            hrm.Get(@"http://www.goroskop.org/luna/049/segodnya.shtml");
            string html = hrm.Html;
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            //title
            result.Append(htmlDoc.DocumentNode.SelectSingleNode(@"//./span[@class='tabl-header']").InnerText.Trim());
            result.Append(Environment.NewLine);
            //info
            HtmlNodeCollection tblSpans = htmlDoc.DocumentNode.SelectNodes(@"//./span[@class='tabl-content']");
            if ((tblSpans != null) && (tblSpans.Count >= 1)) { 
                HtmlNodeCollection tds = tblSpans[1].SelectNodes(@".//./table[1]/tr/td");
                if ((tds != null) && (tds.Count >= 2)) {
                    result.Append(tds[1].InnerText);//.InnerHtml.Replace("<br>", Environment.NewLine).RemoveAllTags().Trim();
                }
            }
            _client.SendMessage(commandToken, CommunicationMessage.FromString(result.ToString()));
        }
    }
}
