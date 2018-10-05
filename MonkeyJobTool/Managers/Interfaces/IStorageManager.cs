using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyJobTool.Managers.Interfaces
{
    public interface IStorageManager
    {
        void Save(string key, object obj);
        T Get<T>(string key) where T : class;
        bool Exist(string key);
    }
}
