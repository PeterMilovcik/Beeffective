using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Beeffective.Presentation.Main.Goals;
using Beeffective.Presentation.Main.Labels;
using Beeffective.Presentation.Main.Projects;
using Beeffective.Presentation.Main.Tasks;
using MaterialDesignThemes.Wpf;

namespace Beeffective.Presentation.Main.Dialogs
{
    [Export(typeof(IDialogDisplay))]
    public class DialogDisplay : IDialogDisplay
    {
        [Import]
        public INewGoalView NewGoalView { get; set; }

        [Import]
        public INewProjectView NewProjectView { get; set; }

        [Import]
        public INewLabelView NewLabelView { get; set; }

        [Import]
        public INewTaskView NewTaskView { get; set; }

        public async Task ShowNewGoalDialogAsync(object dataContext)
        {
            NewGoalView.DataContext  = dataContext;
            await DialogHost.Show(NewGoalView);
        }

        public async Task ShowNewProjectDialogAsync(object dataContext)
        {
            NewProjectView.DataContext = dataContext;
            await DialogHost.Show(NewProjectView);
        }

        public async Task ShowNewLabelDialogAsync(object dataContext)
        {
            NewLabelView.DataContext = dataContext;
            await DialogHost.Show(NewLabelView);
        }

        public async Task ShowNewTaskDialogAsync(object dataContext)
        {
            NewTaskView.DataContext = dataContext;
            await DialogHost.Show(NewTaskView);
        }

        public void CloseDialog() => 
            DialogHost.CloseDialogCommand.Execute(null,null);
    }
}