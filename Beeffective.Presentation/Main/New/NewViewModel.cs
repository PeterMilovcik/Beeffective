using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Beeffective.Core.Models;
using Beeffective.Presentation.Common;
using Beeffective.Presentation.Main.Tasks;
using Beeffective.Services.Repository;

namespace Beeffective.Presentation.Main.New
{
    [Export]
    public class NewViewModel : ViewModel
    {
        private readonly IRepositoryService repository;
        private List<TaskModel> taskModels;
        private TaskViewModel newTaskViewModel;

        [ImportingConstructor]
        public NewViewModel(IRepositoryService repository)
        {
            this.repository = repository;
            SaveCommand = new DelegateCommand(
                o => CanSave(),async o => await SaveAsync());
        }

        public override async Task InitializeAsync()
        {
            taskModels = await repository.LoadTaskAsync();
            NewTask = new TaskViewModel();
        }

        public TaskViewModel NewTask
        {
            get => newTaskViewModel;
            set
            {
                if (newTaskViewModel != null) UnsubscribeFrom(newTaskViewModel);
                if (SetProperty(ref newTaskViewModel, value)) SubscribeTo(newTaskViewModel);
            }
        }

        private void SubscribeTo(TaskViewModel taskViewModel) => 
            taskViewModel.PropertyChanged += OnTaskViewModelPropertyChanged;

        private void UnsubscribeFrom(TaskViewModel taskViewModel) => 
            taskViewModel.PropertyChanged -= OnTaskViewModelPropertyChanged;

        private void OnTaskViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(newTaskViewModel.Title))
            {
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand SaveCommand { get; }

        private bool CanSave()
        {
            if (newTaskViewModel == null) return false;
            if (string.IsNullOrWhiteSpace(newTaskViewModel.Title)) return false;
            if (taskModels.Any(m => m.Title == newTaskViewModel.Title)) return false;
            return true;
        }

        private async Task SaveAsync()
        {
            var newTaskModel = newTaskViewModel.ToModel();
            newTaskModel.Urgency = taskModels.Select(m => m.Urgency).Min() - 1;
            newTaskModel.Importance = taskModels.Select(m => m.Importance).Min() - 1;
            await repository.AddTaskAsync(newTaskModel);
        }
    }
}
