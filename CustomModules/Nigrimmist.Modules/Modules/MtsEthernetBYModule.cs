using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using HelloBotCommunication;
using HelloBotCommunication.Attributes.SettingAttributes;
using HelloBotCommunication.Interfaces;
using HelloBotModuleHelper;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Nigrimmist.Modules.Entities;

namespace Nigrimmist.Modules.Modules
{
    public class MtsEthernetBYModule : ModuleEventBase
    {
        private IClient _client;
        

        public override void Init(IClient client)
        {
            _client = client;
            var settings = _client.GetSettings<ToolUpdateSettings>();
            if (settings == null)
            {
                _client.SaveSettings(new ToolUpdateSettings()
                {
                    LastUpdateCheck = new DateTime(1988, 06, 22)
                });
            }
        }

        public override string ModuleTitle
        {
            get { return "МТС ИНТЕРНЕТ .BY"; }
        }

        public override string ModuleDescription
        {
            get { return ""; }
        }

        public override TimeSpan RunEvery
        {
            get { return TimeSpan.FromHours(1); }
        }

        public override void OnFire(Guid eventToken)
        {
            var settings = _client.GetSettings<MtsEthernetByModuleSettings>();
            if (settings != null )
            {
                if (string.IsNullOrEmpty(settings.Login) || string.IsNullOrEmpty(settings.Password))
                {
                    _client.ShowMessage(eventToken, "Заполните логин и пароль от сайта internet.mts.by", messageType: MessageType.Error);
                    return;
                }

                HtmlReaderManager hrm  = new HtmlReaderManager();
                hrm.Get("https://internet.mts.by/login?referer=%2F");
                HtmlDocument htmlDoc = new HtmlDocument {OptionFixNestedTags = true};
                htmlDoc.LoadHtml(hrm.Html);
                string token = htmlDoc.DocumentNode.SelectSingleNode("//*/input[@name='authenticity_token']").InnerText;
                hrm.Post("https://internet.mts.by/login", string.Format("utf8=%E2%9C%93&authenticity_token={0}&referer=%2F&login=001012917&password=8293229&commit=", HttpUtility.UrlEncode(token)));
                htmlDoc.LoadHtml(hrm.Html);
                string amount = htmlDoc.DocumentNode.SelectSingleNode("//./div[@class='full-column']/table/tbody/tr[3]/td[2]").InnerText;
            }
        }
    }

    [ModuleSettingsFor(typeof(MtsEthernetBYModule))]
    public class MtsEthernetByModuleSettings
    {
        [SettingsNameField("Логин")]
        public string Login { get; set; }

        [SettingsNameField("Пароль")]
        public string Password { get; set; }

        [SettingsNameField("Границы оповещений")]
        public List<MtsEthernetWarningBorder> WarningBorders { get; set; }

        public int? LastScannedValue { get; set; }
        public int? LastWarningAmount { get; set; }

        public MtsEthernetByModuleSettings()
        {
            WarningBorders = new List<MtsEthernetWarningBorder>();
        }
    }

    public class MtsEthernetWarningBorder
    {
        [SettingsNameField("Оповещать если осталось менее (.руб)")]
        public int Border { get; set; }
    }
}

