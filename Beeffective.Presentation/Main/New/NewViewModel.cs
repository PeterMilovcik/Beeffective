using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class NewViewModel : ContentViewModel
    {
        private readonly IRepositoryService repository;
        private List<TaskModel> taskModels;
        private TaskViewModel newTaskViewModel;
        private ObservableCollection<string> goals;
        private ObservableCollection<string> tags;

        [ImportingConstructor]
        public NewViewModel(IRepositoryService repository)
        {
            this.repository = repository;
            SaveCommand = new DelegateCommand(
                o => CanSave(),async o => await SaveAsync());
        }

        public override async Task InitializeAsync()
        {
            try
            {
                IsBusy = true;
                await LoadTaskAsync();
                NewTask = new TaskViewModel();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task LoadTaskAsync()
        {
            taskModels = await repository.LoadTaskAsync();
            Goals = new ObservableCollection<string>(
                taskModels.Where(t => !string.IsNullOrWhiteSpace(t.Goal)).Select(t => t.Goal).Distinct());
            Tags = new ObservableCollection<string>(ParseTags());
        }

        private IEnumerable<string> ParseTags()
        {
            var result = new List<string>();
            foreach (var taskModel in taskModels.Where(t => !string.IsNullOrWhiteSpace(t.Tags)))
            {
                result.AddRange(taskModel.Tags.Trim().Split(" "));
            }

            return result.Distinct();
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

        public ObservableCollection<string> Goals
        {
            get => goals;
            set => SetProperty(ref goals, value);
        }

        public ObservableCollection<string> Tags
        {
            get => tags;
            set => SetProperty(ref tags, value);
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
            try
            {
                IsBusy = true;
                var newTaskModel = NewTask.ToModel();

                foreach (var taskModel in taskModels)
                {
                    taskModel.Urgency++;
                    taskModel.Importance++;
                    await repository.UpdateTaskAsync(taskModel);
                }

                await repository.AddTaskAsync(newTaskModel);
                await InitializeAsync();
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
