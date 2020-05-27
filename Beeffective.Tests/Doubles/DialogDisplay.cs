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
    }
}