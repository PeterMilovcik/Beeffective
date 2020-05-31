using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Beeffective.Core.Models;
using Beeffective.Presentation.Common;
using Beeffective.Presentation.Main.Dialogs;
using Beeffective.Services.Repository;

namespace Beeffective.Presentation.Main.Projects
{
    public class NewProjectViewModel : CoreViewModel
    {
        private readonly IDialogDisplay dialogDisplay;
        private readonly IRepositoryService repository;
        private ProjectModel newProject;

        public NewProjectViewModel(Core core, IDialogDisplay dialogDisplay, IRepositoryService repository) : base(core)
        {
            this.dialogDisplay = dialogDisplay;
            this.repository = repository;
            ShowDialogCommand = new AsyncCommand(ShowDialogAsync);
            SaveCommand = new AsyncCommand(SaveAsync, CanSave);
        }

        public IAsyncCommand ShowDialogCommand { get; }

        private async Task ShowDialogAsync()
        {
            NewProject = new ProjectModel();
            await dialogDisplay.ShowNewProjectDialogAsync(this);
        }

        public AsyncCommand SaveCommand { get; }

        public ProjectModel NewProject
        {
            get => newProject;
            set
            {
                if (Equals(newProject, value)) return;
                if (newProject != null) newProject.PropertyChanged -= OnNewProjectPropertyChanged;
                newProject = value;
                if (newProject != null) newProject.PropertyChanged += OnNewProjectPropertyChanged;
                NotifyPropertyChange();
            }
        }

        private void OnNewProjectPropertyChanged(object sender, PropertyChangedEventArgs e) => 
            SaveCommand.RaiseCanExecuteChanged();

        private bool CanSave() =>
            !string.IsNullOrWhiteSpace(NewProject?.Title) &&
            NewProject.Goal != null &&
            !Core.Projects.Collection.Select(projectModel => projectModel.Title).Contains(NewProject.Title);

        private async Task SaveAsync()
        {
            var savedProject = await repository.Projects.AddAsync(NewProject);
            Core.Projects.Collection.Add(savedProject);
            dialogDisplay.CloseDialog();
        }
    }
}