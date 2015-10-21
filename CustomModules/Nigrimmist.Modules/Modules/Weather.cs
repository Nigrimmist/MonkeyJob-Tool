using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using HelloBotCommunication;
using HelloBotCommunication.Interfaces;
using HelloBotModuleHelper;
using HtmlAgilityPack;

namespace Nigrimmist.Modules.Modules
{
    public class Weather : ModuleCommandBase
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

        public override string ModuleTitle
        {
            get { return "Погода"; }
        }

        public override string IconInBase64
        {
            get { return @"AAABAAEAGhoAAAEAIAAgCwAAFgAAACgAAAAaAAAANAAAAAEAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD///8B////Af///wH///8B////Af///wH///8BHBwcCSsrKwYVFRUMAAAAhwAAAK4PDw8RQEBABBoaGgqAgIAC////Af///wH///8B////Af///wH///8B////Af///wH///8B////Af///wH///8B////Af///wH///8B////AREREQ8AAAB1AAAAaQoKChkAAAC1AAAA6QkJCRsAAABAAAAAgQgICCD///8B////Af///wH///8B////Af///wH///8B////Af///wH///8B////Af///wH///8B////Af///wH///8BBgYGLAAAANIAAADbERERDwAAAIgAAACvDQ0NEwAAAJ4AAADmAwMDTv///wH///8B////Af///wH///8B////Af///wH///8B////Af///wH///8B////Af///wEAAAATAAAAQAAAAA8HBwclAAAAzgAAANeAgIACAAAALAAAADYzMzMFAAAAkQAAAOEEBARIAAAABAAAAD4AAAAeVVVVA////wH///8B////Af///wH///8B////Af///wEKCgoZAAAAmgAAAPgAAAD+AAAAqFVVVQMAAAA8AAAALQ0NDRMAAACiAAAA1AsLCxcAAAAXAAAAQxUVFQwAAABuAAAA/wAAAPkAAACdCgoKGf///wH///8B////Af///wH///8BCwsLGAAAALAAAAD5AAAA2AAAALwAAABaDQ0NFAAAAJYAAACLCwsLGAAAALIAAADmCgoKGgAAAFQAAAClBgYGKwAAADUAAAC0AAAA1gAAAPoAAACgFxcXC////wH///8B////Af///wEAAAByAAAA+AAAALEAAAA5HBwcCf///wEFBQUvAAAA0wAAAOAVFRUMAAAAbgAAAI4REREPAAAApQAAAOgDAwNQ////AUBAQAQAAAA3AAAAuwAAAPcAAAA9////Af///wH///8B////AQAAAMkAAADhAAAATYCAgAL///8B////AQkJCRwAAADBAAAAu////wEREREPDg4OEoCAgAIAAAB3AAAA1QQEBDz///8B////AVVVVQMAAABvAAAA9AAAAHj///8B////Af///wH///8BAAAA9wAAAM0FBQUv////Af///wH///8BgICAAgAAABAgICAI////Af///wH///8B////ASsrKwYAAAAQVVVVA////wH///8BgICAAgAAAHoAAAD7AAAAfv///wH///8B////Af///wEAAADQAAAA5gAAAFJVVVUD////Af///wH///8B////Af///wH///8B////Af///wH///8B////Af///wH///8B////ARAQEBAEBARGAAAAzQAAAPEAAAAv////Af///wH///8B////AQAAAHUAAAD4AAAArwAAAC////8B////Af///wH///8B////Af///wH///8B////Af///wH///8B////Af///wH///8BAAAAjAAAAOYAAADmAAAAeEBAQAT///8B////Af///wH///8BCgoKGQAAAJ4AAAD0AAAAyQAAAEj///8B////Af///wH///8B////Af///wH///8B////Af///wH///8B////Af///wEAAADlAAAA+gAAAJEREREP////Af///wH///8B////Af///wGAgIACCgoKGgAAAKMAAAD8AAAAgP///wH///8B////Af///wH///8B////Af///wH///8B////Af///wH///8BAAAAEgAAAPwAAAD+AAAAwAcHByT///8B////Af///wH///8B////Af///wH///8BAAAAWAAAAOsAAAC/gICAAv///wH///8B////Af///wH///8B////Af///wH///8BMzMzBRAQEBAAAACQAAAA/gAAAPIAAAD+AAAAdFVVVQP///8B////Af///wH///8B////Af///wEAAAArAAAA2wAAAPMAAAAw////Af///wH///8B////Af///wH///8BMzMzBQMDA0oAAABtAAAArAAAAPUAAADPAAAAbgAAAO4AAACrKysrBgYGBisDAwNZAwMDWQQEBEP///8B////ARwcHAkAAACQAAAA/gAAALEAAAAlgICAAv///wH///8B////ARUVFQwAAABuAAAA7AAAAP8AAAD3AAAAuAUFBTYFBQUvAAAA1gAAAMggICAIAAAAiwAAAP4AAAD/AAAA0P///wH///8B////AQ0NDRMAAADHAAAA+AAAAMUAAABfAAAAGQAAAAsAAAA8AAAAnAAAAO8AAAD0AAAAhgAAAD0KCgoa////AQUFBTcAAADmAAAAsCQkJAcHBwcmAAAAYAAAAGUAAAA5////Af///wH///8B////AQAAABEAAACsAAAA+gAAAP4AAAD/AAAA/wAAAP8AAAD/AAAA/AAAAGb///8B////Af///wEcHBwJAAAAjwAAAP4AAAB9QEBABP///wH///8B////Af///wH///8B////Af///wH///8B////ARQUFA0AAABdAAAAqwAAAMcAAADQAAAAxAAAANYAAAD8AAAAsgAAAEQFBQUyBAQEOwAAAJgAAAD4AAAAyAYGBin///8B////Af///wH///8B////Af///wH///8B////Af///wH///8B////Af///wGAgIACCwsLGAAAAEEAAABfAAAATAAAAK4AAAD+AAAA8QAAANwAAADsAAAA/gAAAMgAAABgAAAAUwAAACL///8B////Af///wH///8B////Af///wH///8B////Af///wH///8B////Af///wEAAAASAAAAsQAAAOADAwNLCQkJHQAAAGoAAACwAAAAwwAAAKcAAAB3BgYGKAAAAEwAAADTAAAAxAAAACX///8B////Af///wH///8B////Af///wH///8B////Af///wH///8B////AQAAAJwAAAD+AAAAwwYGBij///8BVVVVAysrKwYkJCQHKysrBlVVVQP///8BCAgIHwAAAMIAAAD+AAAAvgAAAAX///8B////Af///wH///8B////Af///wH///8B////Af///wH///8BAAAAaQAAALIFBQUz////Af///wH///8BBgYGLAAAAJMAAAAv////Af///wH///8BAAAAJAAAALIAAACH////Af///wH///8B////Af///wH///8B////Af///wH///8B////Af///wH///8B////Af///wH///8B////Af///wEDAwNXAAAA/QAAAGr///8B////Af///wH///8B////Af///wH///8B////Af///wH///8B////Af///wH///8B////Af///wH///8B////Af///wH///8B////Af///wH///8B////AQMDA1gAAAD/AAAAa////wH///8B////Af///wH///8B////Af///wH///8B////Af///wH///8B////Af///wH///8B////Af///wH///8B////Af///wH///8B////Af///wH///8BBAQEOQAAAMEAAAA/////Af///wH///8B////Af///wH///8B////Af///wH///8BANcAAAAFAAADJwAACXHgAD0xgADffHgAwPNAABIZDAADhjQA4ABgAEAAfABoACAA2AD4ACQB+AAEBTQAGRzdQAQ2mwAHDBQAAH9wAAGd9AAApjQAANKOAABCDAAAAIAAAAIAAAAAAAA="; }
        }

