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
    public class Map : ModuleCommandBase
    {
        private IClient _client;

        public override void Init(IClient client)
        {
            _client = client;
        }

        public override double Version
        {
            get { return 1.0; }
        }

        public override string Title
        {
            get { return "Карта"; }
        }

        public override string IconInBase64
        {
            get { return "iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAYAAADgdz34AAACEElEQVRIia2VEXQcURSGPygEBgoLgUChUAgEAoHCQGAhsDBQCAwEFgKBhULOCRQWFgIDC4VAoVBYCAQDgcBCYKFQKBQChUKhUNjC/8/27eS9l9OT/ufMOTPv3vffe9/93x1I4wXwAfgG3AOfgJ2M/z9hF/gJLIGvwGe//wb6/yPAncmqYK0EfqCKiqeQb6Nsm4htaNvBUwJUJqkjttK2UbD28jHCzc73Lo9XUAEDYA7c+jlDwniABlh4cwE883eqB9+BHnBq0tp7DpDSrry26tPI5CNnMgb2nenShPfBdx0EPXPgED3gGLgGTnCW4Zn2gS07LDvPF2fb4j3wKnYs6Kgn7cs04lBGAtQdn5kzjqFyJTxH5xZDWEU3e4CbxD7QqazkPE84laSzbxNIoUF3KhugJYllD/kKLgmUNCOhYVRFHVkvyFewFnyG+jBBkn3N3wtYmqy07dzZLYBfaG7Nvb9BZz/oBligZu+ZZGKSuR2PTTxEd6QNfhdw9JAiK6T/tepCxy5uM7ZFxraqoCDfrFyAa1R5NsA2+nOlkKvugvQ0vcSXsI/mTwybdkxhzMNZ1GKKesIRal4MO8THSIsT4E3CdorUxDvgMOHURxMzhgJ4iwbeHrDRsR86AQbAR9TMG6TlI1TeEF2yHporIzTzW/meIwlPg/1jcw5sX8NGQNx40wL1YYI0nvtFFkHVV8DFH8wvjA5BxAlMAAAAAElFTkSuQmCC"; }
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
                    Description = "Генерирует и открывает ссылку на карту по адресу. Умеет прокладывать маршруты. Карты goolge и yandex.",
                    CommandScheme = "карта ?<g|y> <адрес>, где g=google,y=yandex." + Environment.NewLine + "карта ?<g|y> <адрес_из> => <адрес_куда> - проложит маршрут из 'адрес_из' в 'адрес_в'.",
                    SamplesOfUsing = new List<string>()
                    {
                        "карта минск ул. Якуба Коласа 6",
                        "карта y минск ул. Якуба Коласа 6",
                        "карта ул якуба коласа 6 => ул игнатовского 4"
                    }
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

        public override void HandleMessage(string command, string args, Guid commandToken)
       {
           

            if (!string.IsNullOrEmpty(args))
            {
                string inputProvider = args.Split(' ').First();

                if (args.Contains(_fromToDelimeter))
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
                        _client.SendMessage(commandToken, CommunicationMessage.FromString(url), answerType: AnswerBehaviourType.OpenLink);
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
                    _client.SendMessage(commandToken, CommunicationMessage.FromUrl(url), answerType: AnswerBehaviourType.OpenLink);
                }
            }
        }

        
    }
}
