using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.ServiceModel.Syndication;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Xml;
using HelloBotCommunication;
using HelloBotCommunication.Attributes.SettingAttributes;
using HelloBotCommunication.Interfaces;
using HelloBotModuleHelper;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace Nigrimmist.Modules.Modules
{
    public class InstagramGrabberModule : ModuleEventBase
    {
        private IClient _client;


        public override void Init(IClient client)
        {
            _client = client;
        }

        public override string Title
        {
            get { return "Instagram"; }
        }

        public override string ModuleDescription
        {
            get { return ""; }
        }

        public override TimeSpan RunEvery
        {
            get { return TimeSpan.FromMinutes(10); }
        }

        private static List<string> _titles = new List<string>()
        {
            "Я не бот! Вот вам доказательство",
            "Зацените. Моё...",
            "Доброго времени суток, камрады",
            "Ну что, может прокатимся?",
            "Держите годноту в плечи ;-)",
            "Эрондондон!",
            "Хочу себе такой же",
            "Сколько средних зарплат стоит, как думаете?",
            "Пока вспоминаю отличный анекдот, держите : ",
            "Как дела, байкерята?",
            "У соседа во дворе такой же стоит. Честно!",
            "А ведь тут могла быть ваша реклама! хД",
            "Всем привет",
            "Ну-ка рассказывайте кто чем занят?",
            "Как там погодка за окном, м?",
            "Мне таких два заверните пожалуйста...",
            "Лепота...",
            "Меняю на пакет пельменей",
            "Представляете, иду я тут...а мимо вот такое вот чудо проносится...",
            ""
        };

        public override void OnFire(Guid eventToken)
        {
            if ((DateTime.Now.Hour >= 11 && DateTime.Now.Hour <= 13) || (DateTime.Now.Hour >= 20 && DateTime.Now.Hour <= 23))
            {
                var settings = _client.GetSettings<InstagramSettings>();
                if (settings != null)
                {
                    if (settings.LastPostDate == null || settings.LastPostDate < DateTime.Now.AddHours(-6))
                    {
                        foreach (var item in settings.Items.OrderBy(x => Guid.NewGuid()))
                        {
                            HtmlReaderManager hrm = new HtmlReaderManager();
                            hrm.Get(item.Url + "media");
                            string json = hrm.Html;
                            var resp = JsonConvert.DeserializeObject<InstagramResponse>(json);
                            var images = resp.items.Where(x => x.type == "image" && !item.Posted.Contains(x.images.standard_resolution.Url));
                            if (images.Any())
                            {
                                var url = images.FirstOrDefault().images.standard_resolution.Url;
                                var answer = CommunicationMessage.FromImage(url);

                                var r = new Random();

                                if (r.Next(0, 100) > 80)
                                {
                                    var title = _titles[r.Next(0, _titles.Count - 1)];
                                    answer.AppendString(title);
                                }

                                _client.SendMessage(eventToken, answer);
                                item.Posted.Add(url);
                                settings.LastPostDate = DateTime.Now;
                                break;
                            }

                        }
                    }
                    _client.SaveSettings(settings);
                }
            }
        }
    }

    class InstagramResponse
    {
        public string status { get; set; }
        public List<InstagramItemResponse> items { get; set; }
        public bool more_available { get; set; }
    }

    class InstagramItemResponse
    {
        public InstagramItemImageResponse images { get; set; }
        public string type { get; set; }
    }

    class InstagramItemImageResponse
    {
        public InstagramItemImageResolutionResponse standard_resolution { get; set; }
    }

    class InstagramItemImageResolutionResponse
    {
        public string Url { get; set; }
    }


    [ModuleSettingsFor(typeof(InstagramGrabberModule))]
    public class InstagramSettings
    {
        //[SettingsNameField("Random images?")]
        //public bool IsSendRandom { get; set; }

        //[SettingsNameField("Send count")]
        //public int SendPerFireCount { get; set; }

        //public bool IsForEachOr

        [SettingsNameField("Insta accounts")]
        public List<InstagramItemSettings> Items { get; set; }

        public DateTime? LastPostDate { get; set; }
    }

    [ModuleSettingsFor(typeof(InstagramGrabberModule))]
    public class InstagramItemSettings
    {
        [SettingsNameField("Url")]
        public string Url { get; set; }
        public List<string> Posted { get; set; }

        public InstagramItemSettings()
        {
            Posted = new List<string>();
        }
    }
}
