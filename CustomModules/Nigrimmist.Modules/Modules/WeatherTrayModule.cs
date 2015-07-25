using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using HelloBotCommunication;
using HelloBotCommunication.Attributes.SettingAttributes;
using HelloBotCommunication.Interfaces;
using HelloBotModuleHelper;
using HtmlAgilityPack;

namespace Nigrimmist.Modules.Modules
{
    public class WeatherTrayModule : ModuleTrayBase
    {
        private ITrayClient _client;
        public override void Init(ITrayClient client)
        {
            _client = client;
        }

        public override TimeSpan RunEvery
        {
            get { return TimeSpan.FromMinutes(5); }
        }

        public override void OnFire(Guid trayModuleToken)
        {
            var settings = _client.GetSettings<WeatherTraySettings>();
            if (settings != null && !string.IsNullOrEmpty(settings.City))
            {
                HtmlReaderManager hrm = new HtmlReaderManager();

                try
                {
                    hrm.Get(string.Format("http://pogoda.yandex.by/{0}", HttpUtility.UrlEncode(settings.City)));
                }
                catch (WebException wex)
                {
                    if (((HttpWebResponse)wex.Response).StatusCode == HttpStatusCode.NotFound)
                    {
                        _client.ShowTrayBalloonTip(trayModuleToken,"Неверно задан город",tooltipType:TooltipType.Error);
                        return;
                    }
                }

                string html = hrm.Html;
                HtmlDocument htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);

                string currTemp = htmlDoc.DocumentNode.SelectSingleNode("//./div[@class='current-weather__thermometer current-weather__thermometer_type_now']").InnerText.Replace("+", "").Replace(" ", "");
                _client.UpdateTrayText(trayModuleToken, currTemp , Color.White, Color.Black, 6);
            }
            else
            {
                _client.ShowTrayBalloonTip(trayModuleToken,"Для показа текущей температуры, заполните город в настройках модуля",tooltipType:TooltipType.Error);
            }
            
        }

        public override string IconInBase64
        {
            get { return "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAhElEQVQ4je3SsQlCMRQF0FNkCEdwAAvHsHAFCzcQEZRv4QguY/cFx7DIIN/CV1j8iA8LGy9cCCE5D0JoZ4ZbdP7mXDM9FlgGks7QWKeAKSbfAPvoH/gFUEaAkgE2nj/xHO2xywB3rGLygDVqBqjoXoBDFjjhim30Ensfp+AYU2tcHn3EB5R/MKcp5BbYAAAAAElFTkSuQmCC"; }
        }

        public override string ModuleTitle
        {
            get { return "Градусник"; }
        }

        public override string ModuleDescription
        {
            get { return "Выводит в трей текущую температуру в вашем городе. Укажите в настройках ваш город и наблюдайте как меняется температура за вашим окном.";}
        }

        public override string TrayIconInBase64
        {
            get { return "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAhElEQVQ4je3SsQlCMRQF0FNkCEdwAAvHsHAFCzcQEZRv4QguY/cFx7DIIN/CV1j8iA8LGy9cCCE5D0JoZ4ZbdP7mXDM9FlgGks7QWKeAKSbfAPvoH/gFUEaAkgE2nj/xHO2xywB3rGLygDVqBqjoXoBDFjjhim30Ensfp+AYU2tcHn3EB5R/MKcp5BbYAAAAAElFTkSuQmCC"; }
        }

        [ModuleSettingsFor(typeof(WeatherTrayModule))]
        public class WeatherTraySettings
        {
            [SettingsNameField("Город")]
            public string City { get; set; }
        }
    }
}
