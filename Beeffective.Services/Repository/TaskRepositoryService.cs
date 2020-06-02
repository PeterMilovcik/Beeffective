using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Beeffective.Core.Models;
using Beeffective.Data.Entities;
using Beeffective.Data.Repositories;

namespace Beeffective.Services.Repository
{
    [Export(typeof(IRepositoryService<TaskModel>))]
    public class TaskRepositoryService : IRepositoryService<TaskModel>
    {
        private readonly IRepository repository;
        private readonly List<TaskModel> list;

        [ImportingConstructor]
        public TaskRepositoryService(IRepository repository)
        {
            this.repository = repository;
            list = new List<TaskModel>();
        }

        public async Task<List<TaskModel>> LoadAsync()
        {
            var taskModels = new List<TaskModel>();
            var taskEntities = await repository.Tasks.LoadAsync();
            var labelEntities = await repository.Labels.LoadAsync();
            var labelModels = labelEntities.Select(e => e.ToModel());
            var taskLabelEntities = await repository.TaskLabels.LoadAsync();
            var goalEntities = await repository.Goals.LoadAsync();
            var projectEntities = await repository.Projects.LoadAsync();
            foreach (var taskEntity in taskEntities)
            {
                var projectEntity = projectEntities.Find(e => e.Id == taskEntity.ProjectId);
                var goalEntity = projectEntity != null 
                    ? goalEntities.Find(e => e.Id == projectEntity.GoalId)
                    : null;
                var goalModel = goalEntity.ToModel();
                var projectModel = projectEntity.ToModel(goalModel);
                var taskModel = taskEntity.ToModel(projectModel);

                taskLabelEntities
                    .Where(e => e.TaskId == taskEntity.Id)
                    .Select(e => e.LabelId)
                    .Select(labelId => labelModels.Single(lm => lm.Id == labelId))
                    .ToList()
                    .ForEach(label => taskModel.Labels.Add(label));

                taskModels.Add(taskModel);
            }

            list.ForEach(Unsubscribe);
            list.Clear();
            taskModels.ForEach(Subscribe);
            list.AddRange(taskModels);

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
            list.Add(savedTaskModel);

            return savedTaskModel;
        }

        public Task UpdateAsync(TaskModel taskModel) =>
            repository.Tasks.UpdateAsync(taskModel.ToEntity());

        public async Task RemoveAsync(TaskModel taskModel)
        {
            Unsubscribe(taskModel);
            list.Remove(taskModel);
            await repository.Tasks.RemoveAsync(taskModel.ToEntity());
        }

        public Task SaveAsync(List<TaskModel> taskModels) =>
            repository.Tasks.SaveAsync(taskModels.Select(taskModel => taskModel.ToEntity()));

        private void Subscribe(TaskModel taskModel)
        {
            taskModel.Changed += OnChanged;
            taskModel.LabelAdded += OnLabelAdded;
            taskModel.LabelRemoved += OnLabelRemoved;
        }

        private void Unsubscribe(TaskModel taskModel)
        {
            taskModel.Changed -= OnChanged;
            taskModel.LabelAdded -= OnLabelAdded;
            taskModel.LabelRemoved -= OnLabelRemoved;
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
    }
}