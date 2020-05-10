using System.ComponentModel.Composition;
using Beeffective.Presentation.Common;
using Beeffective.Presentation.Main.Priority;

namespace Beeffective.Presentation.Main.Settings
{
    [Export]
    public class SettingsViewModel : ContentViewModel
    {
        [ImportingConstructor]
        public SettingsViewModel(PriorityObservableCollection tasks) : base(tasks)
        {
        }
    }
}