using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using HelloBotCommunication;
using HelloBotCommunication.Interfaces;


namespace SmartAssHandlerLib
{
    public class SmartAssStuffHandlerHandlerBase : ModuleHandlerBase
    {
        private const string Query =
            "http://referats.yandex.ru/referats/write/?t=astronomy+geology+gyroscope+literature+marketing+mathematics+music+polit+agrobiologia+law+psychology+geography+physics+philosophy+chemistry+estetica";

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
                    new CallCommandInfo("огненный текст"),
                });
            }
        }

        public override string ModuleDescription
        {
            get { return "Безумная заумь небольшими дозами. Добавьте слово \"напалмом\" к команде, чтобы получить порцию зауми побольше. "; }
        }

        public override void HandleMessage(string command, string args, Guid commandToken)
        {
            var needLotsOfStuff = !string.IsNullOrEmpty(args) && args.Contains("напалмом");
            _client.ShowMessage(commandToken,RetrieveSmartAssStuff(needLotsOfStuff));
        }

        private string RetrieveSmartAssStuff(bool needLotsOfStuff)
        {
            var result = "ААААРРРГХ!!! Что-то в моей голове! В МОЕЙ ГОЛОВЕ!!!";

            try
            {
                var referat = GetReferatText();

                result = GetReferatPart(referat, needLotsOfStuff);
            }
            catch (Exception ex) { }

            return result;
        }

        private string GetReferatText()
        {
            var client = new WebClient();
            var rawResult = client.DownloadData(Query);
            var chars = new char[rawResult.Length];

            Encoding.UTF8.GetDecoder().GetChars(rawResult, 0, rawResult.Length, chars, 0);

            return new string(chars);
        }

        private string GetReferatPart(string referatText, bool isLong)
        {
            var parts = referatText.Split(new[] { "<p>" }, StringSplitOptions.RemoveEmptyEntries);
            var entry = parts[1].Replace("</p>", string.Empty);

            return isLong ? entry : entry.Split('.').First();
        }
    }
}
