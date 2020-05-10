using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Beeffective.Core.Models;
using Beeffective.Presentation.Common;
using Beeffective.Presentation.Main.Priority;
using Beeffective.Presentation.Main.Tasks;

namespace Beeffective.Presentation.Main.New
{
    [Export]
    public class NewViewModel : ContentViewModel
    {
        private TaskViewModel newTaskViewModel;

        [ImportingConstructor]
        public NewViewModel(PriorityObservableCollection tasks) : base(tasks)
        {
            Tasks.CollectionChanged += (sender, args) => Update();
            SaveCommand = new DelegateCommand(
                o => CanSave(),o => Save());
        }

        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();
            Update();
        }

        private void Update()
        {
            NewTask = new TaskViewModel(new TaskModel());
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
            taskViewModel.Model.PropertyChanged += OnTaskViewModelPropertyChanged;

        private void UnsubscribeFrom(TaskViewModel taskViewModel) => 
            taskViewModel.Model.PropertyChanged -= OnTaskViewModelPropertyChanged;

        private void OnTaskViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(NewTask.Model.Title))
            {
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand SaveCommand { get; }

        private bool CanSave()
        {
            if (NewTask == null) return false;
            if (string.IsNullOrWhiteSpace(NewTask.Model.Title)) return false;
            if (Tasks.Any(m => m.Model.Title == NewTask.Model.Title)) return false;
            return true;
        }

        private void Save()
        {
            foreach (var taskViewModel in Tasks)
            {
                taskViewModel.Model.Urgency++;
                taskViewModel.Model.Importance++;
            }

            Tasks.Add(NewTask);
            Update();
        }
    }
}
