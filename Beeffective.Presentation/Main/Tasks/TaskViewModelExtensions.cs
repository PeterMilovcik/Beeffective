using System.Collections.ObjectModel;
using System.Linq;
using Beeffective.Core.Models;
using Beeffective.Presentation.Main.Record;

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
                Importance = viewModel.Importance,
                Goal = viewModel.Goal,
                Tags = viewModel.Tags,
                Records = viewModel.Records.Select(r => r.ToModel()).ToList()
            };

        public static TaskViewModel ToViewModel(this TaskModel model) =>
            new TaskViewModel
            {
                Id = model.Id,
                Title = model.Title,
                Urgency = model.Urgency,
                Importance = model.Importance,
                Goal = model.Goal,
                Tags = model.Tags,
                Records = new ObservableCollection<RecordViewModel>(
                    model.Records.Select(r => r.ToViewModel())),
                IsChanged = false
            };
    }
}
