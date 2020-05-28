using System.ComponentModel.Composition;
using Beeffective.Presentation.Common;

namespace Beeffective.Presentation.Main.Projects
{
    [Export]
    public class ProjectsViewModel : CoreViewModel
    {
        [ImportingConstructor]
        public ProjectsViewModel(Core core) : base(core)
        {
        }
    }
}