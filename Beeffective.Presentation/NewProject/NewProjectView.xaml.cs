using System.ComponentModel.Composition;

namespace Beeffective.Presentation.NewProject
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
