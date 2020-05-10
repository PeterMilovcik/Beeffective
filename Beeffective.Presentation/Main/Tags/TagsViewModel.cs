using System.ComponentModel.Composition;
using Beeffective.Presentation.Common;
using Beeffective.Presentation.Main.Priority;

namespace Beeffective.Presentation.Main.Tags
{
    [Export]
    public class TagsViewModel : ContentViewModel
    {
        [ImportingConstructor]
        public TagsViewModel(PriorityObservableCollection tasks) : base(tasks)
        {
        }
    }
}
