namespace HelloBotCore.DAL.Interfaces
{
    public interface IDataStorage
    {
        void Save(string key, object data);
        T Get<T>(string key) where T : class;
        bool Exist(string key);
        void Delete(string key);
    }
}
