using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using HelloBotCommunication;
using HelloBotCommunication.Interfaces;

namespace Nigrimmist.Modules.Modules
{
    public class Or : ModuleHandlerBase
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
                    new CallCommandInfo("Выбери", new List<string>(){"или"})
                });
            }
        }

        public override string ModuleTitle
        {
            get { return "Или?"; }
        }

        public override DescriptionInfo ModuleDescription
        {
            get
            {
                return new DescriptionInfo()
                {
                    Description = @"Выбирает между чем-то. Использует ""Или"" в качестве разделителя",
                    CommandScheme = "выбери <аргумент1> или <аргумент2> или <аргумент_n>",
                    SamplesOfUsing = new List<string>()
                    {
                        "выбери да или нет",
                        "выбери лукашенко или лукашенко",
                        "выбери 1 или 2 или 3 или 4"
                    }
                };
            }
        }

        
        private Random _r = new Random();
        private const int ChanceOfSpecialAnswer = 30;

        private List<string> _customMessages = new List<string>()
        {
            "Думаю {0}",
            "Определенно {0}",
            "{0}. К гадалке не ходи",
            "Не нужно быть семи пядей во лбу, чтобы понять, что {0} тут единственно верный вариант",
            "Возможно {0}",
            "Я выбираю {0}",
            "Пусть будет {0}",
            "Эники бэники... {0}!"
        };

        public override void HandleMessage(string command, string args, Guid commandToken)
        {
            var variants = Regex.Split(args, "или", RegexOptions.IgnoreCase);
            string answer = "А что, есть выбор?";
            if (variants.Any())
            {
                answer = variants[_r.Next(0, variants.Count())];
            }
            if (_r.Next(1, 101) < ChanceOfSpecialAnswer)
                answer = string.Format(_customMessages[_r.Next(0, _customMessages.Count)], answer);
            _client.ShowMessage(commandToken,answer);
        }


    }
}
