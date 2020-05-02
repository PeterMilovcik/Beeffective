using Beeffective.Core.Models;

namespace Beeffective.Presentation.Main.Record
{
    public static class RecordViewModelExtensions
    {
        public static RecordModel ToModel(this RecordViewModel viewModel) =>
            new RecordModel
            {
                Id = viewModel.Id,
                TaskId = viewModel.TaskId,
                StartAt = viewModel.StartAt,
                StopAt = viewModel.StopAt
            };

        public static RecordViewModel ToViewModel(this RecordModel model) =>
            new RecordViewModel
            {
                Id = model.Id,
                TaskId = model.TaskId,
                StartAt = model.StartAt,
                StopAt = model.StopAt
            };

    }
}