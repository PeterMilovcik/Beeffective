using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Beeffective.Core.Models;
using Beeffective.Presentation.Common;
using Beeffective.Services.Repository;

namespace Beeffective.Presentation.Main
{
    [Export]
    public class Core : ViewModel
    {
        private readonly IRepositoryService repository;

        [ImportingConstructor]
        public Core(IRepositoryService repository)
        {
            this.repository = repository;
            Goals = new ObservableCollection<GoalModel>();
            Projects = new ObservableCollection<ProjectModel>();
            Labels = new ObservableCollection<LabelModel>();
            Tasks = new ObservableCollection<TaskModel>();
        }

        public ObservableCollection<GoalModel> Goals { get; }

        public ObservableCollection<ProjectModel> Projects { get; }

        public ObservableCollection<LabelModel> Labels { get; }

        public ObservableCollection<TaskModel> Tasks { get; }

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
        }

        private async Task LoadProjectsAsync()
        {
            Projects.Clear();
            var projects = (await repository.Projects.LoadAsync())
                .OrderBy(gm => gm.Title).ToList();
            projects.ForEach(projectModel => Projects.Add(projectModel));
        }

        private async Task LoadLabelsAsync()
        {
            Labels.Clear();
            var labels = (await repository.Labels.LoadAsync())
                .OrderBy(gm => gm.Title).ToList();
            labels.ForEach(labelModel => Labels.Add(labelModel));
        }

        private async Task LoadTasksAsync()
        {
            Tasks.Clear();
            var tasks = (await repository.Tasks.LoadAsync())
                .OrderBy(gm => gm.Title).ToList();
            tasks.ForEach(labelModel => Tasks.Add(labelModel));
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
