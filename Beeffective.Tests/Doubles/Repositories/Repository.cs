using System.ComponentModel.Composition;
using Beeffective.Data.Entities;
using Beeffective.Data.Repositories;

namespace Beeffective.Tests.Doubles.Repositories
{
    [Export(typeof(IRepository))]
    public class Repository : IRepository
    {
        [Import]
        public IRepository<GoalEntity> Goals { get; set; }

        [Import]
        public IRepository<ProjectEntity> Projects { get; set; }

        [Import]
        public IRepository<TaskEntity> Tasks { get; set; }

        [Import]
        public IRepository<RecordEntity> Records { get; set; }
    }
}