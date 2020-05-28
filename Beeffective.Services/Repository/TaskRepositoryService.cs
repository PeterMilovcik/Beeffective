using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Beeffective.Core.Models;
using Beeffective.Data.Repositories;

namespace Beeffective.Services.Repository
{
    [Export(typeof(IRepositoryService<TaskModel>))]
    public class TaskRepositoryService : IRepositoryService<TaskModel>
    {
        private readonly IRepository repository;

        [ImportingConstructor]
        public TaskRepositoryService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<List<TaskModel>> LoadAsync()
        {
            var result = new List<TaskModel>();
            var entities = await repository.Tasks.LoadAsync();
            foreach (var taskEntity in entities)
            {
                var projectEntity = await repository.Projects.GetById(taskEntity.ProjectId);
                var goalEntity = projectEntity != null ? await repository.Goals.GetById(projectEntity.GoalId) : null;
                var goalModel = goalEntity.ToModel();
                var projectModel = projectEntity.ToModel(goalModel);
                var taskModel = taskEntity.ToModel(projectModel);
                result.Add(taskModel);
            }

            return result;
        }

        public async Task<TaskModel> AddAsync(TaskModel newTaskModel)
        {
            var taskEntity = await repository.Tasks.AddAsync(newTaskModel.ToEntity());
            return taskEntity.ToModel(newTaskModel.Project);
        }

        public Task UpdateAsync(TaskModel taskModel) =>
            repository.Tasks.UpdateAsync(taskModel.ToEntity());

        public Task RemoveAsync(TaskModel taskModel) =>
            repository.Tasks.RemoveAsync(taskModel.ToEntity());

        public Task SaveAsync(List<TaskModel> taskModels) =>
            repository.Tasks.SaveAsync(taskModels.Select(taskModel => taskModel.ToEntity()));
    }
}