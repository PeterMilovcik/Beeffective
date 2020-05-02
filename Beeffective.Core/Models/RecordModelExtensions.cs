using Beeffective.Data.Entities;

namespace Beeffective.Core.Models
{
    public static class RecordModelExtensions
    {
        public static RecordModel ToModel(this RecordEntity entity) =>
            new RecordModel
            {
                Id = entity.Id,
                TaskId = entity.TaskId,
                StartAt = entity.StartAt,
                StopAt = entity.StopAt
            };

        public static RecordEntity ToEntity(this RecordModel model) =>
            new RecordEntity
            {
                Id = model.Id,
                TaskId = model.TaskId,
                StartAt = model.StartAt,
                StopAt = model.StopAt
            };
    }
}
