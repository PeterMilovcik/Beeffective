using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Beeffective.Core.Models;
using Beeffective.Presentation.Common;
using Beeffective.Presentation.Main.Dialogs;

namespace Beeffective.Presentation.Main.Tasks
{
    [Export]
    public class NewTaskViewModel : CoreViewModel
    {
        private readonly IDialogDisplay dialogDisplay;
        private TaskModel newTask;

        [ImportingConstructor]
        public NewTaskViewModel(Core core, IDialogDisplay dialogDisplay) : base(core)
        {
            this.dialogDisplay = dialogDisplay;
            ShowNewTaskDialogCommand = new AsyncCommand(ShowNewTaskDialogAsync);
            SaveTaskCommand = new DelegateCommand(CanSaveTask, SaveTask);
        }

        [Import]
        public INewTaskView NewTaskView { get; set; }

        public IAsyncCommand ShowNewTaskDialogCommand { get; }

        private async Task ShowNewTaskDialogAsync()
        {
            NewTask = new TaskModel();
            NewTaskView.DataContext = this;
            await dialogDisplay.ShowAsync(NewTaskView);
        }

        public TaskModel NewTask
        {
            get => newTask;
            set
            {
                if (Equals(newTask, value)) return;
                if (newTask != null) newTask.PropertyChanged -= OnGoalModelPropertyChanged;
                newTask = value;
                if (newTask != null) newTask.PropertyChanged += OnGoalModelPropertyChanged;
                SaveTaskCommand.RaiseCanExecuteChanged();
                NotifyPropertyChange();
            }
        }

        private void OnGoalModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(NewTask.Title))
            {
                SaveTaskCommand.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand SaveTaskCommand { get; }

        private bool CanSaveTask(object arg) =>
            !string.IsNullOrWhiteSpace(NewTask?.Title) &&
            !Core.Tasks.Select(taskModel => taskModel.Title).Contains(NewTask.Title);

        private void SaveTask(object obj)
        {
            Core.Tasks.Add(NewTask);
            dialogDisplay.CloseDialog();
        }
    }
}