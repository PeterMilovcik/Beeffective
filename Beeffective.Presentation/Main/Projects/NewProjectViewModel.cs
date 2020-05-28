using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Beeffective.Core.Models;
using Beeffective.Presentation.Common;
using Beeffective.Presentation.Main.Dialogs;
using Beeffective.Services.Repository;

namespace Beeffective.Presentation.Main.Projects
{
    [Export]
    public class NewProjectViewModel : CoreViewModel
    {
        private readonly IDialogDisplay dialogDisplay;
        private readonly IRepositoryService repository;
        private ProjectModel newProject;

        [ImportingConstructor]
        public NewProjectViewModel(Core core, IDialogDisplay dialogDisplay, IRepositoryService repository) : base(core)
        {
            this.dialogDisplay = dialogDisplay;
            this.repository = repository;
            ShowNewProjectDialogCommand = new AsyncCommand(ShowNewProjectDialog);
            SaveProjectCommand = new DelegateCommand(CanSaveProject, async obj => await SaveProjectAsync());
        }

        public IAsyncCommand ShowNewProjectDialogCommand { get; }

        private async Task ShowNewProjectDialog()
        {
            NewProject = new ProjectModel();
            NewProjectView.DataContext = this;
            await dialogDisplay.ShowAsync(NewProjectView);
        }

        [Import]
        public INewProjectView NewProjectView { get; set; }

        public DelegateCommand SaveProjectCommand { get; }

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
            SaveProjectCommand.RaiseCanExecuteChanged();

        private bool CanSaveProject(object arg) =>
            !string.IsNullOrWhiteSpace(NewProject?.Title) &&
            NewProject.Goal != null &&
            !Core.Projects.Select(projectModel => projectModel.Title).Contains(NewProject.Title);

        private async Task SaveProjectAsync()
        {
            var savedProject = await repository.Projects.AddAsync(NewProject);
            Core.Projects.Add(savedProject);
            dialogDisplay.CloseDialog();
        }
    }
}