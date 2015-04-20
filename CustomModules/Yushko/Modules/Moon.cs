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
    public class Moon : ModuleHandlerBase
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
                    new CallCommandInfo("луна", new List<string>(){"moon"} ),
                });
            }
        }
        

        public override DescriptionInfo ModuleDescription
        {
            get
            {
                return new DescriptionInfo()
                {
                    Description = "лунный календарь"
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
            _client.ShowMessage(commandToken,result.ToString());
        }
    }
}