        public override DescriptionInfo ModuleDescription
        {
            get
            {
                return new DescriptionInfo()
                {
                    Description = @"Показывает погоду в любом городе, используя данные с Яндекс.Погода.",
                    CommandScheme = "погода <город> (?:сегодня | завтра | послезавтра | пн | вт | ср | чт | пт | сб | вс | <ближайшее число в течении недели>)",
                    SamplesOfUsing = new List<string>()
                    {
                        "погода минск",
                        "погода питер завтра",
                        "погода москва пт",
                        "погода брест послезавтра",
                        "погода могилёв 14",
                    }
                };
            }
        }

        public override void HandleMessage(string command, string args, Guid commandToken)
        {
            if (string.IsNullOrEmpty(args))
                args = "минск";

            var queryInfo = new WeatherQueryInfo();
            args = queryInfo.ParseQuery(args);

            HtmlReaderManager hrm = new HtmlReaderManager();

            try
            {
                hrm.Get(string.Format("http://pogoda.yandex.by/{0}", HttpUtility.UrlEncode(args)));
            }
            catch (WebException wex)
            {
                if (((HttpWebResponse) wex.Response).StatusCode == HttpStatusCode.NotFound)
                {
                    _client.ShowMessage(commandToken, string.Format(@"Погоды для ""{0}"" нет, проверьте правильность написания города.", args));
                    return;
                }
            }

            string html = hrm.Html;
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            StringBuilder sb = new StringBuilder();

            string title = htmlDoc.DocumentNode.SelectSingleNode("//./div[@class='navigation-city']/h1").InnerText;
            sb.AppendLine(title);

            if (queryInfo.Type == WeatherQueryType.NotDefined)
            {
                string currTemp = htmlDoc.DocumentNode.SelectSingleNode("//./div[@class='current-weather__thermometer current-weather__thermometer_type_now']").InnerText;
                string currTempDescr = htmlDoc.DocumentNode.SelectSingleNode("//./span[@class='current-weather__comment']").InnerText;
                string windSpeed = htmlDoc.DocumentNode.SelectSingleNode("//./div[@class='current-weather__info-row current-weather__info-row_type_wind']").InnerText;
                windSpeed = ClearText(windSpeed);
                
                sb.Append(Environment.NewLine);
                sb.AppendFormat("Сейчас {0} {1}", currTemp, currTempDescr, windSpeed);
                sb.Append(Environment.NewLine);
            }
            

            var detailedItems = htmlDoc.DocumentNode.SelectNodes("//./dl[@class='forecast-detailed']/*");
            for (int i = 0; i < detailedItems.Count; i += 2)
            {
                
                var dateBlock = detailedItems[i];
                var detailsBlock = detailedItems[i + 1];

                var dayOfWeek = dateBlock.SelectSingleNode("small").InnerText;
                var date = dateBlock.SelectSingleNode("strong").InnerText;
                var dayDetailRows = detailsBlock.SelectNodes(".//./tr[contains(@class,'weather-table__row')]");

                switch (queryInfo.Type)
                {
                    case WeatherQueryType.DayOfWeek:
                    {
                        if (dayOfWeek != queryInfo.Data.ToString()) continue;
                        break;
                    }
                    case WeatherQueryType.Today:
                    {
                        if(i!=0) continue; //0 iteration always will be first day
                        break;   
                    }
                    case WeatherQueryType.Tomorrow:
                    {
                        if(i!=2) continue;
                        break;
                    }
                    case WeatherQueryType.Aftertomorrow:
                    {
                        if(i!=4) continue;
                        break;
                    }
                    case WeatherQueryType.DayOfMonth:
                    {
                        int day = Convert.ToInt32(Regex.Match(date, @"\d+").Value.Trim());
                        if(day!=(int)queryInfo.Data) continue;
                        break;
                    }
                }
                sb.Append(Environment.NewLine);
                sb.AppendFormat("{0} ({1}) : ", date, dayOfWeek);
                sb.Append(Environment.NewLine);

                foreach (var detailRow in dayDetailRows)
                {
                    var dayPart = detailRow.SelectSingleNode(".//./div[@class='weather-table__daypart']").InnerText;
                    var dayTempRange = detailRow.SelectSingleNode(".//./div[@class='weather-table__temp']").InnerText.Replace("&hellip;", "...");
                    var dayWDescr = detailRow.SelectSingleNode("td[@class='weather-table__body-cell weather-table__body-cell_type_condition']").InnerText;
                    var dayWindSpeed = detailRow.SelectSingleNode(".//./span[@class='weather-table__wind']").InnerText;
                    var dayWindDirection = detailRow.SelectSingleNode(".//./abbr").InnerText;
                    var dayHumidity = detailRow.SelectSingleNode("td[@class='weather-table__body-cell weather-table__body-cell_type_humidity']").InnerText;
                    sb.AppendLine(string.Format("{0} {1}. {2}.", dayPart, dayTempRange, dayWDescr, dayWindSpeed, dayWindDirection, dayHumidity));
                }

            }

            _client.ShowMessage(commandToken, sb.ToString().Replace("&deg;", "°"));
        }

