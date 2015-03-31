using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using HelloBotCommunication;
using HelloBotCommunication.Interfaces;
using NCalc;

namespace Nigrimmist.Modules.Modules
{
    public class Calculator : ModuleHandlerBase
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
                    new CallCommandInfo("калькулятор", new List<string>(){"calculator", "calc"})
                });
            }
        }

        public override string ModuleDescription { get { return "Умный калькулятор. Реализация NCalc библиотеки"; }  }

        public override void HandleMessage(string command, string args, Guid commandToken)
        {
            Expression expr = new Expression(args);
            var exprAnswer = expr.Evaluate();
            string answer = string.Empty;

            answer = string.Format("Ответ равен : {0}", exprAnswer);

            _client.ShowMessage(commandToken, answer).OnClick(() =>
            {
                _client.ShowMessage(commandToken, "Не надо сюда кликать");
            }).OnClosed(
                () =>
                {
                    _client.ShowMessage(commandToken, "Notified event fired");
                }
                );
        }
    }
}