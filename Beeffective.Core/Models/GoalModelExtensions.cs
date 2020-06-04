using System;
using Beeffective.Data.Entities;

namespace Beeffective.Core.Models
{
    public static class GoalModelExtensions
    {
        public static GoalModel ToModel(this GoalEntity entity)
        {
            if (entity == null) return null;
            return new GoalModel
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                Importance = entity.Importance
            };
        }

        public static GoalEntity ToEntity(this GoalModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            return new GoalEntity
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                Importance = model.Importance
            };
        }
    }
}