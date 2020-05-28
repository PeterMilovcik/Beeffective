using Beeffective.Data.Entities;

namespace Beeffective.Core.Models
{
    public static class TaskModelExtensions
    {
        public static TaskEntity ToEntity(this TaskModel model) =>
            new TaskEntity
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                ProjectId = model.Project?.Id ?? 0,
                IsFinished = model.IsFinished,
                DueTo = model.DueTo
            };

        public static TaskModel ToModel(this TaskEntity entity, ProjectModel projectModel) =>
            new TaskModel
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                Project = projectModel,
                IsFinished = entity.IsFinished,
                DueTo = entity.DueTo
            };
    }
}
