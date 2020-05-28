using System.ComponentModel.Composition;

namespace Beeffective.Presentation.Main.Projects
{
    [Export(typeof(INewProjectView))]
    public partial class NewProjectView : INewProjectView
    {
        public NewProjectView()
        {
            InitializeComponent();
        }
    }
}
