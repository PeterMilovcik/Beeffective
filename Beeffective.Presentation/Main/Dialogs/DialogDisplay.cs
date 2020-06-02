using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Beeffective.Presentation.Main.Tasks;
using MaterialDesignThemes.Wpf;

namespace Beeffective.Presentation.Main.Dialogs
{
    [Export(typeof(IDialogDisplay))]
    public class DialogDisplay : IDialogDisplay
    {
        [Import]
        public INewTaskView NewTaskView { get; set; }

        public async Task ShowNewTaskDialogAsync(object dataContext)
        {
            NewTaskView.DataContext = dataContext;
            await DialogHost.Show(NewTaskView);
        }

        public void CloseDialog() => 
            DialogHost.CloseDialogCommand.Execute(null,null);
    }
}