using System;
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
        private readonly List<ProjectModel> list;

        [ImportingConstructor]
        public ProjectRepositoryService(IRepository repository)
        {
            this.repository = repository;
            list = new List<ProjectModel>();
        }

        public async Task<List<ProjectModel>> LoadAsync()
        {
            var projectModels = new List<ProjectModel>();
            var entities = await repository.Projects.LoadAsync();
            foreach (var projectEntity in entities)
            {
                var goalEntity = await repository.Goals.GetById(projectEntity.GoalId);
                var goalModel = goalEntity.ToModel();
                var projectModel = projectEntity.ToModel(goalModel);
                projectModels.Add(projectModel);
            }
            list.ForEach(Unsubscribe);
            list.Clear();
            projectModels.ForEach(Subscribe);
            list.AddRange(projectModels);
            return projectModels;
        }

        public async Task<ProjectModel> AddAsync(ProjectModel newProjectModel)
        {
            var projectModel = (await repository.Projects.AddAsync(newProjectModel.ToEntity())).ToModel(newProjectModel.Goal);
            Subscribe(projectModel);
            list.Add(projectModel);
            return projectModel;
        }

        public Task UpdateAsync(ProjectModel projectModel) =>
            repository.Projects.UpdateAsync(projectModel.ToEntity());

        public async Task RemoveAsync(ProjectModel projectModel)
        {
            Unsubscribe(projectModel);
            list.Remove(projectModel);
            await repository.Projects.RemoveAsync(projectModel.ToEntity());
        }

        public Task SaveAsync(List<ProjectModel> projectModels) =>
            repository.Projects.SaveAsync(projectModels.Select(projectModel => projectModel.ToEntity()));

        private void Subscribe(ProjectModel projectModel) => projectModel.Changed += OnChanged;
        
        private void Unsubscribe(ProjectModel projectModel) => projectModel.Changed -= OnChanged;

        private async void OnChanged(object sender, EventArgs e)
        {
            if (sender is ProjectModel projectModel)
            {
                await UpdateAsync(projectModel);
            }
        }
    }
}