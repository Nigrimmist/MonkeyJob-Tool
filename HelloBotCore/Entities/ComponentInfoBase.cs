using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using HelloBotCommunication;
using HelloBotCommunication.Interfaces;
using HelloBotCore.DAL.Interfaces;
using HelloBotCore.Helpers;
using HelloBotCore.Manager;
using Newtonsoft.Json;
using SharedHelper;

namespace HelloBotCore.Entities
{
    public abstract class ComponentInfoBase
    {
        private readonly StorageManager _storageManager;
        public bool IsEnabled { get; set; }
        public Guid Id { get; set; }
        public DescriptionInfo CommandDescription { get; set; }
        public double Version { get; set; }
        public double SettingsModuleVersion { get; set; }
        public string SystemName { get; set; }
        public string ProvidedTitle { get; set; }
        public Image Icon { get; set; }
        public AuthorInfo Author { get; set; }
        public Type SettingsType { get; set; }
        public ModuleLogStorageInfo Trace { get; set; }
        public abstract ModuleType ModuleType { get; }
        public int? InstanceId { get; set; }
        
        protected ComponentInfoBase(StorageManager storageManager)
        {
            _storageManager = storageManager;
            Id = Guid.NewGuid();
        }

        
        public T GetSettings<T>() where T : class
        {
            object additionalData;
            return GetSettings<T,object>(out additionalData);
        }

        public T GetSettings<T, T2>(out T2 serviceData)
            where T : class
            where T2 : class
        {
            serviceData = null;
            var settings = _storageManager.Get<ModuleSettings<T, T2>>(SystemName);
            if (settings == null)
                return null;
            serviceData = settings.ServiceData;
            return settings.ModuleData;
        }

        public void DeleteSettings()
        {
            _storageManager.Delete(SystemName);
        }

        public void SaveSettings<T>(T serializableSettingObject, object settingsAdditionalData=null) where T : class
        {
            var settings = new ModuleSettings<T>(Version, SettingsModuleVersion, serializableSettingObject,settingsAdditionalData);
            _storageManager.Save(SystemName,settings);
        }

        public void SaveServiceData<T>(T serviceData) where T : class
        {
            var settings = _storageManager.Get<ModuleSettings<object, T>>(SystemName);
            
            if (settings==null)
            {
                //create new raw settings with filled servicedata
                settings = new ModuleSettings<object,T>(Version, SettingsModuleVersion, new object(), serviceData);
            };

            SaveSettings(settings.ModuleData, serviceData);
        }

        public string GetModuleName()
        {
            return string.IsNullOrEmpty(this.ProvidedTitle) ? this.SystemName : this.ProvidedTitle;
        }

        public void Init(string dllName, ComponentBase componentBase, AuthorInfo author)
        {
            Version = componentBase.Version;
            SettingsModuleVersion = componentBase.ActualSettingsVersion;
            ProvidedTitle = componentBase.Title;
            var handType = componentBase.GetType();
            SystemName = dllName + "." + handType.Name+(InstanceId.HasValue?"_"+InstanceId.Value:"");

            if (!string.IsNullOrEmpty(componentBase.IconInBase64))
            {
                Icon = ImageHelper.ResizeImage(ImageHelper.GetFromBase64(componentBase.IconInBase64), 26, 26);
            }
            
            Author = author;

            Trace = new ModuleLogStorageInfo(30, SystemName,InstanceId,_storageManager);
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

       
    }


}
