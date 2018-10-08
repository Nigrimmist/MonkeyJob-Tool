using System.IO;
using HelloBotCore.DAL.Interfaces;
using HelloBotCore.Manager;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HelloBotCore.DAL.StorageServices
{
    /// <summary>
    /// Text file storage
    /// </summary>
    public class FileStorage : IDataStorage
    {
        private readonly string _executionFolder;
        private readonly IsoDateTimeConverter _isoDateTimeConverter;

        public FileStorage(string executionFolder, IsoDateTimeConverter isoDateTimeConverter)
        {
            if (!Directory.Exists(executionFolder))
                Directory.CreateDirectory(executionFolder);
            _executionFolder = executionFolder;
            _isoDateTimeConverter = isoDateTimeConverter;
        }

        public FileStorage()
        {
            _executionFolder = App.Instance.ExecutionFolderPath;            
        }

        public void Save(string key, object data)
        {
            string serializedObject = JsonConvert.SerializeObject(data);  
            File.WriteAllText(GetFilePath(key), serializedObject);
        }

        public T Read<T>(string key) where T : class
        {
            if (!File.Exists(GetFilePath(key)))
                return  null;
            string content = File.ReadAllText(GetFilePath(key));
            return JsonConvert.DeserializeObject<T>(content,_isoDateTimeConverter);
        }

        public bool Exist(string key)
        {
            return File.Exists(GetFilePath(key));
        }

        public void Delete(string key)
        {
            if(Exist(key))
                File.Delete(GetFilePath(key));
        }

        private string GetFilePath(string key)
        {
            return $"{_executionFolder}{key}.json";
        }
    }
}
