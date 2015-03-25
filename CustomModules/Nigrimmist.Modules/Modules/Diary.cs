using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Web;
using HelloBotCommunication;
using HelloBotCommunication.Interfaces;
using HelloBotModuleHelper;
using HtmlAgilityPack;

namespace Nigrimmist.Modules.Modules
{
    public class Diary : ModuleEventBase
    {
        private IClient _client;
        private DiarySettings _settings;
        private HtmlReaderManager hrm;
        public override void Init(IClient client)
        {
            _client = client;
            _settings = _client.GetSettings<DiarySettings>();

            //save empty settings for manual edit
            if (_settings == null)
            {
                _settings = new DiarySettings()
                {
                    CheckDiscussions = true,
                    CheckNewComments = true,
                    CheckUmails = true,
                    Password = "",
                    UserName = ""
                };
                _client.SaveSettings(_settings);

            }
        }

        public override double ModuleVersion
        {
            get { return base.ModuleVersion; }
        }

        public override TimeSpan RunEvery
        {
            get { return TimeSpan.FromMinutes(5);}
        }

        public override void OnFire(Guid eventToken)
        {
            if (_settings != null && !string.IsNullOrEmpty(_settings.UserName) && !string.IsNullOrEmpty(_settings.Password))
            {
                //
                if (hrm == null)
                {
                    hrm = new HtmlReaderManager();
                    hrm.Encoding = Encoding.GetEncoding(1251);
                    if (!Login(eventToken))
                    {
                        return;
                    }
                }
                
                if (!IsAuthenticated())
                {
                    if (!Login(eventToken))
                    {
                        return;
                    }
                    
                }

                int newUmails;
                int newDiscussions;
                int newComments;

                GetNotifies(hrm.Html, out newUmails, out newComments, out newDiscussions);
                StringBuilder sbMessage = new StringBuilder();
                if (newUmails > 0)
                {
                    sbMessage.Append("Новые U-Mail (" + newUmails+")"+Environment.NewLine);
                }
                if (newDiscussions > 0)
                {
                    sbMessage.Append("Новые дискуссии (" + newDiscussions+")"+Environment.NewLine);
                }
                if (newComments > 0)
                {
                    sbMessage.Append("Новые комментарии (" + newComments+")");
                }

                string toReturn = sbMessage.ToString();
                if (!string.IsNullOrEmpty(toReturn))
                {
                    toReturn = ("На дневнике : " + Environment.NewLine + Environment.NewLine) + toReturn;

                    _client.ShowMessage(eventToken, toReturn).OnClick(() =>
                    {
                        _client.ShowMessage(eventToken, "http://diary.ru", answerType: AnswerBehaviourType.OpenLink);
                    });
                }
            }
        }

        private bool IsAuthenticated()
        {
            hrm.Get("http://diary.ru");
            var htmlXml = new HtmlDocument();
            htmlXml.LoadHtml(hrm.Html);
            return htmlXml.GetElementbyId("m_menu")!=null;
        }

        private bool Login(Guid eventToken)
        {
            
            string postUrl = String.Format("user_pass={1}&user_login={0}",
                                                               HttpUtility.UrlEncode(_settings.UserName, Encoding.GetEncoding(1251)),
                                                               HttpUtility.UrlEncode(_settings.Password, Encoding.GetEncoding(1251)));
            hrm.Post("http://www.diary.ru/login.php", postUrl);

            if (hrm.Html.Contains("ip изменился"))
            {
                _client.ShowMessage(eventToken, "Логин/пароль не верны", messageType: MessageType.Error);
                return false;
            }

            if (hrm.Html.Contains("Ваш доступ временно заблокирован"))
            {
                _client.ShowMessage(eventToken, "Слишком много попыток входа. Доступ заблокирован на 10 минут сервисом.", messageType: MessageType.Error);
                return false;
            }

            if (hrm.Html.Contains("Доступ к дневнику ограничен"))
            {
                _client.ShowMessage(eventToken, "Доступ к дневнику ограничен. Обновление уведомления возможно только после ввода логина и пароля в файле настроек модуля", messageType: MessageType.Error);
                return false;
            }
            hrm.Get("http://diary.ru");
            return true;
        }

        private void GetNotifies(string html, out int newUmails, out int newComments, out int newDiscussions)
        {
            //html = html.Replace("\r", "").Replace("\n", "").Replace("\t", ""); ;
            newUmails = 0;
            newComments = 0;
            newDiscussions = 0;


            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.OptionFixNestedTags = true;
            htmlDoc.LoadHtml(html);

            var href = htmlDoc.DocumentNode.SelectSingleNode("//a[@href='/u-mail/folder/?f_id=1']");
            if (href != null)
            {
                newUmails = Convert.ToInt32(href.InnerText);
            }

            var comNode =
                htmlDoc.DocumentNode.SelectSingleNode("//li[@id='new_comments_count']/*/a[1]");

            if (comNode != null)
                newComments = Convert.ToInt32(comNode.InnerText);

            var disNode =
                htmlDoc.DocumentNode.SelectSingleNode("//a[@id='menuNewDescussions'][1]");
            if (disNode != null)
            {
                newDiscussions = Convert.ToInt32(disNode.InnerText);
            }

        }
    }

    public class DiarySettings
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public bool CheckUmails { get; set; }
        public bool CheckNewComments { get; set; }
        public bool CheckDiscussions { get; set; }
    }
}
