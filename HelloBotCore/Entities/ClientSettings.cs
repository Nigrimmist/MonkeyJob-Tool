using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloBotCore.Entities
{
    public class ClientSettings
    {
        public ClientInstanceToModuleCommunication ClientInstanceToModuleCommunication { get; set; }
        public IDictionary<string, List<string>> ModuleMessageHashes { get; set; }

        public ClientSettings()
        {
            ModuleMessageHashes = new Dictionary<string, List<string>>();
        }

        public bool MessageHashExist(string moduleSystemName,string hash)
        {
            List<string> messageHashes;
            if (ModuleMessageHashes.TryGetValue(moduleSystemName, out messageHashes))
            {
                return messageHashes.Contains(hash);
            }
            return false;
        }

        public void AddHash(string moduleSystemName, string hash)
        {
            List<string> messageHashes;
            if (ModuleMessageHashes.TryGetValue(moduleSystemName, out messageHashes))
            {
                var storeLimit = 5;
                if (messageHashes.Count>storeLimit-1)
                    messageHashes.RemoveRange(0, messageHashes.Count - storeLimit-1);
                messageHashes.Add(hash);
            }
            else
            {
                ModuleMessageHashes.Add(moduleSystemName,new List<string>{hash});
            }
        }
    }
}
