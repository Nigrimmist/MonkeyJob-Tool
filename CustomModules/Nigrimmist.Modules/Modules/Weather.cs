﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using HelloBotCommunication;
using HelloBotCommunication.Interfaces;
using HelloBotModuleHelper;
using HtmlAgilityPack;

namespace Nigrimmist.Modules.Modules
{
    public class Weather : ModuleHandlerBase
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
                    new CallCommandInfo("погода",new List<string>(){"weather"})
                });
            }
        }
        public override string ModuleDescription { get { return @"Погода с тутбая для Минска. ""!погода"" = текущая+завтра"; } }
        public override void HandleMessage(string command, string args, Guid commandToken)
        {
            HtmlReaderManager hrm = new HtmlReaderManager();
            hrm.Get("http://pogoda.tut.by/");
            string html = hrm.Html;
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            var tds = htmlDoc.DocumentNode.SelectNodes(@"//./td[@class='fcurrent-top' or @class='fcurrent-s']");
            StringBuilder sb = new StringBuilder();
            sb.Append("Погода в Минске :");
            foreach (var td in tds)
            {
                sb.Append(td.SelectSingleNode(".//./div[@class='fcurrent-h']").InnerText + " ");
                sb.Append(td.SelectSingleNode(".//./span[@class='temp-i']").InnerText + " ");
                sb.Append(td.SelectSingleNode(".//./div[@class='fcurrent-descr']").InnerText + " ");
                sb.Append(Environment.NewLine);
            }
            _client.ShowMessage(commandToken,sb.ToString().Replace("&deg;", "°"));
        }
    }
}