using System.ComponentModel.Composition;
using Beeffective.Presentation.Common;
using Beeffective.Presentation.Main.Priority;

namespace Beeffective.Presentation.Main.Calendar
{
    [Export]
    public class CalendarViewModel : ContentViewModel
    {
        [ImportingConstructor]
        public CalendarViewModel(PriorityObservableCollection tasks) : base(tasks)
        {
        }
    }
}
