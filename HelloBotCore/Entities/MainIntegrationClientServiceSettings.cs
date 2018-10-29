using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace HelloBotCore.Entities
{
    public class MainIntegrationClientServiceSettings
    {
        public ClientInstanceToModuleCommunication ClientInstanceToModuleCommunication { get; set; }

        /// <summary>
        /// Message Hashes to prevent displaying old messages twice on clients. struct : moduleName -> {groupId-> list of hashes}
        /// </summary>
        public IDictionary<string, IDictionary<string, List<string>>> ModuleMessageHashes { get; set; }

        public MainIntegrationClientServiceSettings()
        {
            ModuleMessageHashes = new Dictionary<string, IDictionary<string, List<string>>>();
        }

        public bool MessageHashExist(string moduleSystemName,string groupId, string hash)
        {
            IDictionary<string, List<string>> groupHashes;
            if (ModuleMessageHashes.TryGetValue(moduleSystemName, out groupHashes))
            {
                List<string> messageHashes;
                if (groupHashes.TryGetValue(groupId, out messageHashes))
                {
                    return messageHashes.Contains(hash);
                }
            }
            return false;
        }

        public void AddHash(string moduleSystemName, string groupId, string hash)
        {
            IDictionary<string, List<string>> groupHashes;
            if (ModuleMessageHashes.TryGetValue(moduleSystemName, out groupHashes))
            {
                List<string> messageHashes;
                if (groupHashes.TryGetValue(groupId, out messageHashes))
                {
                    var storeLimit = 10;
                    if (messageHashes.Count > storeLimit )
                        messageHashes.RemoveRange(0, messageHashes.Count - storeLimit - 1);
                    messageHashes.Add(hash);
                }
                else
                    groupHashes.Add(groupId, new List<string> { hash });
            }
            else
            {
                ModuleMessageHashes.Add(moduleSystemName, new Dictionary<string, List<string>>() { { groupId, new List<string> { hash } } });
            }
        }

        public void DeleteHashes(string groupId, string moduleSystemName)
        {
            IDictionary<string, List<string>> groupHashes;
            if (ModuleMessageHashes.TryGetValue(moduleSystemName, out groupHashes))
            {
                List<string> messageHashes;
                if (groupHashes.TryGetValue(groupId, out messageHashes))
                {
                    messageHashes.Clear();
                }
            }
        }
    }
}
