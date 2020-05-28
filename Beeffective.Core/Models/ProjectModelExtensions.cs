using System;
using Beeffective.Data.Entities;

namespace Beeffective.Core.Models
{
    public static class ProjectModelExtensions
    {
        public static ProjectModel ToModel(this ProjectEntity entity, GoalModel goal)
        {
            if (entity == null) return null;
            return new ProjectModel
            {
                Id = entity.Id,
                Goal = goal,
                Title = entity.Title,
                Description = entity.Description,
            };
        }

        public static ProjectEntity ToEntity(this ProjectModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            return new ProjectEntity
            {
                Id = model.Id,
                GoalId = model.Goal.Id,
                Title = model.Title,
                Description = model.Description
            };
        }
    }
}