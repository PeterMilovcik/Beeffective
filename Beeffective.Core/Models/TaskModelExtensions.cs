﻿using Beeffective.Data.Entities;

namespace Beeffective.Core.Models
{
    public static class TaskModelExtensions
    {
        public static TaskEntity ToEntity(this TaskModel model) =>
            new TaskEntity
            {
                Id = model.Id,
                Title = model.Title,
                Urgency = model.Urgency,
                Importance = model.Importance
            };

        public static TaskModel ToModel(this TaskEntity entity) =>
            new TaskModel
            {
                Id = entity.Id,
                Title = entity.Title,
                Urgency = entity.Urgency,
                Importance = entity.Importance
            };
    }
}