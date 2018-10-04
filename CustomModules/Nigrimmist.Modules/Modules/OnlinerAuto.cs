﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using HelloBotCommunication;
using HelloBotCommunication.Attributes.SettingAttributes;
using HelloBotCommunication.Interfaces;
using HelloBotModuleHelper;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Nigrimmist.Modules.Modules
{
    public class OnlinerAuto : ModuleEventBase
    {
        private IClient _client;
        
        
        public override void Init(IClient client)
        {
            _client = client;
        }

        public override string Title
        {
            get { return "Onliner.by Auto"; }
        }

        public override string ModuleDescription
        {
            get { return "Оповещение о появлении новых авто по фильтру"; }
        }

        public override TimeSpan RunEvery
        {
            get { return TimeSpan.FromMinutes(30);}
        }



        public override void OnFire(Guid eventToken)
        {
            HtmlReaderManager hrm = new HtmlReaderManager();
            var settings = _client.GetSettings<OnlinerAutoSettings>();
            if (settings != null)
            {
                try
                {
                    //https://ab.onliner.by/#body_type[]=1&min-year=2005&max-year=2010&currency=USD&sort[]=creation_date&page=1&car[0][2][m]=521
                    string dataToPost = settings.Url.Substring(settings.Url.IndexOf('#')+1);
                    hrm.Post("https://ab.onliner.by/search", dataToPost);
                    var data = JObject.Parse(hrm.Html);
                    List<string> idList = data["result"]["advertisements"].Select(x => ((JProperty)x).Name).ToList();
                    List<string> foundCarIds = new List<string>();
                    foreach (var newId in idList)
                    {
                        if (!settings.LastCheckedIds.Contains(newId))
                        {
                            foundCarIds.Add(newId);
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (foundCarIds.Any())
                    {
                        _client.SendMessage(eventToken, CommunicationMessage.FromString("-------Новые объявления по фильтру-------"));
                        foundCarIds = foundCarIds.Take(10).ToList();
                        int queueInc = 0;
                        foreach (var foundId in foundCarIds)
                        {
                            settings.LastCheckedIds.Insert(0+ queueInc++, foundId);
                            _client.SendMessage(eventToken, CommunicationMessage.FromUrl("https://ab.onliner.by/car/"+ foundId));
                        }
                        settings.LastCheckedIds = settings.LastCheckedIds.GetRange(0, 3);
                        _client.SaveSettings(settings);
                    }

                }
                catch (Exception ex)
                {
                    _client.LogTrace("Ошибка " + ex);
                }
            }
        }



        public override string IconInBase64
        {
            #region icon
            get
            {
                return @"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";
            } 
            #endregion
        }

        public override Color? HeaderBackGroundColor
        {
            get { return Color.Firebrick; }
        }
    }


    [ModuleSettingsFor(typeof(OnlinerAuto))]
    public class OnlinerAutoSettings
    {
        [SettingsNameField("URL страницы поиска")]
        public string Url { get; set; }
        public List<string> LastCheckedIds { get; set; }
        public OnlinerAutoSettings()
        {
            LastCheckedIds = new List<string>();
        }
    }

    
    
}