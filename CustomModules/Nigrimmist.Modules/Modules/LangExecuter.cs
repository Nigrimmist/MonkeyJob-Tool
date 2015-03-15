using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Web;
using HelloBotCommunication;
using HelloBotModuleHelper;
using Newtonsoft.Json;

namespace Nigrimmist.Modules.Modules
{
    public class LangExecuter : ModuleBase
    {
        private class tempClass
        {
            public string Result { get; set; }
            public string Errors { get; set; }
        }

        private IBot _bot;

        public override void Init(IBot bot)
        {
            _bot = bot;
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
                    new CallCommandInfo("execute", new List<string>( ){"C#"})
                });
            }
        }

        public override string CommandDescription { get { return "Выполняет код на C#. Добавьте help для вызова справки."; } }

        public override void HandleMessage(string command, string args, Guid commandToken)
        {
            if (args.StartsWith("help"))
            {
                _bot.ShowMessage(commandToken,GetHelpText());
            }
            else
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
                var response = JsonConvert.DeserializeObject<tempClass>(hrm.Html);
                string toReturn = response.Result;

                if (string.IsNullOrEmpty(toReturn))
                {
                    toReturn = response.Errors;
                }

                if (!string.IsNullOrEmpty(toReturn))
                {
                    toReturn = toReturn.Replace(Environment.NewLine," ").Trim();
                    _bot.ShowMessage(commandToken,toReturn.Length > 200 ? toReturn.Substring(0, 50) + "..." : toReturn);
                }
                
                
            }
        }

        public string GetHelpText()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("За основу взят сайт http://rextester.com/runcode.");
            sb.AppendLine("Поддерживает многострочность. Для вывода использовать Out() метод. Например, Out(1+2);");
            sb.AppendLine("Вывод ограничен 50 символами.");
            return sb.ToString();
        }
    }
}
