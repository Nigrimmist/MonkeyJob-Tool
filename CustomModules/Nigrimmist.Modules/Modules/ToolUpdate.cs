using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using HelloBotCommunication;
using HelloBotCommunication.Interfaces;
using HelloBotModuleHelper;
using Newtonsoft.Json;
using Nigrimmist.Modules.Entities;

namespace Nigrimmist.Modules.Modules
{
    public class ToolUpdate : ModuleEventBase
    {
        private IClient _client;
        private const string LatestVersionFileUrlFormat = "https://raw.githubusercontent.com/Nigrimmist/MonkeyJob-Tool/master/ServerData/LatestPublicVersion_{0}.txt";

        public override void Init(IClient client)
        {
            _client = client;
            var settings = _client.GetSettings<ToolUpdateSettings>();
            if (settings == null)
            {
                _client.SaveSettings(new ToolUpdateSettings()
                {
                    LastUpdateCheck = new DateTime(1988,06,22)
                });
            }
        }

        public override string ModuleTitle
        {
            get { return "Program Update checker"; }
        }

        public override string ModuleDescription
        {
            get { return "Модуль проверки наличия новых версий программы"; }
        }

        public override TimeSpan RunEvery
        {
            get { return TimeSpan.FromDays(1); }
        }

        public override void OnFire(Guid eventToken)
        {
            var settings = _client.GetSettings<ToolUpdateSettings>();
            if (settings != null && settings.LastUpdateCheck.AddDays(3) <= DateTime.Now)
            {
                settings.LastUpdateCheck = DateTime.Now.Date;
                _client.SaveSettings(settings);

                HtmlReaderManager hrm = new HtmlReaderManager();

                string langCode;

                switch (_client.ClientLanguage)
                {
                    case ClientLanguage.English:
                        langCode = "en";
                        break;
                    case ClientLanguage.Russian:
                        langCode = "ru";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                hrm.Get(string.Format(LatestVersionFileUrlFormat, langCode));
                string versionJson = hrm.Html.Replace("\t", "");
                AppVersionInfo info = JsonConvert.DeserializeObject<AppVersionInfo>(versionJson);
                var versions = info.Versions.OrderBy(x => x.Version);
                var latestVersion = versions.Last();
                if (latestVersion.Version > _client.UiClientVersion)
                {
                    string message = "Версия v" + latestVersion.Version + " доступна для скачивания. В новой версии :" + Environment.NewLine + Environment.NewLine + latestVersion.WhatsNew + Environment.NewLine + Environment.NewLine + "Кликните для перехода на новую версию";
                    _client.ShowMessage(eventToken, message, "Вышла новая версия.").OnClick(() => { _client.ShowMessage(eventToken, "https://github.com/Nigrimmist/MonkeyJob-Tool/releases", answerType: AnswerBehaviourType.OpenLink); });
                }
            }
        }
    }

    public class ToolUpdateSettings
    {
        public DateTime LastUpdateCheck { get; set; }
    }


}
