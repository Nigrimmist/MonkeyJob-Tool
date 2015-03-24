using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml.Linq;
using System.Xml.XPath;
using HelloBotCommunication;
using HelloBotCommunication.Interfaces;


namespace SmartAssHandlerLib
{
    public class FckinWeatherModuleHandlerBase : ModuleHandlerBase
    {
        private const string DefaultLocation = "minsk";
        private const string QueryTemaplate = "http://thefuckingweather.com/?where={0}";
        private const string FailedResult = "I CAN'T FIND THAT SHIT!";
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
                    new CallCommandInfo("fun weather", new List<string>(){"погода"})
                });
            }
        }

        public override string ModuleDescription { get { return "Shows fucking WEATHER!"; } }

        public override void HandleMessage(string command, string args, Guid commandToken)
        {
            var result = FailedResult;

            try
            {
                var location = args.Split(' ').FirstOrDefault();
                var rawData = GetRawWeatherData(location);
                var forecast = ParseForecast(rawData);

                result = forecast.ToString();
            }
            catch (Exception ex) { }

            _client.ShowMessage(commandToken,result);
        }

        private Forecast ParseForecast(string rawWeatherHtml)
        {
            var encodedStr = HttpUtility.HtmlDecode(rawWeatherHtml);

            var form = encodedStr.Split(new[] { "<div class=\"content\">" }, StringSplitOptions.RemoveEmptyEntries).Last();

            form = form.Split(new[] { "<table" }, StringSplitOptions.RemoveEmptyEntries).First();
            form = Uri.UnescapeDataString(string.Format("<div class=\"content\">{0}</div>", form));
            
            var xml = XElement.Parse(form);

            var tempStr = (string) xml.XPathEvaluate("string(/p/span/@tempf)");
            var remark = (string) xml.XPathEvaluate("string(/div/p[text()])");
            var flavor = (string) xml.XPathEvaluate("string(/p[2][text()])");

            var celsiusTemp = ConvertToCelsius(double.Parse(tempStr));

            return new Forecast()
            {
                DegreeCels = celsiusTemp,
                Remark = remark,
                Flavor = flavor
            };
        }

        private double ConvertToCelsius(double fahrenheit)
        {
            return Math.Round((fahrenheit - 32.0)*5.0/9.0, 1);
        }

        private string GetRawWeatherData(string location)
        {
            var client = new WebClient();

            client.Headers.Add("User-Agent", "Fiddler");

            var rawResult = client.DownloadData(string.Format(QueryTemaplate, location ?? DefaultLocation));
            var chars = new char[rawResult.Length];

            Encoding.UTF8.GetDecoder().GetChars(rawResult, 0, rawResult.Length, chars, 0);

            return new string(chars);
        }

        private class Forecast
        {
            public double DegreeCels { get; set; }
            public string Remark { get; set; }
            public string Flavor { get; set; }

            public override string ToString()
            {
                return string.Format("{0}?!{1}{2}{3}{4}",
                    DegreeCels,
                    Environment.NewLine,
                    Remark,
                    Environment.NewLine,
                    Flavor);
            }
        }
    }
}
