﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Web;
using HelloBotCommunication;
using HelloBotCommunication.Interfaces;
using HelloBotModuleHelper;
using Newtonsoft.Json;
using Nigrimmist.Modules.Helpers;

namespace Nigrimmist.Modules.Modules
{
    /// <summary>
    /// Fun advices from http://fucking-great-advice.ru/
    /// </summary>
    public class Advice : ModuleHandlerBase
    {
        private class AdviceResponse
        {
            public string text { get; set; }
        }

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
                    new CallCommandInfo("дай совет",new List<string>(){"advice", "совет"})
                });
            }
        }

        public override DescriptionInfo ModuleDescription
        {
            get
            {
                return new DescriptionInfo()
                {
                    Description = @"Случайный совет с http://fucking-great-advice.ru/"
                };
            }
        }

        public override string ModuleTitle
        {
            get { return "Лучший совет"; }
        }

        public override void HandleMessage(string command, string args, Guid commandToken)
        {
            HtmlReaderManager hrm = new HtmlReaderManager();
            hrm.Encoding = Encoding.GetEncoding(1251);
            hrm.Get("http://fucking-great-advice.ru/api/random");
            var json = JsonConvert.DeserializeObject<AdviceResponse>(hrm.Html);
            string advice = json.text;
            _client.ShowMessage(commandToken,HttpUtility.HtmlDecode(advice.RemoveAllTags()));
        }
    }
}
