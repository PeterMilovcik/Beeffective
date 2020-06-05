using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Beeffective.Core.Models;
using Beeffective.Data.Entities;
using Beeffective.Data.Repositories;

namespace Beeffective.Services.Repository
{
    public class TaskRepositoryService : IRepositoryService<TaskModel>
    {
        private readonly IRepository repository;
        private readonly IRepositoryService service;

        public TaskRepositoryService(IRepository repository, IRepositoryService service)
        {
            this.repository = repository;
            this.service = service;
            List = new List<TaskModel>();
        }

        public List<TaskModel> List { get; }

        public async Task<List<TaskModel>> LoadAsync()
        {
            var taskModels = new List<TaskModel>();
            var taskEntities = await repository.Tasks.LoadAsync();
            var taskLabelEntities = await repository.TaskLabels.LoadAsync();
            var recordEntities = await repository.Records.LoadAsync();
            foreach (var taskEntity in taskEntities)
            {
                var projectModel = service.Projects.List.Find(p => p.Id == taskEntity.ProjectId);
                var taskModel = taskEntity.ToModel(projectModel);

                taskLabelEntities
                    .Where(e => e.TaskId == taskEntity.Id)
                    .Select(e => e.LabelId)
                    .Select(labelId => service.Labels.List.Single(lm => lm.Id == labelId))
                    .ToList()
                    .ForEach(label => taskModel.Labels.Add(label));

                recordEntities.Where(e => e.TaskId == taskEntity.Id)
                    .Select(e => e.ToModel())
                    .ToList()
                    .ForEach(record => taskModel.Records.Add(record));

                taskModels.Add(taskModel);
            }

            List.ForEach(Unsubscribe);
            List.Clear();
            taskModels.ForEach(Subscribe);
            List.AddRange(taskModels);

            return taskModels;
        }

        public async Task<TaskModel> AddAsync(TaskModel newTaskModel)
        {
            var taskEntity = await repository.Tasks.AddAsync(newTaskModel.ToEntity());
            var taskLabelEntities = await repository.TaskLabels.LoadAsync();
            foreach (var label in newTaskModel.Labels)
            {
                if (!taskLabelEntities.Any(tle => tle.TaskId == taskEntity.Id && tle.LabelId == label.Id))
                {
                    await repository.TaskLabels.AddAsync(
                        new TaskLabelEntity {TaskId = taskEntity.Id, LabelId = label.Id});
                }
            }

            var savedTaskModel = taskEntity.ToModel(newTaskModel.Project);
            newTaskModel.Labels.ToList().ForEach(label => savedTaskModel.Labels.Add(label));
            newTaskModel.Records.ToList().ForEach(record => savedTaskModel.Records.Add(record));

            Subscribe(savedTaskModel);
            List.Add(savedTaskModel);

            return savedTaskModel;
        }

        public Task UpdateAsync(TaskModel taskModel) =>
            repository.Tasks.UpdateAsync(taskModel.ToEntity());

        public async Task RemoveAsync(TaskModel taskModel)
        {
            Unsubscribe(taskModel);
            List.Remove(taskModel);
            await repository.Tasks.RemoveAsync(taskModel.ToEntity());
        }

        public Task SaveAsync(List<TaskModel> taskModels) =>
            repository.Tasks.SaveAsync(taskModels.Select(taskModel => taskModel.ToEntity()));

        private void Subscribe(TaskModel taskModel)
        {
            taskModel.Changed += OnChanged;
            taskModel.LabelAdded += OnLabelAdded;
            taskModel.LabelRemoved += OnLabelRemoved;
            taskModel.RecordAdded += OnRecordAdded;
            taskModel.RecordRemoved += OnRecordRemoved;
        }

        private void Unsubscribe(TaskModel taskModel)
        {
            taskModel.Changed -= OnChanged;
            taskModel.LabelAdded -= OnLabelAdded;
            taskModel.LabelRemoved -= OnLabelRemoved;
            taskModel.RecordAdded -= OnRecordAdded;
            taskModel.RecordRemoved -= OnRecordRemoved;
        }

        private async void OnChanged(object sender, EventArgs e)
        {
            if (sender is TaskModel taskModel)
            {
                await UpdateAsync(taskModel);
            }
        }

        private async void OnLabelAdded(object sender, LabelEventArgs e)
        {
            if (sender is TaskModel taskModel)
            {
                await repository.TaskLabels.AddAsync(
                    new TaskLabelEntity {LabelId = e.LabelModel.Id, TaskId = taskModel.Id});
            }
        }

        private async void OnLabelRemoved(object sender, LabelEventArgs e)
        {
            if (sender is TaskModel taskModel)
            {
                var taskLabelEntities = await repository.TaskLabels.LoadAsync();
                var taskLabelEntity = taskLabelEntities.Single(tle =>
                    tle.LabelId == e.LabelModel.Id &&
                    tle.TaskId == taskModel.Id);
                await repository.TaskLabels.RemoveAsync(taskLabelEntity);
            }
        }

        private async void OnRecordAdded(object sender, RecordEventArgs e) => 
            await repository.Records.AddAsync(e.RecordModel.ToEntity());

        private async void OnRecordRemoved(object sender, RecordEventArgs e) => 
            await repository.Records.RemoveAsync(e.RecordModel.ToEntity());
    }
}