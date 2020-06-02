using System.ComponentModel.Composition;
using MaterialDesignThemes.Wpf;

namespace Beeffective.Presentation.Main.Dialogs
{
    [Export(typeof(IDialogDisplay))]
    public class DialogDisplay : IDialogDisplay
    {
        public void CloseDialog() => 
            DialogHost.CloseDialogCommand.Execute(null,null);
    }
}