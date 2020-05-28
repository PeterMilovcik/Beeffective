using Beeffective.Data.Entities;

namespace Beeffective.Core.Models
{
    public static class LabelModelExtensions
    {
        public static LabelModel ToModel(this LabelEntity entity) =>
            new LabelModel
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description
            };

        public static LabelEntity ToEntity(this LabelModel model) =>
            new LabelEntity
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description
            };
    }
}