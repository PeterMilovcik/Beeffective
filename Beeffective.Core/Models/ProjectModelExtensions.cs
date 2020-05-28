using Beeffective.Data.Entities;

namespace Beeffective.Core.Models
{
    public static class ProjectModelExtensions
    {
        public static ProjectModel ToModel(this ProjectEntity entity, GoalModel goal) =>
            new ProjectModel
            {
                Id = entity.Id,
                Goal = goal,
                Title = entity.Title,
                Description = entity.Description,
            };

        public static ProjectEntity ToEntity(this ProjectModel model) => 
            new ProjectEntity
            {
                Id = model.Id,
                GoalId = model.Goal.Id,
                Title = model.Title,
                Description = model.Description
            };
    }
}