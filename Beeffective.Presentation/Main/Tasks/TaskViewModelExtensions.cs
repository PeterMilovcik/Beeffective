using Beeffective.Core.Models;

namespace Beeffective.Presentation.Main.Tasks
{
    public static class TaskViewModelExtensions
    {
        public static TaskModel ToModel(this TaskViewModel viewModel) =>
            new TaskModel
            {
                Id = viewModel.Id,
                Title = viewModel.Title
            };
    }
}
