﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Beeffective.Core.Models;

namespace Beeffective.Services.Repository
{
    public interface IRepositoryService
    {
        Task<List<TaskModel>> LoadTaskAsync();
        Task<List<RecordModel>> LoadRecordAsync();
        Task<List<GoalModel>> LoadGoalsAsync();
        Task<TaskModel> AddTaskAsync(TaskModel newTaskModel);
        Task<RecordModel> AddRecordAsync(RecordModel newRecordModel);
        Task UpdateTaskAsync(TaskModel taskModel);
        Task UpdateRecordAsync(RecordModel recordModel);
        Task RemoveTaskAsync(TaskModel taskModel);
        Task RemoveRecordAsync(RecordModel recordModel);
        Task SaveTaskAsync(List<TaskModel> taskModels);
        Task SaveGoalsAsync(List<GoalModel> goalModels);
    }
}
