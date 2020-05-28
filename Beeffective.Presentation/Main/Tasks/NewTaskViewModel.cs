using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Beeffective.Core.Extensions;
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
        private ObservableCollection<GoalModel> goals;
        private GoalModel goal;
        private ObservableCollection<ProjectModel> projects;
        private ProjectModel project;

        [ImportingConstructor]
        public NewTaskViewModel(Core core, IDialogDisplay dialogDisplay) : base(core)
        {
            this.dialogDisplay = dialogDisplay;
            ShowNewTaskDialogCommand = new AsyncCommand(ShowNewTaskDialogAsync);
            SaveTaskCommand = new DelegateCommand(CanSaveTask, SaveTask);
            Projects = new ObservableCollection<ProjectModel>();
        }

        [Import]
        public INewTaskView NewTaskView { get; set; }

        public IAsyncCommand ShowNewTaskDialogCommand { get; }

        public ObservableCollection<GoalModel> Goals
        {
            get => goals;
            set => SetProperty(ref goals, value);
        }

        public GoalModel Goal
        {
            get => goal;
            set => SetProperty(ref goal, value).IfTrue(() => 
                    Projects = new ObservableCollection<ProjectModel>(
                        Core.Projects.Where(p => p.Goal == Goal)));
        }

        public ObservableCollection<ProjectModel> Projects
        {
            get => projects;
            set => SetProperty(ref projects, value);
        }

        public ProjectModel Project
        {
            get => project;
            set => SetProperty(ref project, value);
        }

        private async Task ShowNewTaskDialogAsync()
        {
            Goals = Core.Goals;
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
            NewTask.Project = Project;
            Core.Tasks.Add(NewTask);
            dialogDisplay.CloseDialog();
        }
    }
}