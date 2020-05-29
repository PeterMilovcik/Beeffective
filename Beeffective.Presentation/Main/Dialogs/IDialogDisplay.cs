using System.Threading.Tasks;

namespace Beeffective.Presentation.Main.Dialogs
{
    public interface IDialogDisplay
    {
        Task ShowNewGoalDialogAsync(object dataContext);
        Task ShowNewProjectDialogAsync(object dataContext);
        Task ShowNewLabelDialogAsync(object dataContext);
        Task ShowNewTaskDialogAsync(object dataContext);
        void CloseDialog();
    }
}