using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Beeffective.Presentation.Main.Dialogs;

namespace Beeffective.Tests.Doubles
{
    [Export(typeof(IDialogDisplay))]
    public class DialogDisplay : IDialogDisplay
    {
        public bool IsDialogShown { get; set; }

        public Task ShowAsync(object dialogView) => 
            Task.Run(() => { IsDialogShown = true; });

        public void CloseDialog() => 
            IsDialogShown = false;

        public async Task ShowNewProjectDialogAsync(object dataContext) => 
            await ShowAsync(new NewProjectView {DataContext = dataContext});

        public async Task ShowNewLabelDialogAsync(object dataContext) => 
            await ShowAsync(new NewLabelView { DataContext = dataContext });

        public async Task ShowNewTaskDialogAsync(object dataContext) => 
            await ShowAsync(new NewTaskView { DataContext = dataContext });
    }
}