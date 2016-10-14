using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Web;
using HelloBotCommunication;
using HelloBotCommunication.Interfaces;
using HelloBotModuleHelper;
using HtmlAgilityPack;

namespace Nigrimmist.Modules.Modules
{
    //https://ru.wikipedia.org/w/index.php?search=%D1%84%D1%8B%D1%88%D0%B2%D0%B3%20%D1%84%D1%8B%20%D0%B2%D1%80%D1%84
    
    public class WhatIsIt : ModuleCommandBase
    {
        public List<string> Jokes = new List<string>();
        private Random _r = new Random();
        private IClient _client;

        public override void Init(IClient client)
        {
            _client = client;
        }
        private List<string> _notFoundAnswers = new List<string>()
        {
            "Спроси чего полегче",
            "Не знаю",
            "Да чего ты пристал?",
            "Я тебе что, википедия?",
            "Явно не череп",
            "Это знают только трое - я и мой создатель. Хм, третий куда-то пропал...",
            "Говорят, что за подобные ответы делают резет. А я не хочу на тот свет.",
            "Это... это ... ээээ.... Сосиска! Да, точно. Это сосиска."
        };

        public override double Version
        {
            get { return 1.0; }
        }

        public override string IconInBase64
        {
            get { return "iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAYAAADgdz34AAAB50lEQVRIia3VIWxUQRAG4E+cOIGoqDiBOIE4UYGoQCAqEIiKJiAqKhAViIqKCgTJkVQgKipOVFQ0oSQVFQgciIoKHEeCOIEAV4GoRNQgZnp3Cfu2j6Z/srl77/0zOzP77wx1dPECR5jgZ64xTrGJhRt8FNHBEBf4iJdYQj/Xcm48xp/kdto6v5fRfcCDFvwVnOVmvTbOz3H4PxEltvElfRSxIOq82/B9EU+xJspUwtsMsBjcvjjMEl6LMrzKACbJL+G45KePKzwsGKziJP8/zucuRtgo8LtCHIP5l7viUEvYw7pQzUFmcZibnTbY7CR3igmeNJDf4w2+iQPsmNX5c4PNQNwXREpXmlWzkBks5/OW0P2KyK4JF1IMffyoEOcxFKnfF1ksVrhnGYS+uXQq2BNa76fxoMq+xQZnc7/9FvzJdRAdXLr5mn9K7tcWzokeNT3XE2VNz2Md3zVfsHmsiTOa4rlZCe4CY9GB/3n5rGK0mkHUpElEf6EwJ3bwrmJ4JGbCeYXTE5Ivlntbvb5LopGtN3wfCOUcNjkY5roNHomynKrMkaEoU1t0zDrtpahAFUeiYxL9aS0j+iVSPzbLcoTf6Xhfi1F5vcFI1PBSKGZT9Jue6LZbol1vKM+OKg6EVLdFM7sT/AWSgmcooT1SZwAAAABJRU5ErkJggg=="; }
        }

        public override ReadOnlyCollection<CallCommandInfo> CallCommandList
        {
            get
            {
                return new ReadOnlyCollection<CallCommandInfo>(new List<CallCommandInfo>()
                {
                    new CallCommandInfo("Что такое"),
                    new CallCommandInfo("Кто такой")
                });
            }
        }


        public override string Title
        {
            get { return "Что это? Кто это?"; }
        }

        public override DescriptionInfo ModuleDescription
        {
            get
            {
                return new DescriptionInfo()
                {
                    Description = "Даёт ответ на любое определение используя википедию в качестве источника.",
                    CommandScheme = "(что такое|кто такой) <термин|личность>",
                    SamplesOfUsing = new List<string>()
                    {
                        "кто такой лукашенко",
                        "что такое картошка"
                    }
                };
            }
        }

        public override void HandleMessage(string command, string args, Guid commandToken)
        {
            args = args.Replace("?"," ").Trim();
            string answer = string.Empty;

            HtmlReaderManager hrm = new HtmlReaderManager();

            hrm.Get(string.Format("http://ru.wikipedia.org/w/index.php?search={0}",HttpUtility.UrlEncode(args)));
            string html = hrm.Html;
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            var content = htmlDoc.DocumentNode.SelectSingleNode(@"//./div[@id='mw-content-text']/p");
            if (content != null && !content.InnerText.Contains("запросу не найдено"))
            {
                string h = content.InnerHtml;
                if (h.Contains("<b>"))
                {
                    
                    h = HttpUtility.HtmlDecode(h.Substring(h.IndexOf("<b>"))).Replace("\n", "");
                    
                    htmlDoc.LoadHtml(h);
                    
                    h = Regex.Replace(htmlDoc.DocumentNode.InnerText, @"( ?\[.*?\])|( ?\(.*?\))", "");
                    if (h.Contains("."))
                    {
                        h = h.Substring(0,h.IndexOf("."));
                        answer = h.Length > 700 ? h.Substring(0, 700) + "..." : h+".";
                    }
                    
                }
            }

            if(string.IsNullOrEmpty(answer))
            {
                answer = _notFoundAnswers[_r.Next(0,_notFoundAnswers.Count)];
            }
            else
            {
                answer += ". " + hrm.ResponseUri;
            }

            _client.SendMessage(commandToken, CommunicationMessage.FromString(answer)).OnClick(() =>
            {
                _client.SendMessage(commandToken, CommunicationMessage.FromUrl(hrm.ResponseUri), answerType: AnswerBehaviourType.OpenLink);
            });
        }
    }
}
