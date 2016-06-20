using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        }

        public override string Title
        {
            get { return "МТС ИНТЕРНЕТ .BY"; }
        }

        public override string ModuleDescription
        {
            get { return "Для Беларуси. Позволяет оповещать вас о нехватке средств интернет провайдера МТС. Для начала работы нужно задать логин/пароль, а так же границы оповещений. Например, если задать границы 40000 и 10000, то уведомление покажется как минимум дважды - при достижении баланса менее чем 40000 и 10000 рублей соответственно."; }
        }

        public override TimeSpan RunEvery
        {
            get { return TimeSpan.FromHours(10); }
        }

        public override string IconInBase64
        {
            get { return "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAACXBIWXMAAAsTAAALEwEAmpwYAAAAIGNIUk0AAHolAACAgwAA+f8AAIDoAABSCAABFVgAADqXAAAXb9daH5AAAAJtSURBVHjalJPLb01hFMV/33lxq73aom3qkSplIAYEaSIpEREa/gMdSky0o0okpiKpEMGESAxEjEwMiJlJEyOJAVKPcLkNbW/Pvec+zrnfOd/DoNXWY8Ae7uy11l4rWeL5gcEAuAKcBjr5tykD94FxD5ho6e8f7Tp+HKtiVDXExlUq798xP/MdmzTJ1eu0SBdVqCBrkkbgdPjbukZXt+UTBxjZcPQIQhislDjWMjs1RW1tN3uv3mH/3YeIoWOEc3MIwCDQKiMsfgE44wGdViWopgIlqRQ+Iju6OHT1Bq7nATB07SaTmaJ+6x4ObRjHkqkUoNMBULV5VK2MikJmit/Yd/7iEthaC8CeS5fROwfISDAYjFjYOwBZVEE3apTeTbHh4GHyGzf9kphWily+nS3nztIgQQmDEiwT6KiCiUIaYZm+Eyf/GnuqFD3Dw3jre0iNRDkrPtDVMnL2O25rnvbtA78Af1rQacqq3o3kd++iqTXZSgumUiWdKZFb14Xn+3+ArbVYrbG+R2tfHxJIF288AFWuoqIIH/ev71trMdaCMbh+gAQUdplAT0cQx6QfC1hA/EZgjCFTGW5TUi98QrrgstLCdAQlSfPFSyqv3yyBjDFIKYnjmEwIzNci0x9eYYJlEQcgbmqSDLJajbfjF2jGCYnjUE1iqnGMFIJVuBQfPaBY+obnsqi/YCGsr7adRmVAQPT0MenwKfpHx8jtGMDLBehyyOdnT5i8e5ssNbi+wPd8gFA8PzB4Pa5FY6XpAkplaAFZkhFYWNPbjbt5PfVGwuyXAjrVuIEgcAP6e7bS3to+IVbUeQTo+N86/xgAQQ1ECVf8o84AAAAASUVORK5CYII="; }
        }

        public override void OnFire(Guid eventToken)
        {
            var settings = _client.GetSettings<MtsEthernetByModuleSettings>();
            if (settings != null && settings.WarningBorders.Any())
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
                string token = htmlDoc.DocumentNode.SelectSingleNode("//*/input[@name='authenticity_token']").Attributes["value"].Value;
                hrm.Post("https://internet.mts.by/login", string.Format("utf8=%E2%9C%93&authenticity_token={0}&referer=%2F&login={1}&password={2}&commit=", HttpUtility.UrlEncode(token),settings.Login,settings.Password));
                if (hrm.Html.Contains("Неверный логин или пароль."))
                {
                    _client.ShowMessage(eventToken, "Неправильные логин/пароль", messageType: MessageType.Error);
                    return;
                }
                
                htmlDoc.LoadHtml(hrm.Html);
                string amountStr = htmlDoc.DocumentNode.SelectSingleNode("//./div[@class='full-column']/table/tr[3]/td[2]").InnerText;
                amountStr = Regex.Match(amountStr, @"\d+").Value;
                int amount = Int32.Parse(amountStr);

                var topBorders = settings.WarningBorders.Where(x => x.Border >= amount).OrderBy(x => x.Border);
                if (topBorders.Any())
                {
                    var topBorder = topBorders.First();
                    if (topBorder != null)
                    {
                        var shouldNotify = !settings.LastWarningBorder.HasValue || settings.LastWarningBorder.Value != topBorder.Border || (!settings.LastScannedAmount.HasValue || settings.LastScannedAmount.Value < amount);
                        if (shouldNotify)
                        {
                            settings.LastWarningBorder = topBorder.Border;
                            settings.LastScannedAmount = amount;
                            _client.ShowMessage(eventToken, string.Format("Время пополнить баланс интернета! На счету осталось {0} руб", amount));
                            _client.SaveSettings(settings);
                        }
                    }
                }
                if (settings.LastScannedAmount.HasValue && settings.LastScannedAmount<amount && settings.IsNotifyAboutRefill)
                {
                    settings.LastScannedAmount = amount;
                    _client.ShowMessage(eventToken, string.Format("Баланс интернета успешно пополнен до {0} руб", amount));
                    _client.SaveSettings(settings);
                }
            }
        }
    }

    [ModuleSettingsFor(typeof(MtsEthernetBYModule))]
    public class MtsEthernetByModuleSettings
    {
        [SettingsNameField("Уведомлять о пополнениях баланса")]
        public bool IsNotifyAboutRefill { get; set; }

        [SettingsNameField("Логин")]
        public string Login { get; set; }

        [SettingsNameField("Пароль")]
        public string Password { get; set; }

        [SettingsNameField("Границы оповещений")]
        public List<MtsEthernetWarningBorder> WarningBorders { get; set; }

       
        public int? LastWarningBorder { get; set; }
        public int? LastScannedAmount { get; set; }

        public MtsEthernetByModuleSettings()
        {
            WarningBorders = new List<MtsEthernetWarningBorder>();
            IsNotifyAboutRefill = true;
        }
    }

    public class MtsEthernetWarningBorder
    {
        [SettingsNameField("Оповещать если осталось менее (.руб)")]
        public int Border { get; set; }
    }
}

