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
        public void Init(Action<string> logTraceFunc)
        {
            _logTraceFunc = logTraceFunc;
        }

        public void LogTrace(string msg)
        {
            _logTraceFunc(msg);
        }
    }
}
