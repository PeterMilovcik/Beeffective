using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Beeffective.Core.Models;
using Beeffective.Data.Repositories;

namespace Beeffective.Services.Repository
{
    [Export(typeof(IRepositoryService<ProjectModel>))]
    public class ProjectRepositoryService : IRepositoryService<ProjectModel>
    {
        private readonly IRepository repository;

        [ImportingConstructor]
        public ProjectRepositoryService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<List<ProjectModel>> LoadAsync()
        {
            var result = new List<ProjectModel>();
            var entities = await repository.Projects.LoadAsync();
            foreach (var projectEntity in entities)
            {
                var goalEntity = await repository.Goals.GetById(projectEntity.GoalId);
                var goalModel = goalEntity.ToModel();
                var projectModel = projectEntity.ToModel(goalModel);
                result.Add(projectModel);
            }

            return result;
        }

        public async Task<ProjectModel> AddAsync(ProjectModel newProjectModel) =>
            (await repository.Projects.AddAsync(newProjectModel.ToEntity())).ToModel(newProjectModel.Goal);

        public Task UpdateAsync(ProjectModel projectModel) =>
            repository.Projects.UpdateAsync(projectModel.ToEntity());

        public Task RemoveAsync(ProjectModel projectModel) =>
            repository.Projects.RemoveAsync(projectModel.ToEntity());

        public Task SaveAsync(List<ProjectModel> projectModels) =>
            repository.Projects.SaveAsync(projectModels.Select(projectModel => projectModel.ToEntity()));
    }
}