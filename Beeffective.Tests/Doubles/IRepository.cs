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
        }

        public List<TaskEntity> TaskEntities { get; }
        
        public List<RecordEntity> RecordEntities { get; }

        public Task<List<TaskEntity>> LoadTaskAsync() => Task.FromResult(TaskEntities);

        public Task<List<RecordEntity>> LoadRecordAsync() => Task.FromResult(RecordEntities);

        public Task<TaskEntity> AddTaskAsync(TaskEntity taskEntity) =>
            Task.Run(() =>
            {
                var id = TaskEntities.Select(t => t.Id).Max() + 1;
                taskEntity.Id = id;
                TaskEntities.Add(taskEntity);
                return taskEntity;
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

        public Task RemoveRecordAsync(RecordEntity recordEntity) =>
            Task.Run(() => { RecordEntities.Remove(recordEntity); });

        public async Task SaveTaskAsync(IEnumerable<TaskEntity> taskEntities)
        {
            foreach (var taskEntity in taskEntities)
            {
                await UpdateTaskAsync(taskEntity);
            }
        }
    }
}