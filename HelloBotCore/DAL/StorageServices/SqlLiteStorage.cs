using HelloBotCore.DAL.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.SQLite;

namespace HelloBotCore.DAL.StorageServices
{
    public class SqlLiteStorage : IDataStorage
    {
        public SqlLiteStorage(string rootPath)
        {
            using (SQLiteConnection dataContext = new SQLiteConnection(connectionString))
            {
                if (!dataContext.DatabaseExists())
                {
                    dataContext.CreateDatabase();
                }
            }
        }
        public void Delete(string key)
        {
        }

        public bool Exist(string key)
        {
        }

        public T Read<T>(string key) where T : class
        {
        }

        public void Save(string key, object data)
        {
        }
    }

    public class SQLiteConnection : DbContext
    {
        public SQLiteConnection(string connString) :
            base(connString)
        { }
        public DbSet<ModuleSettings> ModuleSettings { get; set; }
    }

    [Table("ModuleSettings")]
    public class ModuleSettings
    {
        [Key]
        [Column("Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Column("Key")]
        public string Key { get; set; }
        [Column("Json")]
        public string Json { get; set; }
    }
}
