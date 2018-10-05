using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using HelloBotCore.Manager;
using Newtonsoft.Json;

namespace HelloBotCore.Entities
{
    public class ModuleLogStorageInfo
    {
        public List<LogMessage> TraceMessages { get; set; }
        private const string LOG_KEY_PREFIX = "module_logs_";
        private int _limit;
        private readonly string _moduleSystemName;
        private readonly int? _instanceId;
        private readonly StorageManager _storageManager;
        private bool _dataAddedSinceLastSave = false;
        private string _savePath { get; set; }

        public ModuleLogStorageInfo(int limit, string moduleName, int? instanceId, StorageManager storageManager)
        {
            _limit = limit;
            _moduleSystemName = moduleName;
            _instanceId = instanceId;
            _storageManager = storageManager;
            TraceMessages = new List<LogMessage>();
        }
        
        public void AddMessage(string message)
        {
            TraceMessages.Add(new LogMessage() { Message = (_instanceId.HasValue ? _instanceId + " : " : "") + message });
            if (TraceMessages.Count > _limit)
            {
                TraceMessages.RemoveRange(0, TraceMessages.Count - _limit);
            }
            _dataAddedSinceLastSave = true;
        }

        public void Save()
        {
            if (_dataAddedSinceLastSave && TraceMessages.Any())
            {
                _storageManager.Save(GetLogModuleKey(), TraceMessages);
                
                _dataAddedSinceLastSave = false;
            }
        }

        public void Load()
        {
            TraceMessages = _storageManager.Get<List<LogMessage>>(GetLogModuleKey()) ?? new List<LogMessage>();
        }

        private string GetLogModuleKey()
        {
            return LOG_KEY_PREFIX+_moduleSystemName;
        }
    }
}
