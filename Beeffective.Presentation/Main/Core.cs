﻿using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Beeffective.Core.Extensions;
using Beeffective.Core.Models;
using Beeffective.Presentation.Common;
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
        public Core(IRepositoryService repository)
        {
            this.repository = repository;
            Goals = new ObservableCollection<GoalModel>();
            Goals.CollectionChanged += OnGoalsCollectionChanged;
            Projects = new ObservableCollection<ProjectModel>();
            Projects.CollectionChanged += OnProjectsCollectionChanged;
            Labels = new ObservableCollection<LabelModel>();
            Labels.CollectionChanged += OnLabelsCollectionChanged;
            Tasks = new ObservableCollection<TaskModel>();
            Tasks.CollectionChanged += OnTasksCollectionChanged;
        }

        public ObservableCollection<GoalModel> Goals { get; }

        private void OnGoalsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            SelectedGoal = null;
            SelectedGoals = Goals;
        }

        public ObservableCollection<GoalModel> SelectedGoals
        {
            get => selectedGoals;
            set => SetProperty(ref selectedGoals, value);
        }

        public GoalModel SelectedGoal
        {
            get => selectedGoal;
            set => SetProperty(ref selectedGoal, value).IfTrue(() =>
            {
                SelectedProjects = SelectedGoal != null
                    ? new ObservableCollection<ProjectModel>(
                        Projects.Where(p => p.Goal == SelectedGoal))
                    : Projects;
            });
        }

        public ObservableCollection<ProjectModel> Projects { get; }

        private void OnProjectsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            SelectedProject = null;
            SelectedProjects = Projects;
        }

        public ObservableCollection<ProjectModel> SelectedProjects
        {
            get => selectedProjects;
            set => SetProperty(ref selectedProjects, value);
        }

        public ProjectModel SelectedProject
        {
            get => selectedProject;
            set => SetProperty(ref selectedProject, value).IfTrue(() =>
            {
                SelectedTasks = SelectedProject != null
                    ? new ObservableCollection<TaskModel>(
                        Tasks.Where(p => p.Project == SelectedProject))
                    : Tasks;
            });
        }

        public ObservableCollection<LabelModel> Labels { get; }

        private void OnLabelsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            SelectedLabel = null;
            SelectedLabels = Labels;
        }

        public ObservableCollection<LabelModel> SelectedLabels
        {
            get => selectedLabels;
            set => SetProperty(ref selectedLabels, value);
        }

        public LabelModel SelectedLabel
        {
            get => selectedLabel;
            set => SetProperty(ref selectedLabel, value);
        }

        public ObservableCollection<TaskModel> Tasks { get; }

        private void OnTasksCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            SelectedTask = null;
            SelectedTasks = Tasks;
        }

        public ObservableCollection<TaskModel> SelectedTasks
        {
            get => selectedTasks;
            set => SetProperty(ref selectedTasks, value);
        }

        public TaskModel SelectedTask
        {
            get => selectedTask;
            set => SetProperty(ref selectedTask, value);
        }

        public async Task LoadAsync()
        {
            await LoadGoalsAsync();
            await LoadProjectsAsync();
            await LoadLabelsAsync();
            await LoadTasksAsync();
        }

        private async Task LoadGoalsAsync()
        {
            Goals.Clear();
            var goals = (await repository.Goals.LoadAsync())
                .OrderBy(gm => gm.Title).ToList();
            goals.ForEach(goalModel => Goals.Add(goalModel));
            SelectedGoals = Goals;
        }

        private async Task LoadProjectsAsync()
        {
            Projects.Clear();
            var projects = (await repository.Projects.LoadAsync())
                .OrderBy(gm => gm.Title).ToList();
            projects.ForEach(projectModel => Projects.Add(projectModel));
            SelectedProjects = Projects;
        }

        private async Task LoadLabelsAsync()
        {
            Labels.Clear();
            var labels = (await repository.Labels.LoadAsync())
                .OrderBy(gm => gm.Title).ToList();
            labels.ForEach(labelModel => Labels.Add(labelModel));
            SelectedLabels = Labels;
        }

        private async Task LoadTasksAsync()
        {
            Tasks.Clear();
            var tasks = (await repository.Tasks.LoadAsync())
                .OrderBy(gm => gm.Title).ToList();
            tasks.ForEach(labelModel => Tasks.Add(labelModel));
            SelectedTasks = Tasks;
        }

        public async Task SaveAsync()
        {
            await repository.Goals.SaveAsync(Goals.ToList());
            await repository.Projects.SaveAsync(Projects.ToList());
            await repository.Labels.SaveAsync(Labels.ToList());
            await repository.Tasks.SaveAsync(Tasks.ToList());
        }
    }
}