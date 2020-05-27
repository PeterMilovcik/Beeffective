using System.ComponentModel.Composition;
using Beeffective.Presentation.Common;
using Beeffective.Presentation.Main.Priority;

namespace Beeffective.Presentation.Main.Projects
{
    [Export]
    public class ProjectsViewModel : TaskCollectionViewModel
    {
        [ImportingConstructor]
        public ProjectsViewModel(PriorityObservableCollection tasks) : base(tasks)
        {
        }
    }
}