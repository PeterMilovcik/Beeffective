using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            var recordEntities = await repository.LoadRecordAsync();
            var taskModels = taskEntities.Select(taskEntity =>
            {
                var taskModel = taskEntity.ToModel();
                recordEntities
                    .Where(e1 => e1.TaskId == taskModel.Id)
                    .Select(e2 => e2.ToModel())
                    .ToList()
                    .ForEach(recordModel => taskModel.Records.Add(recordModel));
                return taskModel;
            }).ToList();

            var urgency = 0;
            foreach (var taskModel in taskModels
                .Where(tm => tm.DueTo.HasValue).
                OrderBy(tm => tm.DueTo.Value))
            {
                taskModel.Urgency = urgency;
                urgency++;
            }
            foreach (var taskModel in taskModels.Where(tm => !tm.DueTo.HasValue))
            {
                taskModel.Urgency = urgency;
            }
            return taskModels;
        }

        public async Task<List<RecordModel>> LoadRecordAsync()
        {
            var recordEntities = await repository.LoadRecordAsync();
            return recordEntities.Select(e => e.ToModel()).ToList();
        }

        public async Task<TaskModel> AddTaskAsync(TaskModel newTaskModel) => 
            (await repository.AddTaskAsync(newTaskModel.ToEntity())).ToModel();

        public async Task<RecordModel> AddRecordAsync(RecordModel newRecordModel) =>
            (await repository.AddRecordAsync(newRecordModel.ToEntity())).ToModel();
        
        public async Task UpdateTaskAsync(TaskModel taskModel) => 
            await repository.UpdateTaskAsync(taskModel.ToEntity());

        public async Task UpdateRecordAsync(RecordModel recordModel) =>
            await repository.UpdateRecordAsync(recordModel.ToEntity());

        public async Task RemoveTaskAsync(TaskModel taskModel) => 
            await repository.RemoveTaskAsync(taskModel.ToEntity());

        public async Task RemoveRecordAsync(RecordModel recordModel) =>
            await repository.RemoveRecordAsync(recordModel.ToEntity());

        public async Task SaveTaskAsync(List<TaskModel> taskModels)
        {
            foreach (var taskModel in taskModels)
            {
                foreach (var recordModel in taskModel.Records)
                {
                    if (recordModel.Id == 0)
                    {
                        await repository.AddRecordAsync(recordModel.ToEntity());
                    }
                    else
                    {
                        await repository.UpdateRecordAsync(recordModel.ToEntity());
                    }
                }
            }
            await repository.SaveTaskAsync(taskModels.Select(tm => tm.ToEntity()));
        }
    }
}