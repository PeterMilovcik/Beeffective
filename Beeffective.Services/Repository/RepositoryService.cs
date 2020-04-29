using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Beeffective.Core.Models;
using Beeffective.Data;

namespace Beeffective.Services.Repository
{
    [Export(typeof(IRepositoryService))]
    public class RepositoryService : IRepositoryService
    {
        private readonly IRepository repository;

        [ImportingConstructor]
        public RepositoryService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<List<TaskModel>> LoadTaskAsync()
        {
            var taskEntities = await repository.LoadTaskAsync();
            return taskEntities.Select(e => e.ToModel()).ToList();
        }

        public async Task<TaskModel> AddTaskAsync(TaskModel newTaskModel) => 
            (await repository.AddTaskAsync(newTaskModel.ToEntity())).ToModel();

        public async Task UpdateTaskAsync(TaskModel taskModel) => 
            await repository.UpdateTaskAsync(taskModel.ToEntity());

        public async Task RemoveTaskAsync(TaskModel taskModel) => 
            await repository.RemoveTaskAsync(taskModel.ToEntity());
    }
}