using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using HelloBotCommunication;
using HelloBotCommunication.Interfaces;
using HelloBotModuleHelper;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace Nigrimmist.Modules.Modules
{
    public class Translate : ModuleCommandBase
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
                    new CallCommandInfo("translate",new List<string>(){"перевод","переведи"}),
                    
                });
            }
        }

        public override DescriptionInfo ModuleDescription
        {
            get
            {
                return new DescriptionInfo()
                {
                    Description = "Переводчик. Язык определяет автоматически, на данный момент поддерживаются только русский и английский",
                    SamplesOfUsing = new List<string>()
                    {
                        "перевод house",
                        "переведи дом"
                    },
                    CommandScheme = "переведи <фраза или слово>"
                };
            }
        }

        public override string Title
        {
            get { return "Переводчик"; }
        }

        public override void HandleMessage(string command, string args, Guid commandToken)
        {
            
            HtmlReaderManager hrm = new HtmlReaderManager();

            StringBuilder sb = new StringBuilder();

            hrm.ContentType = "application/json; charset=utf-8";
            hrm.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:48.0) Gecko/20100101 Firefox/48.0";
            hrm.Post("http://www.translate.ru/services/TranslationService.asmx/GetTranslateNew",
            string.Format("{{dirCode:'ru-en', template:'General', text:'{0}', lang:'ru', limit:3000,useAutoDetect:true, key:'', ts:'MainSite',tid:'',IsMobile:false}}", HttpUtility.UrlEncode(args)));

            var jsonType = new { d = new { result = "", isWord = false } };
            var data = JsonConvert.DeserializeAnonymousType(hrm.Html, jsonType);
            if (data.d.isWord)
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(data.d.result);
                var variantsNodes = doc.DocumentNode.SelectNodes("//div[@class='cforms_result']");
                foreach (var node in variantsNodes)
                {
                    var title = node.SelectSingleNode(".//span[@class='source_only']").InnerText;
                    var type = node.SelectSingleNode(".//span[@class='ref_psp']").InnerText;
                    sb.AppendLine(string.Format("{0} ({1}) :", title, type));
                    var translationNodes = node.SelectNodes(".//div[@id='translations']/.//span[@class='ref_result']");
                    foreach (var translatioNode in translationNodes)
                    {
                        var transl = translatioNode.InnerText;
                        sb.AppendLine(transl.TrimStart());
                    }
                    sb.AppendLine();
                }
            }
            else
            {
                sb.AppendLine(data.d.result);
            }

            _client.SendMessage(commandToken, CommunicationMessage.FromString(sb.ToString()));
        }

       
    }
}
