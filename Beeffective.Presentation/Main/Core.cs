using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Beeffective.Core.Extensions;
using Beeffective.Core.Models;
using Beeffective.Presentation.Common;
using Beeffective.Presentation.Main.Dialogs;
using Beeffective.Presentation.Main.Goals;
using Beeffective.Presentation.Main.Labels;
using Beeffective.Presentation.Main.Projects;
using Beeffective.Presentation.Main.Tasks;
using Beeffective.Services.Repository;

namespace Beeffective.Presentation.Main
{
    [Export]
    public class Core : ViewModel
    {
        private readonly IRepositoryService repository;
        private GoalModel selectedGoal;
        private ProjectModel selectedProject;
        private LabelModel selectedLabel;
        private TaskModel selectedTask;
        private ObservableCollection<ProjectModel> selectedProjects;
        private ObservableCollection<LabelModel> selectedLabels;
        private ObservableCollection<TaskModel> selectedTasks;
        private ObservableCollection<GoalModel> selectedGoals;

        [ImportingConstructor]
        public Core(IRepositoryService repository, IDialogDisplay dialogDisplay)
        {
            this.repository = repository;
            GoalsCollection = new ObservableCollection<GoalModel>();
            GoalsCollection.CollectionChanged += OnGoalsCollectionChanged;
            ProjectsCollection = new ObservableCollection<ProjectModel>();
            ProjectsCollection.CollectionChanged += OnProjectsCollectionChanged;
            LabelsCollection = new ObservableCollection<LabelModel>();
            LabelsCollection.CollectionChanged += OnLabelsCollectionChanged;
            TasksCollection = new ObservableCollection<TaskModel>();
            TasksCollection.CollectionChanged += OnTasksCollectionChanged;
            NewProject = new NewProjectViewModel(this, dialogDisplay, repository);
            NewLabel = new NewLabelViewModel(this, dialogDisplay, repository);
            NewTask = new NewTaskViewModel(this, dialogDisplay, repository);
            SelectAllGoalsCommand = new DelegateCommand((obj) => SelectedGoal = null);
            SelectAllProjectsCommand = new DelegateCommand((obj) => SelectedProject = null);
            SelectAllLabelsCommand = new DelegateCommand((obj) => SelectedLabel = null);
            SelectAllTasksCommand = new DelegateCommand((obj) => SelectedTask = null);
            Goals = new GoalsViewModel(this, dialogDisplay, repository);
        }

        public GoalsViewModel Goals { get; }

        public NewProjectViewModel NewProject { get; }

        public NewLabelViewModel NewLabel { get; }

        public NewTaskViewModel NewTask { get; }

        public ObservableCollection<GoalModel> GoalsCollection { get; }

        private void OnGoalsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            SelectedGoal = null;
            SelectedGoals = GoalsCollection;
        }

        public ObservableCollection<GoalModel> SelectedGoals
        {
            get => selectedGoals;
            set => SetProperty(ref selectedGoals, value).IfTrue(() =>
                SelectedProjects = new ObservableCollection<ProjectModel>(
                    ProjectsCollection.Where(p => SelectedGoals.Contains(p.Goal))));
        }

        public GoalModel SelectedGoal
        {
            get => selectedGoal;
            set => SetProperty(ref selectedGoal, value).IfTrue(() =>
            {
                SelectedProjects = SelectedGoal != null
                    ? new ObservableCollection<ProjectModel>(
                        ProjectsCollection.Where(p => p.Goal == SelectedGoal))
                    : ProjectsCollection;
            });
        }

        public DelegateCommand SelectAllGoalsCommand { get; }

        public ObservableCollection<ProjectModel> ProjectsCollection { get; }

