using System.Threading.Tasks;

namespace Beeffective.Presentation.Main.Dialogs
{
    public interface IDialogDisplay
    {
        Task ShowAsync(object dialogView);
    }
}