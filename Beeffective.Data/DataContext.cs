using System.ComponentModel.Composition;
using Beeffective.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Beeffective.Data
{
    [Export]
    public class DataContext : DbContext
    {
        public DbSet<GoalEntity> Goals { get; set; }
        public DbSet<ProjectEntity> Projects { get; set; }
        public DbSet<LabelEntity> Labels { get; set; }
        public DbSet<TaskEntity> Tasks { get; set; }
        public DbSet<RecordEntity> Records { get; set; }
        public DbSet<TaskLabelEntity> TaskLabels { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            base.OnConfiguring(options);
            options.UseSqlite("Data Source=beeffective.db");
            options.EnableSensitiveDataLogging();
        }
    }
}