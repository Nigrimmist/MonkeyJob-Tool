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
    public abstract class ComponentInfoBase
    {
        public bool IsEnabled { get; set; }
        public Guid Id { get; set; }
        public DescriptionInfo CommandDescription { get; set; }
        public double Version { get; set; }
        public double ActualSettingsModuleVersion { get; set; }
        public string SystemName { get; set; }
        public string ProvidedTitle { get; set; }
        public Image Icon { get; set; }
        public AuthorInfo Author { get; set; }
        public Type SettingsType { get; set; }
        public ModuleLogStorageInfo Trace { get; set; }
        private string _settingsFolderAbsolutePath { get; set; }
        private string _logsFolderAbsolutePath { get; set; }


        protected ComponentInfoBase(string settingsFolderAbsolutePath, string logsFolderAbsolutePath)
        {
            Id = Guid.NewGuid();
            _settingsFolderAbsolutePath = settingsFolderAbsolutePath;
            _logsFolderAbsolutePath = logsFolderAbsolutePath;
        }

        
        public T GetSettings<T>() where T : class
        {
           string fullPath = GetSettingFileFullPath();
           if (!File.Exists(fullPath)) return null;
           //todo: refactoring required (cache)
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
            return string.IsNullOrEmpty(this.ProvidedTitle) ? this.SystemName : this.ProvidedTitle;
        }

        public void Init(string dllName, ComponentBase componentBase, AuthorInfo author)
        {
            Version = componentBase.Version;
            ActualSettingsModuleVersion = componentBase.ActualSettingsVersion;
            ProvidedTitle = componentBase.Title;
            var handType = componentBase.GetType();
            SystemName = dllName + "." + handType.Name;

            if (!string.IsNullOrEmpty(componentBase.IconInBase64))
            {
                Icon = ImageHelper.ResizeImage(ImageHelper.GetFromBase64(componentBase.IconInBase64), 26, 26);
            }
            
            Author = author;

            Trace = new ModuleLogStorageInfo(30, _logsFolderAbsolutePath, SystemName);
            Trace.Load();
        }
        
        public virtual string GetDescriptionText()
        {
            return "";
        }

        public virtual string ToString(bool includingAuthorInfo = true)
        {
            string toReturn = string.Empty;
            if (Author != null && includingAuthorInfo)
            {
                if (!string.IsNullOrEmpty(Author.Name))
                    toReturn += "Автор : " + Author.Name + Environment.NewLine + Environment.NewLine;
                if (!string.IsNullOrEmpty(Author.ContactEmail))
                    toReturn += "Email для связи : " + Author.ContactEmail + Environment.NewLine + Environment.NewLine;
            }
            return toReturn;
        }

        public abstract string GetTypeDescription();

        public virtual string GetSettingFileFullPath()
        {
            return _settingsFolderAbsolutePath + "/" + SystemName + ".json";
        }
    }


}
