using System.IO;
using HelloBotCore.DAL.Interfaces;
using HelloBotCore.Manager;
using Newtonsoft.Json;

namespace HelloBotCore.DAL.StorageServices
{
    /// <summary>
    /// Text file storage
    /// </summary>
    class FileStorage : IDataStorage
    {
        private readonly string _executionFolder;
        public FileStorage(string executionFolder)
        {
            _executionFolder = executionFolder;
        }

        public FileStorage()
        {
            _executionFolder = App.Instance.ExecutionFolderPath;
        }

        public void Save(string key, object data)
        {
            string serializedObject = JsonConvert.SerializeObject(data);
            string filePath = _executionFolder + key;
            if (File.Exists(filePath))
            {
                File.WriteAllText(filePath, serializedObject);
            }
        }

        public T Read<T>(string key) where T : class
        {
            string filePath = _executionFolder + key;

            if (!File.Exists(filePath))
                return  null;
            string content = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}
