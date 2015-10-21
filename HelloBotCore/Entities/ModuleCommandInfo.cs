using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using HelloBotCommunication;
using HelloBotCommunication.Interfaces;

namespace HelloBotCore.Entities
{
    public class ModuleCommandInfo : ModuleInfoBase
    {
        public List<CallCommandInfo> CallCommandList { get; set; }
        public delegate void HandleMessageFunc(string command, string args, Guid token);
        public HandleMessageFunc HandleMessage { get; set; }
        public delegate void OnEventFireDelegate(Guid eventToken);
        public List<string> OriginalAliases { get; set; }
        public Color? BodyBackgroundColor { get; set; }
        public Color? HeaderBackgroundColor { get; set; }

        public ModuleCommandInfo(string settingsFolderAbsolutePath, string logsFolderAbsolutePath) : base(settingsFolderAbsolutePath, logsFolderAbsolutePath)
        {
            OriginalAliases = new List<string>();
        }

        public void Init(string dllName,ModuleCommandBase commandModuleBase, IModuleClientHandler moduleClientHandler, AuthorInfo author)
        {
            HandleMessage = commandModuleBase.HandleMessage;
            base.Init(dllName, commandModuleBase, author);
            BodyBackgroundColor = commandModuleBase.BodyBackgroundColor;
            HeaderBackgroundColor = commandModuleBase.HeaderBackGroundColor;
            IClient client = new ModuleToClientAdapter(moduleClientHandler, this);
            commandModuleBase.Init(client);
            CallCommandList = commandModuleBase.CallCommandList.ToList();
            CommandDescription = commandModuleBase.ModuleDescription;
            OriginalAliases = commandModuleBase.CallCommandList.Where(x=>!string.IsNullOrEmpty(x.Command)).Select(x => x.Command).ToList();
            OriginalAliases.AddRange(commandModuleBase.CallCommandList.Where(x => !string.IsNullOrEmpty(x.Command)).SelectMany(x => x.Aliases).ToList());
            
        }

        public override ModuleType ModuleType
        {
            get { return ModuleType.Handler; }
        }

        public override string GetDescriptionText()
        {
            return "Варианты команды : "+Environment.NewLine+string.Join(Environment.NewLine,OriginalAliases.ToArray());
        }
    }
}
