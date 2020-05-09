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

        public override Task InitializeAsync()
        {
            try
            {
                IsBusy = true;

                Update();

                return base.InitializeAsync();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void Update()
        {
            Goals = new ObservableCollection<string>(GetGoals());
            Tags = new ObservableCollection<string>(GetTags());
            NewTask = new TaskViewModel(new TaskModel());
        }

        private IEnumerable<string> GetGoals() => Tasks
            .Where(t => !string.IsNullOrWhiteSpace(t.Model.Goal))
            .Select(t => t.Model.Goal).Distinct();

        private IEnumerable<string> GetTags()
        {
            var result = new List<string>();
            foreach (var taskModel in Tasks.Where(t => !string.IsNullOrWhiteSpace(t.Model.Tags)))
            {
                result.AddRange(taskModel.Model.Tags.Trim().Split(" "));
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
            if (e.PropertyName == nameof(newTaskViewModel.Model.Title))
            {
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand SaveCommand { get; }

        private bool CanSave()
        {
            if (newTaskViewModel == null) return false;
            if (string.IsNullOrWhiteSpace(newTaskViewModel.Model.Title)) return false;
            if (Tasks.Any(m => m.Model.Title == newTaskViewModel.Model.Title)) return false;
            return true;
        }

        private async Task SaveAsync()
        {
            try
            {
                IsBusy = true;
                var newTaskModel = NewTask.Model;

                foreach (var taskViewModel in Tasks)
                {
                    taskViewModel.Model.Urgency++;
                    taskViewModel.Model.Importance++;
                    await repository.UpdateTaskAsync(taskViewModel.Model);
                }

                await repository.AddTaskAsync(newTaskModel);
                await Tasks.LoadAsync();

                Update();
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
