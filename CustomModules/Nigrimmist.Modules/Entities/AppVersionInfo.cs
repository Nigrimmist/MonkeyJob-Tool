using System.Collections.Generic;

namespace Nigrimmist.Modules.Entities
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
