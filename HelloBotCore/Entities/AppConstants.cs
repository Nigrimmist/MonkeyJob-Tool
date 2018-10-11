using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloBotCore.Entities
{
    internal class AppConstants
    {
        /// <summary>
        /// if exceeds - will be overriden by new
        /// </summary>
        public static readonly int DefaultModuleTraceCount = 30;
        public static class Names
        {
            public static string ModuleSettingsFolderPostFix = "Settings\\";
            public static string DbName = "mj.db";
        }
    }
}
