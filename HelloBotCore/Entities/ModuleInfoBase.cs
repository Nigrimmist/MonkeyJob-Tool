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

        protected ModuleInfoBase(StorageManager storageManager)
: base(storageManager)
        {
        }

        public override bool IsEnabled
        {
            get
            {
                return base.IsEnabled;
            }

            set
            {
                base.IsEnabled = value;
                Instances.ForEach(x => x.IsEnabled = value);
            }
        }

        public abstract bool IsEnabledByDefault { get; }
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

        public new MainComponentInstanceSettings GetSettings()
        {
            return GetSettings<MainComponentInstanceSettings>();
        }

        public override void RemoveInstance(string systemName)
        {
            var found = Instances.SingleOrDefault(x=>x.SystemName==systemName);
            var settings = GetSettings();
            settings.Instances.Remove(found.InstanceId.Value);
            base.RemoveInstance(systemName);
            SaveSettings(settings);
        }

        public override ComponentInfoBase AddInstance(string systemName)
        {
            var addedInstance = base.AddInstance(systemName);
            var settings = GetSettings();
            settings.Instances.Add(addedInstance.InstanceId.Value);
            SaveSettings(settings);
            return addedInstance;
        }
    }


}
