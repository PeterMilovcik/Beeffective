using Beeffective.Core.Models;

namespace Beeffective.Presentation.Main.Tasks
{
    public static class TaskViewModelExtensions
    {
        public static TaskModel ToModel(this TaskViewModel viewModel) =>
            new TaskModel
            {
                Id = viewModel.Id,
                Title = viewModel.Title,
                Urgency = viewModel.Urgency,
                Importance = viewModel.Importance
            };

        public static TaskViewModel ToViewModel(this TaskModel model) =>
            new TaskViewModel
            {
                Id = model.Id,
                Title = model.Title,
                Urgency = model.Urgency,
                Importance = model.Importance,
                IsChanged = false
            };
    }
}
