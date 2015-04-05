using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using HelloBotCommunication;
using HelloBotCommunication.Interfaces;
using HelloBotCore.Helpers;

namespace HelloBotCore.Entities
{
    public class ModuleCommandInfoBase
    {
        public Guid Id { get; set; }
        public string CommandDescription { get; set; }
        public double Version { get; set; }
        public string ModuleName { get; set; }
        public string DisplayName { get; set; }
        public Image Icon { get; set; }
        public Color? BodyBackgroundColor { get; set; }
        public Color? HeaderBackgroundColor { get; set; }

        public ModuleCommandInfoBase()
        {
            Id = Guid.NewGuid();
        }

        public void Init(string dllName, ModuleBase handlerModuleBase, IModuleClientHandler moduleClientHandler)
        {
            CommandDescription = handlerModuleBase.ModuleDescription;
            Version = handlerModuleBase.ModuleVersion;
            DisplayName = handlerModuleBase.ModuleTitle;
            var handType = handlerModuleBase.GetType();
            ModuleName = dllName + "." + handType.Name;

            if (!string.IsNullOrEmpty(handlerModuleBase.IconInBase64))
            {
                Icon = ImageHelper.ResizeImage(ImageHelper.GetFromBase64(handlerModuleBase.IconInBase64), 26, 26);
            }

            BodyBackgroundColor = handlerModuleBase.BodyBackgroundColor;
            HeaderBackgroundColor = handlerModuleBase.HeaderBackGroundColor;

            IClient client = new ModuleToClientAdapter(moduleClientHandler, this);
            handlerModuleBase.Init(client);
        }
    }


}
