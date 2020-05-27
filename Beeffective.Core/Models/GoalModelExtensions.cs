using Beeffective.Data.Entities;

namespace Beeffective.Core.Models
{
    public static class GoalModelExtensions
    {
        public static GoalModel ToModel(this GoalEntity entity) =>
            new GoalModel
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description
            };

        public static GoalEntity ToEntity(this GoalModel model) =>
            new GoalEntity
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description
            };
    }
}