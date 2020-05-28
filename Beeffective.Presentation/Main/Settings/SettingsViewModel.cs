using System.ComponentModel.Composition;
using Beeffective.Presentation.Common;

namespace Beeffective.Presentation.Main.Settings
{
    [Export]
    public class SettingsViewModel : ContentViewModel
    {
        [ImportingConstructor]
        public SettingsViewModel(Core core) : base(core)
        {
        }
    }
}