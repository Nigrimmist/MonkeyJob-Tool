using HelloBotCore.DAL.Interfaces;
using MonkeyJobTool.Managers.Interfaces;

namespace MonkeyJobTool.Managers
{
    public class StorageManager:IStorageManager
    {
        private readonly IDataStorage _botStorageStrategy;

        public StorageManager(IDataStorage botStorageStrategy)
        {
            _botStorageStrategy = botStorageStrategy;
            //will use bot's storage file strategy to save requried ui data
        }

        public void Save(string key, object obj)
        {
            _botStorageStrategy.Save(key, obj);
        }

        public T Get<T>(string key) where T : class
        {
            return _botStorageStrategy.Read<T>(key);
        }

        public bool Exist(string key)
        {
            return _botStorageStrategy.Exist(key);
        }
    }
}
