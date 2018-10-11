using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelloBotCore.DAL.Interfaces;

namespace HelloBotCore.Manager
{
    public class StorageManager
    {
        private readonly IDataStorage _storageService;

        public StorageManager(IDataStorage storageService)
        {
            _storageService = storageService;
        }

        public void Save(string key, object obj)
        {
            _storageService.Save(key,obj);
        }

        public T Get<T>(string key) where T : class
        {
            return _storageService.Get<T>(key);
        }

        public void Delete(string key)
        {
            _storageService.Delete(key);
        }
    }
}
