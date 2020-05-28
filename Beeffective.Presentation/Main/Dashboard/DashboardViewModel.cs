using System.ComponentModel.Composition;
using Beeffective.Presentation.Common;

namespace Beeffective.Presentation.Main.Dashboard
{
    [Export]
    public class DashboardViewModel : ContentViewModel
    {
        [ImportingConstructor]
        public DashboardViewModel(Core core) : base(core)
        {
        }
    }
}
