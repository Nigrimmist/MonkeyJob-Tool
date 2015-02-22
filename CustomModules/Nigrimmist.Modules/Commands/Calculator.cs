using System;
using System.Collections.Generic;

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
                    new CallCommandInfo("calc"),
                    new CallCommandInfo("калькулятор")
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

            sendMessageFunc(answer, AnswerBehaviourType.Text);
            
        }
    }
}