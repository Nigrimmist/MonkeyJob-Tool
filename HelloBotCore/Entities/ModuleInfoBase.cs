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
    public abstract class ModuleInfoBase
    {
        public bool IsEnabled { get; set; }
        public Guid Id { get; set; }
        public DescriptionInfo CommandDescription { get; set; }
        public double Version { get; set; }
        public double ActualSettingsModuleVersion { get; set; }
        public string ModuleSystemName { get; set; }
        public string ProvidedTitle { get; set; }
        public Image Icon { get; set; }
        public AuthorInfo Author { get; set; }
        public Type ModuleSettingsType { get; set; }
        public abstract ModuleType ModuleType { get; }
        public ModuleLogStorageInfo Trace { get; set; }
        private string _settingsFolderAbsolutePath { get; set; }
        private string _logsFolderAbsolutePath { get; set; }

        public ModuleInfoBase(string settingsFolderAbsolutePath, string logsFolderAbsolutePath)
        {
            Id = Guid.NewGuid();
            _settingsFolderAbsolutePath = settingsFolderAbsolutePath;
            _logsFolderAbsolutePath = logsFolderAbsolutePath;
        }

        public T GetSettings<T>() where T : class
        {
            string fullPath = GetSettingFileFullPath();
           if (!File.Exists(fullPath)) return null;
           string data = File.ReadAllText(fullPath);
           var settings = JsonConvert.DeserializeObject<ModuleSettings<T>>(data);
           return settings.ModuleData;
        }

        public void SaveSettings<T>(T serializableSettingObject) where T : class
        {
            var settings = new ModuleSettings<T>(Version, ActualSettingsModuleVersion, serializableSettingObject);
            var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
            File.WriteAllText(GetSettingFileFullPath(), json);
        }

        public string GetModuleName()
        {
            return string.IsNullOrEmpty(this.ProvidedTitle) ? this.ModuleSystemName : this.ProvidedTitle;
        }

        public void Init(string dllName, ModuleBase handlerModuleBase, AuthorInfo author)
        {
            Version = handlerModuleBase.ModuleVersion;
            ActualSettingsModuleVersion = handlerModuleBase.ActualSettingsModuleVersion;
            ProvidedTitle = handlerModuleBase.ModuleTitle;
            var handType = handlerModuleBase.GetType();
            ModuleSystemName = dllName + "." + handType.Name;

            if (!string.IsNullOrEmpty(handlerModuleBase.IconInBase64))
            {
                Icon = ImageHelper.ResizeImage(ImageHelper.GetFromBase64(handlerModuleBase.IconInBase64), 26, 26);
            }
            
            Author = author;

            Trace = new ModuleLogStorageInfo(30, _logsFolderAbsolutePath, ModuleSystemName);
            Trace.Load();
        }

        

        public virtual string GetDescriptionText()
        {
            return "";
        }

        public override string ToString()
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
            
            
            if (Author != null)
            {
                if (!string.IsNullOrEmpty(Author.Name))
                    toReturn += "Автор : " + Author.Name + Environment.NewLine + Environment.NewLine;
                if (!string.IsNullOrEmpty(Author.ContactEmail))
                    toReturn += "Email для связи : " + Author.ContactEmail + Environment.NewLine + Environment.NewLine;
            }
            return toReturn;
        }

        public string GetSettingFileFullPath()
        {
            return _settingsFolderAbsolutePath + "/" + ModuleSystemName + ".json";
        }

        

        public string GetTypeDescription()
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
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return toReturn;
        }
    }


}
