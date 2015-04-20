using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using HelloBotCommunication;
using HelloBotCommunication.Interfaces;
using HelloBotModuleHelper;

namespace Nigrimmist.Modules.Modules
{
    public class Map : ModuleHandlerBase
    {
        private IClient _client;

        public override void Init(IClient client)
        {
            _client = client;
        }

        public override double ModuleVersion
        {
            get { return 1.0; }
        }

        public override ReadOnlyCollection<CallCommandInfo> CallCommandList
        {
            get
            {
                return new ReadOnlyCollection<CallCommandInfo>(new List<CallCommandInfo>()
                {
                    new CallCommandInfo("карта",new List<string>(){"map"}),
                });
            }
        }
        
        public override DescriptionInfo ModuleDescription
        {
            get
            {
                return new DescriptionInfo()
                {
                    Description = "Генерирует ссылку на карту по адресу. Добавьте help для просмотра справки."
                };
            }
        }
       
        private IDictionary<string, string> mapUrlProviders = new Dictionary<string, string>()
        {
            {"g", "http://maps.google.com/?q={0}"},
            {"y", "http://maps.yandex.ru/?text={0}"},
        };

        private IDictionary<string, string> mapDirectionProviders = new Dictionary<string, string>()
        {
            {"g", "https://www.google.com/maps/dir/{0}/{1}"},
            {"y", "https://maps.yandex.ru/?rtext={0}~{1}"},
        };

        private const string _defaultProvider = "g";
        private const string _fromToDelimeter = "=>";

        private string helpMsg = string.Format(@"""!map <опционально:поисковик> <адрес>"", где поисковик может быть y(yandex) или g(google).
Проложить маршрут : ""!map <опционально:поисковик> <из>{0}<в>""", _fromToDelimeter);

        public override void HandleMessage(string command, string args, Guid commandToken)
       {
           

            if (!string.IsNullOrEmpty(args))
            {
                string inputProvider = args.Split(' ').First();

                if (inputProvider == "help")
                {
                    _client.ShowMessage(commandToken,helpMsg);
                }
                else if (args.Contains(_fromToDelimeter))
                {
                    var addressParts = args.Split(new []{_fromToDelimeter},StringSplitOptions.RemoveEmptyEntries);
                    if (addressParts.Count() == 2)
                    {
                        var leftPart = addressParts[0];
                        var rightPart = addressParts[1];
                        string foundProvider;
                        if (!mapDirectionProviders.TryGetValue(inputProvider, out foundProvider))
                        {
                            inputProvider = _defaultProvider;
                            foundProvider = mapDirectionProviders[inputProvider];
                        }
                        else
                        {
                            leftPart = leftPart.Substring(inputProvider.Length).Trim();
                        }
                        string url = string.Format(foundProvider, HttpUtility.UrlEncode(leftPart), HttpUtility.UrlEncode(rightPart));
                        _client.ShowMessage(commandToken,url.ToShortUrl(),answerType: AnswerBehaviourType.OpenLink);
                    }
                }
                else
                {
                    string foundProvider;
                    string address = args;
                    if (!mapUrlProviders.TryGetValue(inputProvider, out foundProvider))
                    {
                        inputProvider = _defaultProvider;
                        foundProvider = mapUrlProviders[inputProvider];
                    }
                    else
                    {
                        address = args.Substring(inputProvider.Length).Trim();    
                    }
                    string url = string.Format(foundProvider, HttpUtility.UrlEncode(address));
                    _client.ShowMessage(commandToken,url.ToShortUrl(),answerType: AnswerBehaviourType.OpenLink);
                }
            }
        }

        
    }
}
