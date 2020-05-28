using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Beeffective.Core.Models;
using Beeffective.Presentation.Common;
using Beeffective.Presentation.Main.Dialogs;
using Beeffective.Presentation.Main.Priority;
using Beeffective.Presentation.NewProject;

namespace Beeffective.Presentation.Main.Projects
{
    [Export]
    public class NewProjectViewModel : TaskCollectionViewModel
    {
        private readonly IDialogDisplay dialogDisplay;
        private ProjectModel newProject;

        [ImportingConstructor]
        public NewProjectViewModel(PriorityObservableCollection tasks, IDialogDisplay dialogDisplay) : base(tasks)
        {
            this.dialogDisplay = dialogDisplay;
            ShowNewProjectDialogCommand = new AsyncCommand(ShowNewProjectDialog);
            SaveProjectCommand = new DelegateCommand(CanSaveProject, SaveProject);
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
            !Tasks.Projects.Select(projectModel => projectModel.Title).Contains(NewProject.Title);

        private void SaveProject(object obj)
        {
            Tasks.Projects.Add(NewProject);
            dialogDisplay.CloseDialog();
        }
    }
}