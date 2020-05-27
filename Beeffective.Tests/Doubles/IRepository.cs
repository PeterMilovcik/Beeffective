using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Beeffective.Data;
using Beeffective.Data.Entities;

namespace Beeffective.Tests.Doubles
{
    [Export(typeof(IRepository))]
    public class Repository : IRepository
    {
        public Repository()
        {
            TaskEntities = new List<TaskEntity>();
            RecordEntities = new List<RecordEntity>();
            GoalEntities = new List<GoalEntity>();
        }

        public List<TaskEntity> TaskEntities { get; }
        
        public List<RecordEntity> RecordEntities { get; }
        
        public List<GoalEntity> GoalEntities { get; }

        public Task<List<TaskEntity>> LoadTaskAsync() => Task.FromResult(TaskEntities);

        public Task<List<RecordEntity>> LoadRecordAsync() => Task.FromResult(RecordEntities);

        public Task<List<GoalEntity>> LoadGoalsAsync() => Task.FromResult(GoalEntities);

        public Task<TaskEntity> AddTaskAsync(TaskEntity taskEntity) =>
            Task.Run(() =>
            {
                var id = TaskEntities.Select(t => t.Id).Max() + 1;
                taskEntity.Id = id;
                TaskEntities.Add(taskEntity);
                return taskEntity;
            });

        public Task<GoalEntity> AddGoalAsync(GoalEntity goalEntity) =>
            Task.Run(() =>
            {
                var id = GoalEntities.Select(t => t.Id).Max() + 1;
                goalEntity.Id = id;
                GoalEntities.Add(goalEntity);
                return goalEntity;
            });

        public Task<RecordEntity> AddRecordAsync(RecordEntity recordEntity) =>
            Task.Run(() =>
            {
                var id = RecordEntities.Select(r => r.Id).Max() + 1;
                recordEntity.Id = id;
                RecordEntities.Add(recordEntity);
                return recordEntity;
            });

        public Task UpdateTaskAsync(TaskEntity taskEntity) =>
            Task.Run(() =>
            {
                var existing = TaskEntities.Single(t => t.Id == taskEntity.Id);
                existing.Title = taskEntity.Title;
                existing.Goal = taskEntity.Goal;
                existing.Importance = taskEntity.Importance;
                existing.Urgency = taskEntity.Urgency;
                existing.DueTo = taskEntity.DueTo;
                existing.IsFinished = taskEntity.IsFinished;
                existing.Tags = taskEntity.Tags;
            });

        public Task UpdateGoalAsync(GoalEntity goalEntity) =>
            Task.Run(() =>
            {
                var existing = GoalEntities.Single(t => t.Id == goalEntity.Id);
                existing.Title = goalEntity.Title;
                existing.Description = goalEntity.Description;
            });

        public Task UpdateRecordAsync(RecordEntity recordEntity) =>
            Task.Run(() =>
            {
                var existing = RecordEntities.Single(r => r.Id == recordEntity.Id);
                existing.TaskId = recordEntity.TaskId;
                existing.StartAt = recordEntity.StartAt;
                existing.StopAt = recordEntity.StopAt;
            });

        public Task RemoveTaskAsync(TaskEntity taskEntity) =>
            Task.Run(() => { TaskEntities.Remove(taskEntity); });

        public Task RemoveGoalAsync(GoalEntity goalEntity) =>
            Task.Run(() => { GoalEntities.Remove(goalEntity); });

        public Task RemoveRecordAsync(RecordEntity recordEntity) =>
            Task.Run(() => { RecordEntities.Remove(recordEntity); });

        public async Task SaveTaskAsync(IEnumerable<TaskEntity> taskEntities)
        {
            foreach (var taskEntity in taskEntities)
            {
                await UpdateTaskAsync(taskEntity);
            }
        }

        public async Task SaveGoalsAsync(IEnumerable<GoalEntity> goalEntities)
        {
            foreach (var goalEntity in goalEntities)
            {
                await UpdateGoalAsync(goalEntity);
            }
        }
    }
}