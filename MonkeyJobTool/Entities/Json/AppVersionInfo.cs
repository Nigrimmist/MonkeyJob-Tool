using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonkeyJobTool.Entities.Json
{
    public class AppVersionInfo
    {
        public List<VersionInfo> Versions { get; set; }

        public AppVersionInfo()
        {
            Versions = new List<VersionInfo>();
        }
    }

    public class VersionInfo
    {
        public float Version { get; set; }
        public string WhatsNew { get; set; }
    }
}
