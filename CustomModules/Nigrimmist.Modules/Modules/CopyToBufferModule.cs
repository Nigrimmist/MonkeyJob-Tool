using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using HelloBotCommunication;
using HelloBotCommunication.Attributes.SettingAttributes;
using HelloBotCommunication.Interfaces;

namespace Nigrimmist.Modules.Modules
{
    public class CopyToBufferModule: ModuleCommandBase
    {
        private IClient _client;
        private List<BufferCommandKeyValue> _commandUrls;

        private ReadOnlyCollection<CallCommandInfo> _callCommandList;
        public override ReadOnlyCollection<CallCommandInfo> CallCommandList{get { return _callCommandList; }}
        
        public override DescriptionInfo ModuleDescription
        {
            get
            {
                return new DescriptionInfo()
                {
                    Description = @"Копирует заданные значения в буфер обмена при вызове соответствующей команды. Можете вбить сюда, допустим, свой email, фамилию, имя и другие часто используемые строки, которые зачастую лень набирать каждый раз. Любые изменения настроек требуют перезапуска программы."
                };
            }
        }
        public override double ModuleVersion { get { return 1.0; } }

        public override string ModuleTitle
        {
            get { return "Буфер обмена"; }
        }

        public override void Init(IClient client)
        {
            _client = client;
            var existSettings = _client.GetSettings<CopyToBufferSettings>();
            if (existSettings == null)
            {
                //let's save default settings
                existSettings = new CopyToBufferSettings()
                {
                    Items = new List<BufferCommandKeyValue>()
                    {
                        new BufferCommandKeyValue() {Command = "Название программы", Text = "MonkeyJob Tool"}
                    }
                };
                _client.SaveSettings(existSettings);
            }
            _commandUrls = existSettings.Items;
            _callCommandList = new ReadOnlyCollection<CallCommandInfo>(existSettings.Items.Select(x=> new CallCommandInfo(x.Command)).ToList());
        }

        public override string IconInBase64
        {
            get { return "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAABn0lEQVRYhe2XLVcCQRSGn2AgEIgGAtFAJBg5SjAYiPwAgz/AYCMaCP4AgsFgIBgMBsMeIRjx8/jtRoLBYCAQMMysZxnWmTsDHIrvOW+Zc+/sc+7M3rsLYSoAp8AQGAs8ANYCn5Wppt64Axw53FkERB3YFsaWWGAlJCoweRw9YNMMygENVGln8S6w6gAYA11gIwnIA/2MoFB/ARUDoJsRdwlUAfb1wglQ04uhTi5nz6hCTa/FKY+AC4AznVRgPrrSm7v0AUSg3ud5AkR6P5febQB7TJYrRpVQAikFeHMBRIbP5wzwagOYRVKAl2UDPC8b4MkGsI5fB0z3dynAow0gWZO6FQDwYANYQU0xqdOSAtzbAIr4teB8AMCdDcD3CA4DAG5tAL4VSOdKAW5sACA//6KRJwW4tgEc43cEzQCAvg2ghl8fKHsClIBv1AfrwjthzN+V2/IFKKM6WGx4xwLQYvpTvZ083BeghBrLkeG6BcCpZQ2jf4BftXVCxRUoUA71+/Xpk1TVAEOmL5evB0yPZ5EaqNfLp/tleQAcoMa5SD+yqzWtqDqahgAAAABJRU5ErkJggg=="; }
        }

        public override void HandleMessage(string command, string args, Guid commandToken)
        {
            var foundCommand = _commandUrls.SingleOrDefault(x => x.Command == command);
            if (foundCommand!=null)
            {
                _client.ShowMessage(commandToken, string.Format(foundCommand.Text, args), answerType: AnswerBehaviourType.CopyToClipBoard);
            }
        }
    }

    [ModuleSettingsFor(typeof(CopyToBufferModule))]
    public class CopyToBufferSettings
    {
        [SettingsNameField("Списки буфера")]
        public List<BufferCommandKeyValue> Items { get; set; }

        public CopyToBufferSettings()
        {
            Items = new List<BufferCommandKeyValue>();
        }
    }

    public class BufferCommandKeyValue
    {
        [SettingsNameField("Команда")]
        public string Command { get; set; }

        [SettingsNameField("Текст")]
        public string Text { get; set; }
    }
}
