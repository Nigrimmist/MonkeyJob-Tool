﻿using System;
using System.Collections.Generic;
using System.Text;

using HelloBotCommunication;
using HelloBotModuleHelper;
using HtmlAgilityPack;
using Nigrimmist.Modules.Helpers;

namespace Nigrimmist.Modules.Commands
{
    public class Weather : IActionHandler
    {
        public List<CallCommandInfo> CallCommandList
        {
            get
            {
                return new List<CallCommandInfo>()
                {
                    new CallCommandInfo("погода")
                };
            }
        }
        public string CommandDescription { get { return @"Погода с тутбая для Минска. ""!погода"" = текущая+завтра"; } }
        public void HandleMessage(string command, string args, object clientData, Action<string,AnswerBehaviourType> sendMessageFunc)
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
            sendMessageFunc(sb.ToString().Replace("&deg;", "°"), AnswerBehaviourType.ShowText);
        }
    }
}
