using System.ComponentModel.Composition;
using Beeffective.Presentation.Common;

namespace Beeffective.Presentation.Main.Calendar
{
    [Export]
    public class CalendarViewModel : ContentViewModel
    {
        [ImportingConstructor]
        public CalendarViewModel(Core core) : base(core)
        {
        }
    }
}
