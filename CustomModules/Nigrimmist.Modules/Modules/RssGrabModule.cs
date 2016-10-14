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

namespace Nigrimmist.Modules.Modules
{
    public class RssGrabModule : ModuleEventBase
    {
        private IClient _client;
        
        
        public override void Init(IClient client)
        {
            _client = client;
        }

        public override string Title
        {
            get { return "Новости"; }
        }

        public override string ModuleDescription
        {
            get { return "Оповещения о новостях с onliner.by по ключевым словам"; }
        }

        public override TimeSpan RunEvery
        {
            get { return TimeSpan.FromMinutes(10);}
        }


        public override void OnFire(Guid eventToken)
        {
            RssSettings settings = _client.GetSettings<RssSettings>();
            if (settings != null && settings.Rss.Any())
            {
                bool updateSettings = false;
                foreach (var rss in settings.Rss)
                {
                    List<RssResponseItem> responseItems = new List<RssResponseItem>();
                    using (XmlReader reader = XmlReader.Create(rss.URL))
                    {
                        SyndicationFeed feed = SyndicationFeed.Load(reader);
                        foreach (SyndicationItem item in feed.Items)
                        {
                            if (rss.LastDisplayedDateTime.HasValue && item.PublishDate.DateTime <= rss.LastDisplayedDateTime.Value)
                            {
                                continue;
                            }
                            
                            responseItems.Add(new RssResponseItem()
                            {
                                Url = item.Id,
                                Title = item.Title.Text,
                                PublishDate = item.PublishDate.DateTime
                            });
                        }
                        reader.Close();
                    }
                    if (responseItems.Any())
                    {
                        string[] separators = { ",", ".", "!", "\'", " ", "\'s","(",")","\"","<<",">>","?" };

                        foreach (var item in responseItems)
                        {
                            bool validItem = true;
                            List<string> words = item.Title.Split(separators, StringSplitOptions.RemoveEmptyEntries).ToList();
                            foreach (var word in words)
                            {
                                var w = word.Trim().ToLower();
                                if (rss.WhiteList.Any())
                                {
                                    var isInWhiteList = rss.WhiteList.Any(x => w.Trim().ToLower().Contains(x.Trim().ToLower()));
                                    var isInBlackList = rss.BlackList.Any(y => w.Contains(y.Trim().ToLower()));
                                    if (isInWhiteList && !isInBlackList)
                                    {
                                        validItem = true;
                                        break;
                                    }
                                    else
                                    {
                                        validItem = false;
                                    }
                                }
                                else if(rss.BlackList.Any())
                                {
                                    if (rss.BlackList.Any(x => w.Trim().ToLower().Contains(x.Trim().ToLower())))
                                    {
                                        validItem = false;
                                        break;
                                    }
                                }
                            }

                            if (rss.StopList.Any() && validItem)
                            {
                                validItem = !rss.StopList.Any(x => words.Any(y => y.Equals(x)));
                            }

                            if (validItem)
                            {
                                _client.SendMessage(eventToken, CommunicationMessage.FromUrl(item.Url));
                                rss.LastDisplayedDateTime = item.PublishDate;
                                updateSettings = true;
                            }
                        }
                        
                    }

                }
                if(updateSettings)
                    _client.SaveSettings(settings);
            }
        }
    }

    public class RssResponseItem
    {
        public string Url { get; set; }
        public string Title { get; set; }
        public DateTime PublishDate { get; set; }
        
    }

    [ModuleSettingsFor(typeof(RssGrabModule))]
    public class RssSettings
    {
        [SettingsNameField("Rss ленты")]
        public List<RssSettingsItem> Rss { get; set; }

        public RssSettings()
        {
            Rss = new List<RssSettingsItem>();
        }
    }

    public class RssSettingsItem
    {
        [SettingsNameField("RSS URL")]
        public string URL { get; set; }

        [SettingsNameField("Белый список слов")]
        public List<string> WhiteList { get; set; }

        [SettingsNameField("Чёрный список слов")]
        public List<string> BlackList { get; set; }

        [SettingsNameField("Список стоп-слов")]
        public List<string> StopList { get; set; }

        public DateTime? LastDisplayedDateTime { get; set; }

        public RssSettingsItem()
        {
            WhiteList = new List<string>();
            BlackList = new List<string>();
            StopList = new List<string>();
        }
    }
}