        private void OnProjectsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            SelectedProject = null;
            SelectedProjects = ProjectsCollection;
        }

        public ObservableCollection<ProjectModel> SelectedProjects
        {
            get => selectedProjects;
            set => SetProperty(ref selectedProjects, value).IfTrue(() =>
                SelectedTasks = new ObservableCollection<TaskModel>(
                    TasksCollection.Where(t => SelectedProjects.Contains(t.Project))));
        }

        public ProjectModel SelectedProject
        {
            get => selectedProject;
            set => SetProperty(ref selectedProject, value).IfTrue(() =>
            {
                SelectedTasks = SelectedProject != null
                    ? new ObservableCollection<TaskModel>(
                        TasksCollection.Where(p => p.Project == SelectedProject))
                    : TasksCollection;
            });
        }

        public DelegateCommand SelectAllProjectsCommand { get; }

        public ObservableCollection<LabelModel> LabelsCollection { get; }

        private void OnLabelsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            SelectedLabel = null;
            SelectedLabels = LabelsCollection;
        }

        public ObservableCollection<LabelModel> SelectedLabels
        {
            get => selectedLabels;
            set => SetProperty(ref selectedLabels, value);
        }

        public LabelModel SelectedLabel
        {
            get => selectedLabel;
            set => SetProperty(ref selectedLabel, value).IfTrue(() =>
            {
                SelectedTasks = SelectedLabel != null
                    ? new ObservableCollection<TaskModel>(
                        TasksCollection.Where(p => p.Labels.Contains(SelectedLabel)))
                    : TasksCollection;
            });
        }

        public DelegateCommand SelectAllLabelsCommand { get; }

        public ObservableCollection<TaskModel> TasksCollection { get; }

        private void OnTasksCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            SelectedTask = null;
            SelectedTasks = TasksCollection;
        }

        public ObservableCollection<TaskModel> SelectedTasks
        {
            get => selectedTasks;
            set => SetProperty(ref selectedTasks, value);
        }

        public TaskModel SelectedTask
        {
            get => selectedTask;
            set => SetProperty(ref selectedTask, value).IfTrue(() => NotifyPropertyChange(nameof(IsTaskSelected)));
        }

        public bool IsTaskSelected => SelectedTask != null;

        public DelegateCommand SelectAllTasksCommand { get; }

        public async Task LoadAsync()
        {
            await LoadGoalsAsync();
            await LoadProjectsAsync();
            await LoadLabelsAsync();
            await LoadTasksAsync();
        }

        private async Task LoadGoalsAsync()
        {
            GoalsCollection.Clear();
            var goals = (await repository.Goals.LoadAsync())
                .OrderBy(gm => gm.Title).ToList();
            goals.ForEach(goalModel => GoalsCollection.Add(goalModel));
            SelectedGoals = GoalsCollection;
        }

        private async Task LoadProjectsAsync()
        {
            ProjectsCollection.Clear();
            var projects = (await repository.Projects.LoadAsync())
                .OrderBy(gm => gm.Title).ToList();
            projects.ForEach(projectModel => ProjectsCollection.Add(projectModel));
            SelectedProjects = ProjectsCollection;
        }

        private async Task LoadLabelsAsync()
        {
            LabelsCollection.Clear();
            var labels = (await repository.Labels.LoadAsync())
                .OrderBy(gm => gm.Title).ToList();
            labels.ForEach(labelModel => LabelsCollection.Add(labelModel));
            SelectedLabels = LabelsCollection;
        }

        private async Task LoadTasksAsync()
        {
            TasksCollection.Clear();
            var tasks = (await repository.Tasks.LoadAsync())
                .OrderBy(gm => gm.Title).ToList();
            tasks.ForEach(labelModel => TasksCollection.Add(labelModel));
            SelectedTasks = TasksCollection;
        }

        public async Task SaveAsync()
        {
            await repository.Goals.SaveAsync(GoalsCollection.ToList());
            await repository.Projects.SaveAsync(ProjectsCollection.ToList());
            await repository.Labels.SaveAsync(LabelsCollection.ToList());
            await repository.Tasks.SaveAsync(TasksCollection.ToList());
        }
    }
}
