using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
    public class ProcessTitleCheck : ModuleEventBase
    {
        private IClient _client;


        public override void Init(IClient client)
        {
            _client = client;
        }

        public override string Title
        {
            get { return "Process Title checker"; }
        }

        public override string ModuleDescription
        {
            get { return "Сообщает об изменившимся заголовке приложения (процесса)"; }
        }

        public override TimeSpan RunEvery
        {
            get { return TimeSpan.FromMinutes(5); }
        }


        public override void OnFire(Guid eventToken)
        {
            var settings = _client.GetSettings<ProcessTitleSettings>();
            if (settings != null)
            {
                Process[] processes = Process.GetProcesses();
                foreach (var process in processes)
                {
                    if (process.MainWindowTitle.Contains(settings.TitleContains))
                    {
                        var skypeCaption = process.MainWindowTitle;
                        if (skypeCaption.Contains(settings.TitleAlertContains))
                        {
                            _client.SendMessage(eventToken, CommunicationMessage.FromString(skypeCaption + " Title changed!"));
                        }else
                            _client.SendMessage(eventToken, CommunicationMessage.NoContent());
                        break;
                    }
                }
            }
        }
    }
    


        [ModuleSettingsFor(typeof (ProcessTitleCheck))]
        public class ProcessTitleSettings
        {
            [SettingsNameField("Title содержит")]
            public string TitleContains { get; set; }

            [SettingsNameField("Уведомлять если Title содержит")]
            public string TitleAlertContains { get; set; }
            
            public ProcessTitleSettings()
            {
            }
        }
    }

