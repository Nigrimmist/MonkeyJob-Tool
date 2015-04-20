﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using HelloBotCommunication;
using HelloBotCommunication.Interfaces;
using HelloBotCore.Helpers;

namespace HelloBotCore.Entities
{
    public abstract class ModuleCommandInfoBase
    {
        public bool IsEnabled { get; set; }
        public Guid Id { get; set; }
        public DescriptionInfo CommandDescription { get; set; }
        public double Version { get; set; }
        public string ModuleSystemName { get; set; }
        public string ProvidedTitle { get; set; }
        public Image Icon { get; set; }
        public Color? BodyBackgroundColor { get; set; }
        public Color? HeaderBackgroundColor { get; set; }
        public AuthorInfo Author { get; set; }


        public ModuleCommandInfoBase()
        {
            Id = Guid.NewGuid();
        }

        public string GetModuleName()
        {
            return string.IsNullOrEmpty(this.ProvidedTitle) ? this.ModuleSystemName : this.ProvidedTitle;
        }

        public void Init(string dllName, ModuleBase handlerModuleBase, IModuleClientHandler moduleClientHandler, AuthorInfo author)
        {
            Version = handlerModuleBase.ModuleVersion;
            ProvidedTitle = handlerModuleBase.ModuleTitle;
            var handType = handlerModuleBase.GetType();
            ModuleSystemName = dllName + "." + handType.Name;

            if (!string.IsNullOrEmpty(handlerModuleBase.IconInBase64))
            {
                Icon = ImageHelper.ResizeImage(ImageHelper.GetFromBase64(handlerModuleBase.IconInBase64), 26, 26);
            }

            BodyBackgroundColor = handlerModuleBase.BodyBackgroundColor;
            HeaderBackgroundColor = handlerModuleBase.HeaderBackGroundColor;
            Author = author;
            IClient client = new ModuleToClientAdapter(moduleClientHandler, this);
            handlerModuleBase.Init(client);
        }

        public abstract ModuleType ModuleType { get; }
    }


}
