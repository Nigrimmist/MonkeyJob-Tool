using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using HelloBotCommunication;
using HelloBotCommunication.Interfaces;
using NCalc;

namespace Nigrimmist.Modules.Modules
{
    public class Calculator : ModuleCommandBase
    {
        private IClient _client;

        public override void Init(IClient client)
        {
            _client = client;
            _client.OnCommandArgsChanged += _client_OnCommandArgsChanged;
        }
        
        void _client_OnCommandArgsChanged(string command, string args)
        {
            _client.ShowSuggestList(new List<AutoSuggestItem>() { new AutoSuggestItem() { DisplayedKey = "arg1", Value = args }, new AutoSuggestItem() { DisplayedKey = "arg2", Value = args } });
        }

        public override double ModuleVersion
        {
            get { return 1.0; }
        }

        public override string ModuleTitle
        {
            get { return "Калькулятор"; }
        }

        public override string IconInBase64
        {
            get { return "iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAYAAADgdz34AAABsElEQVRIibWVIU8DQRCFP4FAIBAIEgSIJjSEVJCKioqKJlU0NKk4WYFAIhAIkpOVSETF/YCTFRWIExUViIrKihMnTiAQFZVF7Nv0aLjSPWCTyc3bmdk3uzO7B19HE0iAVUFJgC5bxgyob3P4YVRFsvedsQaMpU+A2FEmio3ykmzIiAJ2HTWglImJtNafEfhA7z8JysCxK0GIew1CF4KScFW4LnwkabAu5KVwyYVgLn0JdIAUmAJDyVRzbWAh37kLQSycAh4wAu5lj6SPZEvl61SDZ+lD4Bx4FX6UWKKKfCLF7ExwCwRAHzgEXoRvJAEwwHRPX/jWhSDG9PY75hjGWshm3tecJ58eBe6BD3xokamytASB5jz5+K4EnoIeMEf0JFyT+Jo7AO6EPRcCe9FmwBWmBe2ubLZzTJHtw+h00eLM18McibXbBQLZ4o2YnQhmClgCLcxlSjEtGUpfANdFCS6UnXVsC58CJ9I7wNlvdhApyxb5T0W3KEGs7BJlG+pra5CdSzZ2kkuQ/WVmn4pKZuFtT8UgQ5D7X59hnt+io4zZyX6eQxPT16uC8sb6wgHwCY3v375DPVjNAAAAAElFTkSuQmCC"; }
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

        
        public override DescriptionInfo ModuleDescription
        {
            get
            {
                return new DescriptionInfo()
                {
                    Description = "Умный калькулятор. Реализация NCalc библиотеки",
                    SamplesOfUsing = new List<string>()
                    {
                        "калькулятор 1+2-3",
                        "calc 3*(2+9)",
                        "calc Sqrt(4)",
                        "calc Tan(0)",
                    },
                    CommandScheme = "calc <мат. выражение>"
                };
            }
        }

        
        public override void HandleMessage(string command, string args, Guid commandToken)
        {
            if (!string.IsNullOrEmpty(args.Trim()))
            {
                Expression expr = new Expression(args);
                var exprAnswer = expr.Evaluate();
                string answer = string.Empty;

                answer = string.Format("{0}", exprAnswer);

                _client.ShowMessage(commandToken, answer);
            }
            else
            {
                _client.ShowMessage(commandToken, "Введите выражение",messageType:MessageType.Error);
            }
        }
    }
}