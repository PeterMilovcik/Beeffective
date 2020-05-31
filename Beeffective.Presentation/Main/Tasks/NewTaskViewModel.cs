using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Beeffective.Core.Extensions;
using Beeffective.Core.Models;
using Beeffective.Presentation.Common;
using Beeffective.Presentation.Main.Dialogs;
using Beeffective.Services.Repository;

namespace Beeffective.Presentation.Main.Tasks
{
    public class NewTaskViewModel : CoreViewModel
    {
        private readonly IDialogDisplay dialogDisplay;
        private readonly IRepositoryService repository;
        private TaskModel newTask;
        private ObservableCollection<GoalModel> goals;
        private GoalModel goal;
        private ObservableCollection<ProjectModel> projects;
        private ProjectModel project;
        private LabelModel labelToAdd;
        private ObservableCollection<LabelViewModel> labels;

        public NewTaskViewModel(Core core, IDialogDisplay dialogDisplay, IRepositoryService repository) : base(core)
        {
            this.dialogDisplay = dialogDisplay;
            this.repository = repository;
            ShowNewTaskDialogCommand = new AsyncCommand(ShowNewTaskDialogAsync);
            SaveTaskCommand = new DelegateCommand(CanSaveTask, async obj => await SaveTaskAsync());
            Projects = new ObservableCollection<ProjectModel>();
        }

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
                        Core.Projects.Collection.Where(p => p.Goal == Goal)));
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

        public ObservableCollection<LabelViewModel> Labels
        {
            get => labels;
            set
            {
                if (Equals(labels, value)) return;
                labels?.ToList().ForEach(label => label.PropertyChanged -= OnLabelPropertyChanged);
                labels = value;
                labels?.ToList().ForEach(label => label.PropertyChanged += OnLabelPropertyChanged);
                NotifyPropertyChange();
            }
        }

        private void OnLabelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (NewTask == null) return;
            if (e.PropertyName == nameof(LabelViewModel.IsSelected))
            {
                NewTask.Labels.Clear();
                Labels.Where(label => label.IsSelected).ToList().ForEach(label => NewTask.Labels.Add(label.Model));
            }
        }

        private async Task ShowNewTaskDialogAsync()
        {
            Goals = Core.Goals.Collection;
            Labels = new ObservableCollection<LabelViewModel>(
                Core.Labels.Collection.Select(l => new LabelViewModel(l)));
            NewTask = new TaskModel();
            await dialogDisplay.ShowNewTaskDialogAsync(this);
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

        public LabelModel LabelToAdd
        {
            get => labelToAdd;
            set => SetProperty(ref labelToAdd, value).IfTrue(() =>
            {
                if (NewTask == null) return;
                if (NewTask.Labels.Contains(LabelToAdd)) return;
                NewTask.Labels.Add(LabelToAdd);
            });
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
            !Core.Tasks.Collection.Select(taskModel => taskModel.Title).Contains(NewTask.Title);

        private async Task SaveTaskAsync()
        {
            NewTask.Project = Project;
            var savedTask = await repository.Tasks.AddAsync(NewTask);
            Core.Tasks.Collection.Add(savedTask);
            dialogDisplay.CloseDialog();
        }
    }
}