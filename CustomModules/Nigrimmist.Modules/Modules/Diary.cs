using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using HelloBotCommunication;
using HelloBotCommunication.Attributes.SettingAttributes;
using HelloBotCommunication.Interfaces;
using HelloBotModuleHelper;
using HtmlAgilityPack;

namespace Nigrimmist.Modules.Modules
{
    public class Diary : ModuleEventBase
    {
        private IClient _client;
        
        
        public override void Init(IClient client)
        {
            _client = client;
        }

        public override string ModuleTitle
        {
            get { return "Diary"; }
        }

        public override string ModuleDescription
        {
            get { return "Оповещения о новых дискуссиях, u-mail'ах и комментариях на сайте Diary.ru"; }
        }

        public override TimeSpan RunEvery
        {
            get { return TimeSpan.FromMinutes(5);}
        }



        private Dictionary<string, HtmlReaderManager> _dict = new Dictionary<string, HtmlReaderManager>(); 

        public override void OnFire(Guid eventToken)
        {
            DiarySettings settings = _client.GetSettings<DiarySettings>();
            if (settings != null)
            {
                foreach (var diary in settings.DiaryList.Where(x => !string.IsNullOrEmpty(x.UserName) && !string.IsNullOrEmpty(x.Password)))
                {
                    HtmlReaderManager hrm;
                    if (!_dict.TryGetValue(diary.UserName, out hrm))
                    {
                        hrm = new HtmlReaderManager();
                        hrm.Encoding = Encoding.GetEncoding(1251);
                        _dict.Add(diary.UserName, hrm);
                        if (!Login(eventToken,diary.UserName,diary.Password,hrm))
                        {
                            continue;
                        }
                    }

                    if (!IsAuthenticated(hrm))
                    {
                        if (!Login(eventToken, diary.UserName, diary.Password, hrm))
                        {
                            continue;
                        }
                    }

                    int newUmails;
                    int newDiscussions;
                    int newComments;

                    GetNotifies(hrm.Html, out newUmails, out newComments, out newDiscussions);
                    StringBuilder sbMessage = new StringBuilder();
                    if (newUmails > 0 && diary.CheckUmails)
                    {
                        sbMessage.Append("Новые U-Mail (" + newUmails + ")" + Environment.NewLine);
                    }
                    if (newDiscussions > 0 && diary.CheckDiscussions)
                    {
                        sbMessage.Append("Новые дискуссии (" + newDiscussions + ")" + Environment.NewLine);
                    }
                    if (newComments > 0 && diary.CheckNewComments)
                    {
                        sbMessage.Append("Новые комментарии (" + newComments + ")");
                    }

                    string toReturn = sbMessage.ToString();
                    if (!string.IsNullOrEmpty(toReturn))
                    {
                        toReturn = (@"На дневнике """+diary.UserName+@""" : " + Environment.NewLine + Environment.NewLine) + toReturn;

                        _client.ShowMessage(eventToken, toReturn).OnClick(() =>
                        {
                            _client.ShowMessage(eventToken, "http://diary.ru", answerType: AnswerBehaviourType.OpenLink);
                        });
                    }
                }
            }
        }

        private bool IsAuthenticated(HtmlReaderManager hrm)
        {
            hrm.Get("http://diary.ru");
            var htmlXml = new HtmlDocument();
            htmlXml.LoadHtml(hrm.Html);
            return htmlXml.GetElementbyId("m_menu")!=null;
        }

        private bool Login(Guid eventToken, string username, string password, HtmlReaderManager hrm)
        {
            
            string postUrl = String.Format("user_pass={1}&user_login={0}",
                                                               HttpUtility.UrlEncode(username, Encoding.GetEncoding(1251)),
                                                               HttpUtility.UrlEncode(password, Encoding.GetEncoding(1251)));
            hrm.Post("http://www.diary.ru/login.php", postUrl);

            if (hrm.Html.Contains("ip изменился"))
            {
                _client.ShowMessage(eventToken, string.Format(@"Логин/пароль для ""{0}"" не верны", username), messageType: MessageType.Error);
                return false;
            }

            if (hrm.Html.Contains("Ваш доступ временно заблокирован"))
            {
                _client.ShowMessage(eventToken, "Слишком много попыток входа. Доступ заблокирован на 10 минут сервисом.", messageType: MessageType.Error);
                return false;
            }

            if (hrm.Html.Contains("Доступ к дневнику ограничен"))
            {
                _client.ShowMessage(eventToken, string.Format(@"Доступ к дневнику ""{0}"" ограничен. Обновление уведомления возможно только после ввода логина и пароля в файле настроек модуля", username), messageType: MessageType.Error);
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


            HtmlDocument htmlDoc = new HtmlDocument();
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

        public override string IconInBase64
        {
            #region icon
            get
            {
                return @"iVBORw0KGgoAAAANSUhEUgAAABoAAAAaCAYAAACpSkzOAAAABGdBTUEAALGPC/xhBQAAACBjSFJNAAB6JgAAgIQAAPoAAACA6AAAdTAAAOpgAAA6mAAAF3CculE8AAAABmJLR0QA/wD/AP+gvaeTAAAAB3RJTUUH3wQFBzQ0cjNDbwAACXpJREFUSMdNlGlQlGcWhc/7fV8D3TR0A20vsmMIoriwCG6gBNwwm2ti1KRwKpVM1KnElIljpVJxYsXMGJ1UNsc4mtWoGHEJYqIGlYgiiihLK6Ds0iwNTe/Nt7zv/CBJza06f+6fU7fucw7w+zDG/pTP7+f7+gdUbq+X/789V9/QKJy/cFGlKAppbWnBo14bnE4Xl5SUxFutVkiSRBhj/Lq1azFv/nzO5fESSaZobmkFoZSCUoqu7h6YjEYMOxwcz3O0t68PXp8PdvsQP2/OHLR3dLKszHTV6OgoXbFipXLyZCkZHnEyszGZur29446e/GlJX3dnrtMxxMc9ltKw+eXirwE4KKUEACNXrlYBADRqNUwmI6ItFnAcFw0gFUALgG4ADECC2+PdJYqSJipCt9nnD3RpNGoAMLy2afOZoaYbsyJ8dub1uiHpo4kxPbf0k08/LQbgAkCE7MwMACDfHDqIVzduYuW/XikqObT/k5HezsgwfaR78XPrTr7w3OrdX+w78M7tW9ee9/u8bG7eE0Or16zbr9Hg/s7de/9pu1w6Sy86AmaDjiQn6VhHXyeqr/qW79l3oLb0l4oPxP4ungBA8asbhe7eXnn18md3fPPRzu2Jkl1wijKaHUB0tJFxplhfcoxeU1SYo4iSwiqrG1Qdj4YDQ06/GOho1M41ClyTrKMjIw6sjuWhD+Z95TamjSpcc/TgZx+vAcCRN97axj+ekaPEm6KKd2/duH8O18eHB6vIDUk/mPXcq1eO7Nm5SKMLCf/XrrcVSVL4w8fOICEhjibGWsj2D74kWdPSOy2Jib1FK9d8V33l0uyOq+XreNcw+Memu97Y8eHLJCio5P6d2wKxeUQofk/Wti2vnwu6/ZMhN0YbuGAXQhwJ2e+V/3j0/MyZORWTJphDEqLHYcTlQUziRNxvqseyxbPka7VWIdScdnbb1jeXAxABmL8/cWrVw9bWiWtfXF+SHG258spHX+A/b/4VfE1zO86UlT/hfVi/bq5e9vd7AuoeXbLrWFnZeypGg0tLT6yPjp0gtPUMIsqciKBQAzx+GeEawj2zJFcpOXYiRSKh1ilpkxoLCgsC09PSqp8uWlweFR7WmZ+fj+P7PwMAcC2Nd9FirWfMO4JQjioP/CpET5lRpyGkesf7H6w06oODZd+AMi1lPCJDKb78/N9w9LfjsQkJ0IbpsXh+BupuXt0BAPetTaSs7IwqMjJSqKyqEioqKsgfOSVv79oLJksreu9c+9F+/Wc5ZPLs/gOlZesNWo33pbWrzq9anKHLyEino6OjnMvjg6JQMMag12lx+dpdpiIUNXX3pJXFb643jDOVdLU1q4qWLJIOHjyEDRuKQQgZE2MMAHQXKy5taLQ2vv/sU0//PTEh4dO/vLJ5jzjctmXLK8tEjTZCJcoy0ajVEEUJkiRDq1Wj5OQFpCTHy3X194S6lqHGM6dPFgLov1pVw8XFWaharcY4gwGMMRAmuQkELQOg37P3ixqvGOyamqov+eHrr97TaZh6aeEs2tzl5Cqv3YIhUovXildAF6aG0+3HpRv1WJI3A1GROnnfwR8Euy/0t/1f7n8eQO/Z8nPck0uLKKV07Ee1dfXcWNn5p9VWV0Y2tMmZez4+tHNp3uMh06dNZR02D3f4+DmUn7+MXytrERoZjwmTZ2HAIUHldEOnC0dbVx9fkJsd4AJ9uW9ve2cjIQRPLi1iN2pugVIKjuPAZc6YrdRcv6TKzzReWbN6wWHitUIfuxB1LYMsJFhAuD4C72zZgEULC/H9tweRkpyCgDoKDWcqEHnsNEbtQ7C73KSjdzjoycIsOmBr+9uRUxfiN279Bzt16iTheR6MMXCSJCFtaja3d18Z5sxf2uAfvEmDA7eI09XPzZqRRvr6+uHxiVi+7BnMmVcIhdfgvwcOobOmGtMNkWAeD0xmA4aGRrhYs4nKHruWk9zLPt/9LjKnTyaMMVBKIQz093PRMTGj6TnzkrZu314s8mouVmPHb9fuoPzCDSTEGHD4+FlEhIego7URjAhoaGrC6k3rYR1ywn2vHbYRD1o7e3G7pY21d9qg4lkYABgMxj/xFuwuJ29BDPUoSmH3zZrZobZH8lBwMj8pMw93H/bA7vYhLiYaVJHgHB6AtaUdSYkJqO+0wSXJ8La54XO5ERIIwG5tI419/XAH/OMB4EjJCTYvv2DsIoPJJHMAwnn+eHyE/nXjrbupXtuwMqAO4l0ROvRYxsGSmowuWx8CvgBm586H5PPBca8NyrATJjCE9vRCGnIhNDOVf2HrS3DaOpd5ZPZG1c3ugCiKRBAEJkRHGUAI4QA43t3+Vr+vqjo1nnEkRqaQbMNwdffjfm0TbFQBUQnosbZhfP8QJpuiMGVRPsLHRSHKbMT+XZ8gMnsS84sBMjiiBIfyIDNzYgFJHMNbFEXWaG0SzGkpmDo3r8IVHISArNARQiAFq2DUapGiCkISr0K0KMFgH8Z0sxHZL64CCwlBUMoUWNasQXBuJkYG7Kze2g6jyfwVIcSv4wg8Hg8jhEDgeR5x8Qn4uvQUJlrGP6iymCVNcxuvDlJBpBR+RYYMApEnoESFIX8AIakTMCqOwpySgslFC+CzPQJ91KeEp0Zz8xY8NTg+MfvDO3ebEPDaSUREBAMAgeM4BAmCtDB5IgjwM5ea6ii702AUCKOxAJcSrAYvS+AUiiDGICoKJP8oImJiUHbwW3x25hQUlUAfVV6niZqFvCYpdVN5b9NA6w/HuG8Pl9BAIAC1Wg2BUgpFkoOaH7aPmkymFT1uh96Zn60kJSdwTXes0N1sRkyUXmriwHFU4RGmZbS1Q3547pJw16Qj03LSUHe3lQ3PylKZTZbKgvQZVwrSZ6ApMRUOhwMRERFj1AEA4Ti5qcuJKEvchbTUVHFKbLB2UmoyuzwhTjl9r52mOByqESqxMMZkpyxzD2Sqqiw7SzM2vSgvX5THZyYZqLU7wE+e/dT3p89X9WsFKSgrY7qI31PEcdyYkVodokhtp4hxcU7nsaNHGh80Xpypi9RSosi8bkEuT7XjzseNDEdZq2syk9Ime6TomF/Msu/pEJ1OVVvXqNh6u1QB3vioMC+nFgAGB+2KTq+HoiggZMyNUEpBCMH169eJ0+1jWZnpuYe/++ZiXd2toIULiyqS4+O+mjE3t7Ty1k1LW1t7viU+vnFRTk5tY1P9nIetD5Jdbu+7lEGvDdPlrVz+TN35i5e4BQXz6R9lyhgDx3H4H4uJqdtLcVdfAAAAJXRFWHRkYXRlOmNyZWF0ZQAyMDE1LTA0LTA1VDA3OjUyOjUyLTA0OjAwLFRp2wAAACV0RVh0ZGF0ZTptb2RpZnkAMjAxNS0wNC0wNVQwNzo1Mjo1Mi0wNDowMF0J0WcAAAAASUVORK5CYII=";
            } 
            #endregion
        }

        public override Color? HeaderBackGroundColor
        {
            get { return Color.LightCoral; }
        }
    }


    [ModuleSettingsFor(typeof(Diary))]
    public class DiarySettings
    {
        [SettingsNameField("Аккаунты")]
        public List<DiaryItem> DiaryList { get; set; }

        public DiarySettings()
        {
            DiaryList = new List<DiaryItem>();
        }
    }

    public class DiaryItem
    {
        [SettingsNameField("Логин")]
        public string UserName { get; set; }
        [SettingsNameField("Пароль")]
        public string Password { get; set; }
        [SettingsNameField("Проверять U-mail?")]
        public bool CheckUmails { get; set; }
        [SettingsNameField("Проверять комментарии?")]
        public bool CheckNewComments { get; set; }
        [SettingsNameField("Проверять дискуссии?")]
        public bool CheckDiscussions { get; set; }
    }
}
