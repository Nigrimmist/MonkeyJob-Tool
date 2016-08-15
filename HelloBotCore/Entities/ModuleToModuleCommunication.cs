using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloBotCore.Entities
{
    public class ClientInstanceToModuleCommunication
    {
        public List<string> EnabledModules { get; set; }
        public List<string> DisabledModules { get; set; }
        public ModuleType? EnabledByType { get; set; }
        public ModuleType? DisabledByType { get; set; }
        public bool EnabledForAll { get; set; }

        public void Reset()
        {
            EnabledModules = new List<string>();
            DisabledModules = new List<string>();
            EnabledByType = null;
            DisabledByType = null;
            EnabledForAll = false;
        }

        public bool IsEnabledFor(string moduleSystemName, ModuleType moduleType)
        {
            if (EnabledForAll) return true;
            if (DisabledByType.HasValue) return DisabledByType.Value != moduleType;
            if (EnabledByType.HasValue) return EnabledByType.Value == moduleType;
            if (EnabledModules != null && EnabledModules.Any()) return EnabledModules.Contains(moduleSystemName);
            if (DisabledModules != null && DisabledModules.Any()) return !DisabledModules.Contains(moduleSystemName);

            throw new ApplicationException("IsEnabledFor works incorrect");
        }

        public ClientInstanceToModuleCommunication()
        {
            EnabledModules = new List<string>();
            DisabledModules = new List<string>();
        }

        public static ClientInstanceToModuleCommunication GetDefault()
        {
            return new ClientInstanceToModuleCommunication()
            {
                
                EnabledForAll = false,
                EnabledByType = ModuleType.Event
            };
        }
    }
}
