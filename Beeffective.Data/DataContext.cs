using System.ComponentModel.Composition;
using Beeffective.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Beeffective.Data
{
    [Export]
    public class DataContext : DbContext
    {
        public DataContext()
        {
            Database.Migrate();
        }

        public DbSet<TaskEntity> Tasks { get; set; }
        public DbSet<RecordEntity> Records { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            base.OnConfiguring(options);
            options.UseSqlite("Data Source=database.db");
            options.EnableSensitiveDataLogging();
        }
    }
}