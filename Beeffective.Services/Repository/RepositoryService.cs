using System.ComponentModel.Composition;
using Beeffective.Core.Models;

namespace Beeffective.Services.Repository
{
    [Export(typeof(IRepositoryService))]
    public class RepositoryService : IRepositoryService
    {
        [Import]
        public IRepositoryService<GoalModel> Goals { get; set; }

        [Import]
        public IRepositoryService<ProjectModel> Projects { get; set; }

        [Import]
        public IRepositoryService<TaskModel> Tasks { get; set; }
    }
}