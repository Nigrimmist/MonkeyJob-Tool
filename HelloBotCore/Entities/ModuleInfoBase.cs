using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using HelloBotCommunication;
using HelloBotCommunication.Interfaces;
using HelloBotCore.Helpers;
using HelloBotCore.Manager;
using Newtonsoft.Json;
using SharedHelper;

namespace HelloBotCore.Entities
{
    public abstract class ModuleInfoBase : ComponentInfoBase
    {
        private List<ComponentInfoBase> _instances;

        protected ModuleInfoBase(StorageManager storageManager)
: base(storageManager)
        {

        }

        public List<ComponentInfoBase> Instances { get => _instances; set => _instances = value; }
        public Func<int?, ModuleInfoBase> CreateNewInstanceFunc { get; set; }
        public override string ToString(bool includingAuthorInfo = true)
        {
            string toReturn = string.Empty;
            if (!string.IsNullOrEmpty(CommandDescription.Description))
                toReturn += CommandDescription.Description + Environment.NewLine + Environment.NewLine;
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

        public abstract void Init(string dllName, ComponentBase componentBase, IModuleClientHandler moduleClientHandler, AuthorInfo author);        

        public abstract string GetTypeDescription();

        public new MainComponentInstanceSettings GetSettings()
        {
            return GetSettings<MainComponentInstanceSettings>();
        }
    }


}
