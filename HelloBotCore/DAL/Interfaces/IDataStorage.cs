﻿namespace HelloBotCore.DAL.Interfaces
{
    public interface IDataStorage
    {
        void Save(string key, object data);
        T Read<T>(string key) where T : class;
        bool Exist(string key);
        void Delete(string key);
    }
}
