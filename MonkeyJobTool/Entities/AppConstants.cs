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

        public class Registry
        {
            public const string LastStatsCollectedDateKey = "LastStatsDate";
        }

        public class Paths
        {
            public const string MainConfFileName = @"conf.json";
        }

        public class Urls
        {
            public const string LatestVersionFileUrlFormat = "https://raw.githubusercontent.com/Nigrimmist/MonkeyJob-Tool/master/ServerData/LatestPublicVersion_{0}.txt";
        }
    }
}
