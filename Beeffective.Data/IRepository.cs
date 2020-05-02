using System.Collections.Generic;
using System.Threading.Tasks;
using Beeffective.Data.Entities;

namespace Beeffective.Data
{
    public interface IRepository
    {
        Task<List<TaskEntity>> LoadTaskAsync();
        Task<List<RecordEntity>> LoadRecordAsync();
        Task<TaskEntity> AddTaskAsync(TaskEntity taskEntity);
        Task<RecordEntity> AddRecordAsync(RecordEntity recordEntity);
        Task UpdateTaskAsync(TaskEntity taskEntity);
        Task UpdateRecordAsync(RecordEntity recordEntity);
        Task RemoveTaskAsync(TaskEntity taskEntity);
        Task RemoveRecordAsync(RecordEntity recordEntity);
    }
}