﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace HelloBotCore.Entities
{
    public class ModuleLogStorageInfo
    {
        public List<LogMessage> TraceMessages { get; set; }
        private int _limit;
        private readonly string _moduleSystemName;
        private bool _dataAddedSinceLastSave = false;
        private string _savePath { get; set; }

        public ModuleLogStorageInfo(int limit, string savePath, string moduleName)
        {
            _limit = limit;
            _moduleSystemName = moduleName;
            _savePath = savePath;
            TraceMessages = new List<LogMessage>();
        }
        
        public void AddMessage(string message)
        {
            TraceMessages.Add(new LogMessage(){Message = message});
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
                var fPath = GetLogFileFullPath();
                if (!File.Exists(fPath))
                    File.Create(fPath);
                File.WriteAllText(fPath,JsonConvert.SerializeObject(TraceMessages));
                _dataAddedSinceLastSave = false;
            }
        }

        public void Load()
        {
            if (File.Exists(GetLogFileFullPath()))
            {
                TraceMessages = JsonConvert.DeserializeObject<List<LogMessage>>(File.ReadAllText(GetLogFileFullPath()));
            }
        }

        public string GetLogFileFullPath()
        {
            return _savePath + "/" + _moduleSystemName + ".json";
        }
    }
}