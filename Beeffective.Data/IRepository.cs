using System.Collections.Generic;
using System.Threading.Tasks;
using Beeffective.Data.Entities;

namespace Beeffective.Data
{
    public interface IRepository
    {
        Task<List<TaskEntity>> LoadTaskAsync();
        Task<TaskEntity> AddTaskAsync(TaskEntity taskEntity);
        Task UpdateTaskAsync(TaskEntity taskEntity);
    }
}