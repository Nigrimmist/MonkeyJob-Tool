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
    public class MtsGsmByModule : ModuleEventBase
    {
        private IClient _client;

        public override void Init(IClient client)
        {
            _client = client;
        }

        public override string Title
        {
            get { return "МТС Баланс .BY"; }
        }

        public override string ModuleDescription
        {
            get { return "Для Беларуси. Позволяет оповещать вас о нехватке средств GSM провайдера МТС. Для начала работы нужно задать логин/пароль, а так же границы оповещений. Например, если задать границы 40000 и 10000, то уведомление покажется как минимум дважды - при достижении баланса менее чем 40000 и 10000 рублей соответственно. Поддерживается несколько аккаунтов"; }
        }

        public override TimeSpan RunEvery
        {
            get { return TimeSpan.FromHours(3); }
        }

        public override string IconInBase64
        {
            get { return "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAACXBIWXMAAAsTAAALEwEAmpwYAAAAIGNIUk0AAHolAACAgwAA+f8AAIDoAABSCAABFVgAADqXAAAXb9daH5AAAAJtSURBVHjalJPLb01hFMV/33lxq73aom3qkSplIAYEaSIpEREa/gMdSky0o0okpiKpEMGESAxEjEwMiJlJEyOJAVKPcLkNbW/Pvec+zrnfOd/DoNXWY8Ae7uy11l4rWeL5gcEAuAKcBjr5tykD94FxD5ho6e8f7Tp+HKtiVDXExlUq798xP/MdmzTJ1eu0SBdVqCBrkkbgdPjbukZXt+UTBxjZcPQIQhislDjWMjs1RW1tN3uv3mH/3YeIoWOEc3MIwCDQKiMsfgE44wGdViWopgIlqRQ+Iju6OHT1Bq7nATB07SaTmaJ+6x4ObRjHkqkUoNMBULV5VK2MikJmit/Yd/7iEthaC8CeS5fROwfISDAYjFjYOwBZVEE3apTeTbHh4GHyGzf9kphWily+nS3nztIgQQmDEiwT6KiCiUIaYZm+Eyf/GnuqFD3Dw3jre0iNRDkrPtDVMnL2O25rnvbtA78Af1rQacqq3o3kd++iqTXZSgumUiWdKZFb14Xn+3+ArbVYrbG+R2tfHxJIF288AFWuoqIIH/ev71trMdaCMbh+gAQUdplAT0cQx6QfC1hA/EZgjCFTGW5TUi98QrrgstLCdAQlSfPFSyqv3yyBjDFIKYnjmEwIzNci0x9eYYJlEQcgbmqSDLJajbfjF2jGCYnjUE1iqnGMFIJVuBQfPaBY+obnsqi/YCGsr7adRmVAQPT0MenwKfpHx8jtGMDLBehyyOdnT5i8e5ssNbi+wPd8gFA8PzB4Pa5FY6XpAkplaAFZkhFYWNPbjbt5PfVGwuyXAjrVuIEgcAP6e7bS3to+IVbUeQTo+N86/xgAQQ1ECVf8o84AAAAASUVORK5CYII="; }
        }

        public override void OnFire(Guid eventToken)
        {
            var settings = _client.GetSettings<MtsGsmByModuleModule>();
            if (settings!=null && settings.Accounts.Any())
            {
                foreach (var account in settings.Accounts)
                {
                    if (account.WarningBorders.Any())
                    {
                        if (string.IsNullOrEmpty(account.Login) || string.IsNullOrEmpty(account.Password))
                        {
                            _client.SendMessage(eventToken, CommunicationMessage.FromString("Заполните логин и пароль от сайта mts.by"), messageType: MessageType.Error);
                            return;
                        }

                        HtmlReaderManager hrm = new HtmlReaderManager();
                        hrm.Get("https://ihelper.mts.by/Selfcare/logon.aspx");
                        HtmlDocument htmlDoc = new HtmlDocument {OptionFixNestedTags = true};
                        htmlDoc.LoadHtml(hrm.Html);
                        string viewstate = htmlDoc.DocumentNode.SelectSingleNode("//*/input[@id='__VIEWSTATE']").Attributes["value"].Value;
                        hrm.Post("https://ihelper.mts.by/Selfcare/logon.aspx", string.Format(@"__VIEWSTATE={0}&ctl00%24MainContent%24tbPhoneNumber={1}&ctl00%24MainContent%24tbPassword={2}&ctl00%24MainContent%24btnEnter=%D0%92%D0%BE%D0%B9%D1%82%D0%B8", HttpUtility.UrlEncode(viewstate), account.Login, account.Password));
                        if (hrm.Html.Contains("Введён неверный пароль/телефон"))
                        {
                            _client.SendMessage(eventToken, CommunicationMessage.FromString("Неправильные логин/пароль"), messageType: MessageType.Error);
                            return;
                        }

                        htmlDoc.LoadHtml(hrm.Html);
                        string amountStr = htmlDoc.DocumentNode.SelectSingleNode("//./span[@id='customer-info-balance']/strong").InnerText;
                        if (amountStr.Contains(","))
                        {
                            amountStr = amountStr.Split(',').First();
                        }
                        
                        amountStr = Regex.Match(amountStr, @"(-|\d)+").Value;
                        int amount = Int32.Parse(amountStr);
                        
                        var topBorders = account.WarningBorders.Where(x => x.Border >= amount).OrderBy(x => x.Border);
                        if (topBorders.Any())
                        {
                            var topBorder = topBorders.First();
                            if (topBorder != null)
                            {
                                var shouldNotify = !account.LastWarningBorder.HasValue || account.LastWarningBorder.Value != topBorder.Border || (!account.LastScannedAmount.HasValue || account.LastScannedAmount.Value < amount);
                                if (shouldNotify)
                                {
                                    account.LastWarningBorder = topBorder.Border;
                                    account.LastScannedAmount = amount;
                                    _client.SendMessage(eventToken, CommunicationMessage.FromString(string.Format("Время пополнить баланс номера : \r\n\r\n+375 {1}\r\n\r\n На счету осталось {0} руб", amount, account.Login)));
                                    _client.SaveSettings(settings);
                                }
                            }
                        }

                        if (account.LastScannedAmount.HasValue && account.LastScannedAmount < amount && settings.IsNotifyAboutRefill)
                        {
                            account.LastScannedAmount = amount;
                            _client.SendMessage(eventToken, CommunicationMessage.FromString(string.Format("Баланс аккаунта {1} успешно пополнен до {0} руб", amount, account.Login)));
                            _client.SaveSettings(settings);
                        }
                    }
                }
            }
        }
    }


    [ModuleSettingsFor(typeof(MtsGsmByModule))]
    public class MtsGsmByModuleModule
    {
        [SettingsNameField("Аккаунты")]
        public List<MtsGsmByModuleAccount> Accounts { get; set; }

        public bool IsNotifyAboutRefill { get; set; }

        public MtsGsmByModuleModule()
        {
            Accounts = new List<MtsGsmByModuleAccount>();
            IsNotifyAboutRefill = true;
        }
    }
    
    public class MtsGsmByModuleAccount
    {
        [SettingsNameField("Телефон")]
        public string Login { get; set; }

        [SettingsNameField("Пароль")]
        public string Password { get; set; }

        [SettingsNameField("Границы оповещений")]
        public List<MtsEthernetWarningBorder> WarningBorders { get; set; }
       
        public int? LastWarningBorder { get; set; }
        public int? LastScannedAmount { get; set; }

        public MtsGsmByModuleAccount()
        {
            WarningBorders = new List<MtsEthernetWarningBorder>();
        }
    }

    public class MtsGsmWarningBorder
    {
        [SettingsNameField("Оповещать если осталось менее (.руб)")]
        public int Border { get; set; }
    }
}

