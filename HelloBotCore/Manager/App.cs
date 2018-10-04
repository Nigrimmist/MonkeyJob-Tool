using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloBotCore.Manager
{
    internal class App
    {
        private static object _appLocker= new object();
        private static App _instance;
        public static App Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_appLocker)
                    {
                        if (_instance == null)
                        {
                            _instance = new App();
                        }
                    }
                }
                return _instance;
            }
        }

        Action<string> _logTraceFunc = delegate {  };
        private string _executionFolderPath;

        public void Init(Action<string> logTraceFunc,string executionFolderPath)
        {
            _logTraceFunc = logTraceFunc;
            _executionFolderPath = executionFolderPath;
        }

        public void LogTrace(string msg)
        {
            _logTraceFunc(msg);
        }

        public string ExecutionFolderPath => _executionFolderPath;
    }
}
