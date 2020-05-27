using System.ComponentModel.Composition;
using System.Threading.Tasks;
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

        [ImportingConstructor]
        public NewProjectViewModel(PriorityObservableCollection tasks, IDialogDisplay dialogDisplay) : base(tasks)
        {
            this.dialogDisplay = dialogDisplay;
            ShowNewProjectDialogCommand = new AsyncCommand(ShowNewProjectDialog);
        }

        public IAsyncCommand ShowNewProjectDialogCommand { get; }
        
        [Import]
        public INewProjectView NewProjectView { get; set; }

        private async Task ShowNewProjectDialog()
        {
            NewProjectView.DataContext = this;
            await dialogDisplay.ShowAsync(NewProjectView);
        }
    }
}