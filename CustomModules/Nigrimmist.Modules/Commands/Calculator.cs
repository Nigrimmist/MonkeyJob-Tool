using System;
using System.Collections.Generic;
using System.Threading;
using HelloBotCommunication;

using NCalc;

namespace Nigrimmist.Modules.Commands
{
    public class Calculator : IActionHandler
    {
        public List<CallCommandInfo> CallCommandList
        {
            get
            {
                return new List<CallCommandInfo>()
                {
                    new CallCommandInfo("калькулятор", new List<string>(){"calculator", "calc"})
                };
            }
        }

        public string CommandDescription { get { return "Умный калькулятор. Реализация NCalc библиотеки"; }  }

        public void HandleMessage(string command, string args, object clientData, Action<string, AnswerBehaviourType> sendMessageFunc)
        {
            Expression expr = new Expression(args);
            var exprAnswer = expr.Evaluate();
            string answer = string.Empty;

            answer = string.Format("Ответ равен : {0}", exprAnswer);

            sendMessageFunc(answer, AnswerBehaviourType.ShowText);
            
        }
    }
}