using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonkeyJobTool.Entities
{
    public class AppConstants
    {
        public const string DateTimeFormat = "dd-MM-yyyy";
        public const string AppName = "MonkeyJob Tool";
        public const double AppVersion = 0.6;

        /// <summary>
        /// Should be changed only in order with migrations
        /// </summary>
        public const double ConfigVersion = 0.1;
        

        public class Registry
        {
            public const string LastStatsCollectedDateKey = "LastStatsDate";
            public const string FirstRun = "FirstRunV2";
        }

        public class Paths
        {
            public const string MainConfFileName = @"conf.json";
        }

        public class Urls
        {
            public const string DonateListUrl = "https://raw.githubusercontent.com/Nigrimmist/MonkeyJob-Tool/master/ServerData/Donate.txt";
        }
    }
}
