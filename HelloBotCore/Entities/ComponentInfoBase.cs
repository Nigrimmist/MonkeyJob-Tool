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
        private readonly StorageManager _moduleStorageManager;
        public virtual bool IsEnabled { get; set; }
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
        public bool IsMainComponent { get { return !InstanceId.HasValue; } }
        private List<ComponentInfoBase> _instances;
        public List<ComponentInfoBase> Instances { get => _instances; set => _instances = value; }
        public virtual void Dispose() { }

        public virtual void RemoveInstance(string systemName)
        {
            Instances = Instances.Where(x => x.SystemName == systemName).ToList();
        }


        protected ComponentInfoBase(StorageManager moduleStorageManager)
        {
            _moduleStorageManager = moduleStorageManager;
            Id = Guid.NewGuid();
            Instances = new List<ComponentInfoBase>();

        }

        public ModuleSettings GetSettings() 
        {
            return _moduleStorageManager.Get<ModuleSettings>(SystemName); 
        }

        public TModuleType GetSettings<TModuleType>() where TModuleType : class
        {
            object additionalData;
            return GetSettings<TModuleType, object>(out additionalData);
        }

        public TModuleType GetSettings<TModuleType, TServiceType>(out TServiceType serviceData)
            where TModuleType : class
            where TServiceType : class
        {
            serviceData = null;
            var settings = _moduleStorageManager.Get<ModuleSettings<TModuleType, TServiceType>>(SystemName);
            if (settings == null)
                return null;
            serviceData = settings.ServiceData;
            return settings.ModuleData;
        }

        public void DeleteSettings()
        {
            _moduleStorageManager.Delete(SystemName);
        }

        public void SaveSettings<TModuleType>(TModuleType serializableSettingObject, object settingsAdditionalData=null) where TModuleType : class
        {
            var settings = new ModuleSettings<TModuleType>(Version, SettingsModuleVersion, serializableSettingObject,settingsAdditionalData);
            _moduleStorageManager.Save(SystemName,settings);
        }

        public void SaveServiceData<TServiceType>(TServiceType serviceData) where TServiceType : class
        {
            var settings = _moduleStorageManager.Get<ModuleSettings<object, TServiceType>>(SystemName);
            
            if (settings==null)
            {
                //create new raw settings with filled servicedata
                settings = new ModuleSettings<object, TServiceType>(Version, SettingsModuleVersion, new object(), serviceData);
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

            Trace = new ModuleLogStorageInfo(AppConstants.DefaultModuleTraceCount, SystemName,InstanceId,_moduleStorageManager);
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

        public virtual  Func<int?, ComponentInfoBase> CreateNewInstanceFunc { get; set; }
    }


}
