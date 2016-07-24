using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using HelloBotCommunication;
using HelloBotCommunication.Interfaces;
using HelloBotCore.Helpers;
using Newtonsoft.Json;
using SharedHelper;

namespace HelloBotCore.Entities
{
    public abstract class ModuleInfoBase : ComponentInfoBase
    {
        protected ModuleInfoBase(string settingsFolderAbsolutePath, string logsFolderAbsolutePath)
            : base(settingsFolderAbsolutePath, logsFolderAbsolutePath)
        {
            
        }
        
        public override string ToString(bool includingAuthorInfo=true)
        {
            string toReturn = string.Empty;
            if (!string.IsNullOrEmpty(CommandDescription.Description))
                toReturn +=  CommandDescription.Description + Environment.NewLine + Environment.NewLine;
            if (!string.IsNullOrEmpty(CommandDescription.CommandScheme))
                toReturn += "Схема команды : " + Environment.NewLine + CommandDescription.CommandScheme + Environment.NewLine + Environment.NewLine;
            if (CommandDescription.SamplesOfUsing != null && CommandDescription.SamplesOfUsing.Any())
                toReturn += "Примеры использования : " + Environment.NewLine + string.Join(Environment.NewLine, CommandDescription.SamplesOfUsing.ToArray()) + Environment.NewLine + Environment.NewLine;
            
            string moduleDescription = GetDescriptionText();

            if (!string.IsNullOrEmpty(moduleDescription))
            {
                toReturn += moduleDescription + Environment.NewLine + Environment.NewLine;
            }

            
            return toReturn + base.ToString(includingAuthorInfo);
        }

        public override string GetTypeDescription()
        {
            string toReturn;
            switch (ModuleType)
            {
                case ModuleType.Handler:
                    toReturn = "Команда";
                    break;
                case ModuleType.Event:
                    toReturn = "Событийный";
                    break;
                case ModuleType.Tray:
                    toReturn = "Трей";
                    break;
                case ModuleType.IntegrationClient:
                    toReturn = "Клиент";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return toReturn;
        }
    }


}
