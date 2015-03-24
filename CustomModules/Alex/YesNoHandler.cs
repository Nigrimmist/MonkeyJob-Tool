using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using HelloBotCommunication;
using HelloBotCommunication.Interfaces;


namespace SmartAssHandlerLib
{
    public class YesNoHandlerHandlerBase : ModuleHandlerBase
    {
        private readonly RandomHelper _decisionMaker = new RandomHelper();

        private SpecialAnswersProvider _specialAnswersProvider;
        private GeneralAnswersProvider _generalAnswersProvider;
        private EmptyAnswerProvider _emptyAnswerProvider;

        private const int CHANCE_OF_SPECIAL_ANSWER = 30;
        private IClient _client;

        public override void Init(IClient client)
        {
            _client = client;
        }
        public YesNoHandlerHandlerBase()
        {
            _specialAnswersProvider = new SpecialAnswersProvider(_decisionMaker);
            _generalAnswersProvider = new GeneralAnswersProvider(_decisionMaker);
            _emptyAnswerProvider = new EmptyAnswerProvider(_decisionMaker);
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
                    new CallCommandInfo("бот", new List<string>(){"бот,"}),
                    
                });
            }
        }
        public override string ModuleDescription
        {
            get { return "Лаконичный ответ на простой вопрос."; }
        }

        public override void HandleMessage(string command, string args, Guid commandToken)
        {
            var answer = string.Empty;

            if (!string.IsNullOrEmpty(args) && args.Contains("?") && args.Any(x => x != '?') && args.Length > 1)
            {
                var giveSpecialAnswer = _decisionMaker.PercentageDecision(CHANCE_OF_SPECIAL_ANSWER);
                answer = giveSpecialAnswer ? _specialAnswersProvider.Get() : _generalAnswersProvider.Get();
            }
            else
            {
                answer = _emptyAnswerProvider.Get();
            }

            _client.ShowMessage(commandToken,answer);
        }

        private class RandomHelper
        {
            private readonly Random _randomEngine = new Random();

            public bool BinaryDecision
            {
                get { return _randomEngine.Next(0, 2) > 0; }
            }

            public bool PercentageDecision(int percents)
            {
                return _randomEngine.Next(1, 101) < percents;
            }

            public string PeekFromList(List<string> items)
            {
                return items[_randomEngine.Next(0, items.Count)];
            }

            public string ChooseSomething(string one,string another)
            {
                return BinaryDecision ? one : another;
            }
        }

        private class GeneralAnswersProvider
        {
            private readonly List<string> YesAnswers = new List<string>()
            {
                "ДА.",
                "Yes.",
                "Конечно.",
                "Несомненно.",
                "Именно так.",
                "Сто пудов.",
                "Это так.",
                "Не уверен на 100%, но кажется, что да.",
                "да, ДА, О ДАААА!!!"
            };

            private readonly List<string> NoAnswers = new List<string>()
            {
                "НЕТ.",
                "NO.",
                "Ни в коем случае.",
                "Нет, нет и ещё раз нет.",
                "Никогда.",
                "нееее, нуууу нет.",
                "омг, нет!",
                "Не уверен на 100%, но кажется, что нет.",
                "NOOOOOOOOOOOOOO!"
            };

            private RandomHelper _randomEngine;

            public GeneralAnswersProvider(RandomHelper randomEngine)
            {
                _randomEngine = randomEngine;
            }

            public string Get()
            {
                return _randomEngine.BinaryDecision
                    ? _randomEngine.PeekFromList(YesAnswers)
                    : _randomEngine.PeekFromList(NoAnswers);
            }
        }

        private class SpecialAnswersProvider
        {
            private List<string> _answerTemplates;
            private RandomHelper _randomEngine;
            private Dictionary<string, List<string>> _answersLookup;

            private const string PositiveAnswerBase = "ДА";
            private const string NegativeAnswerBase = "Нет";

            public SpecialAnswersProvider(RandomHelper randomEngine)
            {
                _randomEngine = randomEngine;

                
            }

            public string Get()
            {
                var answersSet =  AnonymousSpecial;
                var template = _randomEngine.PeekFromList(answersSet);

                return string.Format(template, _randomEngine.ChooseSomething(PositiveAnswerBase, NegativeAnswerBase));
            }

            

            private readonly List<string> AnonymousSpecial = new List<string>()
            {
                "Ты кто? Чего ты от меня хочешь?",
                "Я тебя не знаю... Не скажу",
                "Хороший вопрос! Но я пока не могу на него ответить.",
                "Надо подумать... Кстати, ты когда-нибудь бываешь иногда?",
                "Это слишком философский вопрос...",
                "Я думаю, что бы там ни было, мой ответ будет всегда понятен каждому. Ведь если бы это было не так, тогда бы получилось ерунда, не так ли?",
                "А почему ты об этом спрашиваешь?",
                "Не задавай риторических вопросов!",
                "А сам-то как думаешь?",
                "Да не сойти мне с этого места!",
                "Чтоб я провалился!",
                "С одной стороны, конечно да, но... Если посмотреть с противоположной стороны, то выходит, что как бы и... Но в то же время!.. И тогда выходит, что не совсем, НО! НО!... Это же получается не так ведь, понимаешь меня?",
                "Меня учили не говорить с незнакомцами. Вдруг ты ботофил?"
            };
        }

        private class EmptyAnswerProvider
        {
            private RandomHelper _randomEngine;

            private readonly List<string> _answers = new List<string>()
            {
                "Что?",
                "Ну?",
                "Во что бы то ни стало!",
                "?",
                "Как два байта переслать.",
                "WHAT?",
                "...",
                "ТАГИИИИИИИИИИИЛ!!!!!!",
                "??",
                "...А за ней шли... Все её тридцать три кошки...",
                "???",
                "wut?",
                "ась?",
                "Щито?",
                "мммммм?"
            };

            public EmptyAnswerProvider(RandomHelper randomEngine)
            {
                _randomEngine = randomEngine;
            }

            public string Get()
            {
                return string.Format(_randomEngine.PeekFromList(_answers));
            }
        }
    }
}
