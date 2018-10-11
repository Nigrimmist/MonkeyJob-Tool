using HelloBotCore.DAL.Interfaces;
using HelloBotCore.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;



namespace HelloBotCore.DAL.StorageServices
{
    public class SqlLiteStorage : IDataStorage
    {
        private readonly string _rootPath;
        private readonly IsoDateTimeConverter _isoDateTimeConverter;

        public SqlLiteStorage(string rootPath, IsoDateTimeConverter isoDateTimeConverter)
        {
            this._rootPath = rootPath;
            this._isoDateTimeConverter = isoDateTimeConverter;
            using (SQLiteContext dataContext = CreateContext())
            {
                dataContext.Database.CreateIfNotExists();
                //dataContext.Database.Initialize(false);
            }

        }
        public void Delete(string key)
        {
            using (SQLiteContext dataContext = CreateContext())
            {
                List<ModuleSettings> entities = dataContext.ModuleSettings.Where(x => x.Key == key).ToList();
                foreach(var ent in entities)
                {
                    dataContext.ModuleSettings.Remove(ent);
                }
                dataContext.SaveChanges();
            }
        }

        public bool Exist(string key)
        {
            using (SQLiteContext dataContext = CreateContext())
            {
                return dataContext.ModuleSettings.Any(x => x.Key == key);                
            }
        }

        public T Get<T>(string key) where T : class
        {
            using (SQLiteContext dataContext = CreateContext())
            {
                var entity = dataContext.ModuleSettings.SingleOrDefault(x => x.Key == key);
                if (entity != null)
                {
                    return JsonConvert.DeserializeObject<T>(entity.Json, _isoDateTimeConverter);
                }
            }
            return null;
        }

        public void Save(string key, object data)
        {
            var serializedData = JsonConvert.SerializeObject(data);
            using (SQLiteContext dataContext = CreateContext())
            {
                
                var existingEntity = dataContext.ModuleSettings.SingleOrDefault(x => x.Key == key);
                if (existingEntity!=null)
                {
                    existingEntity.Json = serializedData;
                }
                else
                {
                    dataContext.ModuleSettings.Add(new ModuleSettings
                    {
                        Json = serializedData,
                        Key = key
                    });
                }

                dataContext.SaveChanges();
            }
        }

        private SQLiteContext CreateContext()
        {
            return new SQLiteContext($"Data Source = {AppConstants.Names.DbName}");
        }
    }

    public class SQLiteContext : DbContext
    {
        public SQLiteContext(string connString) :
            base(new SQLiteConnection() { ConnectionString = connString },true)
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
