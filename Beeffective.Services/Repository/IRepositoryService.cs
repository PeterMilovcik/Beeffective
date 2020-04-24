using System.Collections.Generic;
using System.Threading.Tasks;
using Beeffective.Core.Models;

namespace Beeffective.Services.Repository
{
    public interface IRepositoryService
    {
        public Task<List<TaskModel>> LoadTaskAsync();
        Task<TaskModel> AddTaskAsync(TaskModel newTaskModel);
    }
}
