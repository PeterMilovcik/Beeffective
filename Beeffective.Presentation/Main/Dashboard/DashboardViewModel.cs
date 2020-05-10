using System.ComponentModel.Composition;
using Beeffective.Presentation.Common;
using Beeffective.Presentation.Main.Priority;

namespace Beeffective.Presentation.Main.Dashboard
{
    [Export]
    public class DashboardViewModel : ContentViewModel
    {
        [ImportingConstructor]
        public DashboardViewModel(PriorityObservableCollection tasks) : base(tasks)
        {
        }
    }
}
