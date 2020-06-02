using System.Threading.Tasks;

namespace Beeffective.Presentation.Main.Dialogs
{
    public interface IDialogDisplay
    {
        Task ShowNewLabelDialogAsync(object dataContext);
        Task ShowNewTaskDialogAsync(object dataContext);
        void CloseDialog();
    }
}