        private string ClearText(string str)
        {
            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex(@"[ ]{2,}", options);
            return regex.Replace(str, @" ").Replace("\n", "").Trim();
        }
    }

    public enum WeatherQueryType
    {
        DayOfWeek,
        Today,
        Tomorrow,
        Aftertomorrow,
        DayOfMonth,
        NotDefined
    }

    public class WeatherQueryInfo
    {
        public WeatherQueryType Type { get; set; }
        public object Data { get; set; }

        public string ParseQuery(string query)
        {
            List<string> daysOfWeek = new List<string>() {"пн", "вт", "ср", "чт", "пт", "сб", "вс"};
            query = query.ToLower().Trim();
            var endWith = daysOfWeek.SingleOrDefault(x => query.EndsWith(" " + x));
            if (endWith != null)
            {
                Type = WeatherQueryType.DayOfWeek;
                Data = endWith;
            }
            else
            {
                 if (query.EndsWith("послезавтра"))
                {
                    Type = WeatherQueryType.Aftertomorrow;
                    Data = "послезавтра";
                } else if (query.EndsWith("завтра"))
                {
                    Type = WeatherQueryType.Tomorrow;
                    Data = "завтра";
                }
                else if (query.EndsWith("сегодня"))
                {
                    Type = WeatherQueryType.Today;
                    Data = "сегодня";
                }
                else
                {
                    if (query.Contains(" "))
                    {
                        string numberOfMonth = query.Split(' ').Last();
                        int numOfMonth;
                        if (int.TryParse(numberOfMonth, out numOfMonth))
                        {
                            Type = WeatherQueryType.DayOfMonth;
                            Data = numOfMonth;
                        }
                    }
                }
            }
            if (Data == null)
            {
                Type = WeatherQueryType.NotDefined;
                Data = "";
            }
            return query.TrimEnd(Data.ToString().ToCharArray()).TrimEnd();
        }
    }
}
