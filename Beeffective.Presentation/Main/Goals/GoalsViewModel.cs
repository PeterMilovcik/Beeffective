using System.ComponentModel.Composition;
using Beeffective.Presentation.Common;
using Beeffective.Presentation.Main.Priority;

namespace Beeffective.Presentation.Main.Goals
{
    [Export]
    public class GoalsViewModel : ContentViewModel
    {
        [ImportingConstructor]
        public GoalsViewModel(PriorityObservableCollection tasks) : base(tasks)
        {
        }
    }
}
