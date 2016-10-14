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
    public class IpChecker : ModuleCommandBase
    {
        
        private Random _r = new Random();

        private IClient _client;
        public override void Init(IClient client)
        {
            _client = client;
        }

        public override double Version
        {
            get { return 1.0; }
        }

        public override ReadOnlyCollection<CallCommandInfo> CallCommandList
        {
            get
            {
                return new ReadOnlyCollection<CallCommandInfo>(new List<CallCommandInfo>()
                {
                    new CallCommandInfo("IP?",new List<string>(){"мой ip","my ip","ip"})
                });
            }
        }

        public override string Title
        {
            get { return "Ip checker"; }
        }

        public override string IconInBase64
        {
            get { return "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAADCUlEQVRYhb3XXYhVVRgG4Gd0tBqwNEoqanQqhKLSQJogKyohoqKgKFKS6MYoyIJKCG8ihjAIakAQ74Lsl26iLiLoh7Ju0oKKUerCIkSLTLPSEud08a3T7Nmuvc6eafKDxdl7r/dd77v+vrUOzTEf67Ed+7GhgJ3R6MMjOIBOpdxxIsQH8HZNuINvT4R4H97NiHfLPf+3gdsL4h3swakV/Fk4H+dh7kwYOAdPY53j579bnk/YZ/E+dmA81e3Dq7gb8/6rmQcaDBzDcA17RTL2awX3i9hBA9M1MFv0LmdiB7bgM4xWOG9msLuxbLomhkWPS+viywp+XQPmd7G+phSL0u+WgvhRrKxwThcLNYf9Cyvaip+Jr3ASljY0eASrG4xva+Dsw2AbA2/gIZycjOQau6bA78eI/PS90kt8qdhWg1hVI/9kIlFtbNGRm/CbyaM2juUl0ssJvAyLsUms9Ltwikg4Y2JOL2ph4tHU3jO4VIzK1ibwXBxKhO24oAG3IvXkY5G+S3GG6Pns9P6ByBdzcuAbTB7yPwoNv5gw9/YwALvEDoHHEu/6HPDhmoGjmlPqQnFH2CvuDaUYw9r0fG1qe20OOOL4Vbu50PCDJp8Pubg1YfaLBT6U3p/KgUdr4m/h3ELj/fhQbNlczBMj1G1vLy5Lzy/kCBtS5RhuKQi3jfqa6uD19PtkjnB/qry58m0I1+k9z7lYkjHQTU735QjLU+VIer9NLMQOvjNxPrSN1RkD3XJ5jtCHH/GNSMP1g2U37sSClgZyx3MHPyjkj00JtLXgfk8L8Vkidef4owWeRThcEB/HmhYGhhv4f4o7ZDE2Ngi/gxtbiM8ycabUy0iB928M4Isa8fs2RFyITxrEPxcHWqtYjJ9NHoGze3AGxWW0I/L/c2LfHzOFy0g1LhH/iDr4G1f2wM8ROWMoib1UMXPxVMW7MV/ckLqn42ZxoPQ3GFgp7pFHEuc1nDZd8WpcjU9NTMkhfIT3xBn/NQ5W6rfhqpkQrscSPCGuZrvElj2Mnenb42Ihto5/AMrxTxBm8EQgAAAAAElFTkSuQmCC"; }
        }

        public override DescriptionInfo ModuleDescription
        {
            get
            {
                return new DescriptionInfo()
                {
                    Description = "Выводит ваш ip адрес. Используется сервис http://whatismyipaddress.com/"
                };
            }
        }

        public override void HandleMessage(string command, string args, Guid commandToken)
        {
            HtmlReaderManager hrm = new HtmlReaderManager();
            hrm.Encoding = Encoding.GetEncoding(1251);
            hrm.Get("http://whatismyipaddress.com/");
            string html = hrm.Html;
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            var ip = htmlDoc.DocumentNode.SelectSingleNode(@"//./div[@id='section_left']/div[2]/a").InnerText.Trim();
            _client.SendMessage(commandToken, CommunicationMessage.FromString(ip));
        }
    }
}
