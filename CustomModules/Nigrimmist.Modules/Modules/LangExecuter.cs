using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Web;
using HelloBotCommunication;
using HelloBotCommunication.Interfaces;
using HelloBotModuleHelper;
using Newtonsoft.Json;

namespace Nigrimmist.Modules.Modules
{
    public class LangExecuter : ModuleCommandBase
    {
        private class RexTesterResponse
        {
            public string Result { get; set; }
            public string Errors { get; set; }
        }

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
                    new CallCommandInfo("execute", new List<string>() {"C#"})
                });
            }
        }

        public override string Title
        {
            get { return "C# компилятор"; }
        }

        public override DescriptionInfo ModuleDescription
        {
            get
            {
                return new DescriptionInfo()
                {
                    Description = "Исполняет и выводит C# код. За основу взят сайт http://rextester.com/runcode. Для вывода использовать Out() метод",
                    SamplesOfUsing = new List<string>()
                    {
                        "Out(1+2);",
                        @"string str = ""hello world"";string str2=""!""; Out(str+str2);"
                    },
                    CommandScheme = "C# <C# код>"
                };
            }
        }

        public override void HandleMessage(string command, string args, Guid commandToken)
        {

            string templateCode = @"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Rextester
{{
    public class Program
    {{
        public static void Main(string[] args)
        {{
            {0}
        }}
        public static void Out(object obj){{
            Console.WriteLine(obj);
        }}
    }}
}}";

            HtmlReaderManager hrm = new HtmlReaderManager();

            hrm.Post("http://rextester.com/rundotnet/Run", string.Format("LanguageChoiceWrapper=1&EditorChoiceWrapper=1&Program={0}&Input=&ShowWarnings=false&Title=&SavedOutput=&WholeError=&WholeWarning=&StatsToSave=&CodeGuid=&IsInEditMode=False&IsLive=False"
                , HttpUtility.UrlEncode(string.Format(templateCode, args))));
            var response = JsonConvert.DeserializeObject<RexTesterResponse>(hrm.Html);
            string toReturn = response.Result;

            if (string.IsNullOrEmpty(toReturn))
            {
                toReturn = response.Errors;
            }

            if (!string.IsNullOrEmpty(toReturn))
            {
                toReturn = toReturn.Replace(Environment.NewLine, " ").Trim();
                _client.ShowMessage(commandToken, toReturn.Length > 200 ? toReturn.Substring(0, 50) + "..." : toReturn);
            }
            else
            {
                _client.ShowMessage(commandToken, "Что-то скомпилилось не так, попробуйте по другому");
            }


        }



    }
}
