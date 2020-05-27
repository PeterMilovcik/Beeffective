using System.Collections.Generic;
using System.Threading.Tasks;
using Beeffective.Data.Entities;

namespace Beeffective.Data
{
    public interface IRepository
    {
        Task<List<TaskEntity>> LoadTaskAsync();
        Task<List<RecordEntity>> LoadRecordAsync();
        Task<List<GoalEntity>> LoadGoalsAsync();
        Task<TaskEntity> AddTaskAsync(TaskEntity taskEntity);
        Task<GoalEntity> AddGoalAsync(GoalEntity goalEntity);
        Task<RecordEntity> AddRecordAsync(RecordEntity recordEntity);
        Task UpdateTaskAsync(TaskEntity taskEntity);
        Task UpdateGoalAsync(GoalEntity goalEntity);
        Task UpdateRecordAsync(RecordEntity recordEntity);
        Task RemoveTaskAsync(TaskEntity taskEntity);
        Task RemoveGoalAsync(GoalEntity goalEntity);
        Task RemoveRecordAsync(RecordEntity recordEntity);
        Task SaveTaskAsync(IEnumerable<TaskEntity> taskEntities);
        Task SaveGoalsAsync(IEnumerable<GoalEntity> goalEntities);
    }
}