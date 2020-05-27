using System.ComponentModel.Composition;
using System.Threading.Tasks;
using MaterialDesignThemes.Wpf;

namespace Beeffective.Presentation.Main.Dialogs
{
    [Export(typeof(IDialogDisplay))]
    public class DialogDisplay : IDialogDisplay
    {
        public Task ShowAsync(object dialogView) => DialogHost.Show(dialogView);
    }
}