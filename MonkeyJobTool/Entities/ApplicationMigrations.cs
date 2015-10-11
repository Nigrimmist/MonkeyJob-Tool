using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonkeyJobTool.Managers;

namespace MonkeyJobTool.Entities
{
    public class ApplicationMigrations
    {
        private static readonly List<MigrationInfo> _configMigrations = new List<MigrationInfo>()
        {
            new MigrationInfo(0, 0.1) {ApplyAction = () => 
            {
                App.Instance.AppConf.ShowCommandHelp = true;
                App.Instance.AppConf.SystemData.ClearCommandAfterMinOfInactivity = TimeSpan.FromMinutes(5); 
            }}
        };

        public static void UpdateAppMigrations()
        {
            //config migration
            var topMigrationVersion = _configMigrations.Max(x => x.ToVersion);
            if (App.Instance.AppConf.ConfigVersion < topMigrationVersion)
            {
                var migrations = _configMigrations.Where(x => x.FromVersion >= App.Instance.AppConf.ConfigVersion).OrderBy(x => x.FromVersion);
                foreach (MigrationInfo migration in migrations)
                {
                    try
                    {
                        migration.ApplyAction();
                        App.Instance.AppConf.ConfigVersion = migration.ToVersion;
                        App.Instance.AppConf.Save();
                    }
                    catch (Exception ex)
                    {
                        LogManager.Error(ex, string.Format("Config Migration exception.from {0} to {1}", migration.FromVersion, migration.ToVersion));
                        throw;
                    }
                }
            }
        }
    }


    public class MigrationInfo
    {
        public double FromVersion { get; set; }
        public double ToVersion { get; set; }
        public Action ApplyAction;

        public MigrationInfo(double fromVersion, double toVersion)
        {
            FromVersion = fromVersion;
            ToVersion = toVersion;
        }
    }
}
