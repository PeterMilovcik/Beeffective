using System.ComponentModel.Composition;
using Beeffective.Core.Models;
using Beeffective.Data.Repositories;

namespace Beeffective.Services.Repository
{
    [Export(typeof(IRepositoryService))]
    public class RepositoryService : IRepositoryService
    {
        [ImportingConstructor]
        public RepositoryService(IRepository repository)
        {
            Goals = new GoalRepositoryService(repository);
            Projects = new ProjectRepositoryService(repository, this);
            Labels = new LabelRepositoryService(repository);
            Tasks = new TaskRepositoryService(repository, this);
        }

        public IRepositoryService<GoalModel> Goals { get; set; }

        public IRepositoryService<ProjectModel> Projects { get; set; }

        public IRepositoryService<TaskModel> Tasks { get; set; }

        public IRepositoryService<LabelModel> Labels { get; set; }
    }
}