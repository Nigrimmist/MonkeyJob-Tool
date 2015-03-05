using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

using HelloBotCommunication;
using HelloBotModuleHelper;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Nigrimmist.Modules.Helpers;

namespace Nigrimmist.Modules.Commands
{
    /// <summary>
    /// Quote from http://online-generators.ru/
    /// </summary>
    public class Quote : IActionHandler
    {
        
        public ReadOnlyCollection<CallCommandInfo> CallCommandList
        {
            get
            {
                return new ReadOnlyCollection<CallCommandInfo>(new List<CallCommandInfo>()
                {
                    new CallCommandInfo("цитата", new List<string>(){"quote"})
                });
            }
        }

        public string CommandDescription { get { return @"Случайная цитата c http://online-generators.ru"; } }

        public void HandleMessage(string command, string args, object clientData, Action<string, AnswerBehaviourType> sendMessageFunc)
        {

            HtmlReaderManager hrm = new HtmlReaderManager();
            
            hrm.Post("http://online-generators.ru/ajax.php", "processor=quotes");
            var answerParts = hrm.Html.Split(new string[]{"##"},StringSplitOptions.RemoveEmptyEntries);
            string quote = answerParts[0];
            string author = answerParts[1];
            sendMessageFunc(string.Format("{0} ©{1}", quote, author), AnswerBehaviourType.ShowText);
        }
    }
}
