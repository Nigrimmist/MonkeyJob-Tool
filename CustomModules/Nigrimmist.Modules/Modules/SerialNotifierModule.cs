using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using HelloBotCommunication;
using HelloBotCommunication.Attributes.SettingAttributes;
using HelloBotCommunication.Interfaces;
using HelloBotModuleHelper;
using HtmlAgilityPack;

namespace Nigrimmist.Modules.Modules
{
    public class SerialNotifierModule : ModuleEventBase
    {
        private IClient _client;
        
        
        public override void Init(IClient client)
        {
            _client = client;
        }

        public override string Title
        {
            get { return "Сериалы"; }
        }

        public override string ModuleDescription
        {
            get { return "Оповещения о новых сериях вашего любимого сериала. Как база - используется сайт http://seasonvar.ru/. Для отслеживания любимого сериала - найдите его на сайте, и добавте URL вида 'http://seasonvar.ru/serial-14242-Teoriya_bol_shogo_vzryva-10-season.html' в настройки. Для каждого сериала можно задать слова-триггеры, введя которые можно отслеживать, допустим только сериалы с субтитрами. Или в переводе от определённой студии озвучки. Например если добавить триггер 'кубик' - будет оповещать вас только при выходе серии в озвучке от 'кубика'. Рекомендуем по началу не писать ничего в это поле, если вы не уверены, а получать все обновления статусов, тем самым узнав какие слова-триггеры вообще могут быть использованы. Несколько триггеров совмещаются по принципу логического 'ИЛИ'."; }
        }

        public override TimeSpan RunEvery
        {
            get { return TimeSpan.FromHours(2);}
        }

        private HtmlReaderManager hrm = new HtmlReaderManager();

        public override void OnFire(Guid eventToken)
        {
            SerialsSettings settings = _client.GetSettings<SerialsSettings>();
            if (settings != null && settings.Serials.Any())
            {
                bool updateSettings = false;
                foreach (var serial in settings.Serials)
                {
                    hrm.Get(serial.Url);
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(hrm.Html);
                    var lastSeasonNode = doc.DocumentNode.SelectSingleNode("//div[contains(@class,'seasonlist')]/h2[last()]");
                    var href = lastSeasonNode.SelectSingleNode("a").Attributes["href"].Value;
                    if (!serial.Url.Trim().EndsWith(href.Trim()))
                    {
                        href = "http://seasonvar.ru" + href;
                        hrm.Get(href);
                        doc.LoadHtml(hrm.Html);
                        lastSeasonNode = doc.DocumentNode.SelectSingleNode("//div[contains(@class,'seasonlist')]/h2[last()]");
                        serial.Url = href;
                        updateSettings = true;
                    }
                    var lastSeasonNodeStatusNode = lastSeasonNode.SelectSingleNode("a/span");
                    if (lastSeasonNodeStatusNode != null)
                    {
                        var currStatus = NormalizeString(lastSeasonNodeStatusNode.InnerText);
                        if (currStatus != serial.LastStatus)
                        {
                            bool statusValidated = true;
                            if (serial.StringsToSearch.Any())
                                foreach (var str in serial.StringsToSearch)
                                {
                                    if (!string.IsNullOrEmpty(str))
                                    {
                                        statusValidated = currStatus.ToLower().Contains(str.ToLower().Trim());
                                        if(!statusValidated)
                                            break;
                                    }
                                }

                            if (string.IsNullOrEmpty(serial.LastStatus)) //skip first notify
                                statusValidated = false;

                            serial.LastStatus = currStatus;
                            updateSettings = true;

                            if (statusValidated)
                            {
                                _client.ShowMessage(eventToken,
                                    CommunicationMessage.FromString("У сериала " + serial.Name + " обновился статус новой серии : " + Environment.NewLine + Environment.NewLine + serial.LastStatus)
                                        .AppendUrl(serial.Url));
                            }
                            
                        }
                    }
                }

                if(updateSettings)
                    _client.SaveSettings(settings);
            }
        }

        private string NormalizeString(string val)
        {
            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[ ]{2,}", options);
            return regex.Replace(val, " ").Trim(); //remove double spaces
        }
    }


    [ModuleSettingsFor(typeof(SerialNotifierModule))]
    public class SerialsSettings
    {
        [SettingsNameField("Сериалы")]
        public List<SerialItem> Serials { get; set; }

        public SerialsSettings()
        {
            Serials = new List<SerialItem>();
        }
    }

    public class SerialItem
    {
        [SettingsNameField("Название сериала")]
        public string Name { get; set; }

        [SettingsNameField("URL")]
        public string Url { get; set; }

        [SettingsNameField("Слово-триггер")]
        public List<string> StringsToSearch { get; set; }

        public string LastStatus { get; set; }

        public SerialItem()
        {
            StringsToSearch = new List<string>();
        }
    }
}
