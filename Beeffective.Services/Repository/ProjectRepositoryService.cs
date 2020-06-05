using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Beeffective.Core.Models;
using Beeffective.Data.Repositories;

namespace Beeffective.Services.Repository
{
    public class ProjectRepositoryService : IRepositoryService<ProjectModel>
    {
        private readonly IRepository repository;
        private readonly IRepositoryService service;

        public ProjectRepositoryService(IRepository repository, IRepositoryService service)
        {
            this.repository = repository;
            this.service = service;
            List = new List<ProjectModel>();
        }

        public List<ProjectModel> List { get; }

        public async Task<List<ProjectModel>> LoadAsync()
        {
            var projectModels = new List<ProjectModel>();
            var entities = await repository.Projects.LoadAsync();
            foreach (var projectEntity in entities)
            {
                var goalModel = service.Goals.List.Find(g => g.Id == projectEntity.GoalId);
                var projectModel = projectEntity.ToModel(goalModel);
                projectModels.Add(projectModel);
            }
            List.ForEach(Unsubscribe);
            List.Clear();
            projectModels.ForEach(Subscribe);
            List.AddRange(projectModels);
            return projectModels;
        }

        public async Task<ProjectModel> AddAsync(ProjectModel newProjectModel)
        {
            var projectModel = (await repository.Projects.AddAsync(newProjectModel.ToEntity())).ToModel(newProjectModel.Goal);
            Subscribe(projectModel);
            List.Add(projectModel);
            return projectModel;
        }

        public Task UpdateAsync(ProjectModel projectModel) =>
            repository.Projects.UpdateAsync(projectModel.ToEntity());

        public async Task RemoveAsync(ProjectModel projectModel)
        {
            Unsubscribe(projectModel);
            List.Remove(projectModel);
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