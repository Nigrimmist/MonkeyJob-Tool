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
    public class Kstati : ModuleCommandBase
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
                    new CallCommandInfo("факт", new List<string>(){"кстати"} )
                });
            }
        }

        public override string Title
        {
            get { return "Интересный факт"; }
        }

        public override DescriptionInfo ModuleDescription
        {
            get
            {
                return new DescriptionInfo()
                {
                    Description = "Интересный факт одной строкой с сайта know-that.ru"
                };
            }
        }

        public override void HandleMessage(string command, string args, Guid commandToken)
        {
            string url = "http://know-that.ru/randomizer.php";
            string result = string.Empty;

            HtmlReaderManager hrm = new HtmlReaderManager();
            hrm.Encoding = Encoding.GetEncoding(1251);
            hrm.Get(url);
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(hrm.Html);
            HtmlNodeCollection factList = htmlDoc.DocumentNode.SelectNodes(@"//./center/table");
            if (factList != null)
            {
                Random random = new Random();
                int showResult = random.Next(factList.Count-1);
                HtmlNodeCollection tds = factList[showResult].SelectNodes(".//./td");//.InnerText.Trim();
                if ((tds != null) && (tds.Count >= 3))
                {
                    result = tds[2].InnerText.Trim();
                }
                else {
                    result = "Факт сломался... :(";
                }
            }
            else {
                result = "Факты кончились... :(";
            }
            _client.ShowMessage(commandToken, CommunicationMessage.FromString(result));
        }
    }
}
