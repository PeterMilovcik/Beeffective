using System.ComponentModel.Composition;

namespace Beeffective.Presentation.Main.Tasks
{
    [Export(typeof(INewTaskView))]
    public partial class NewTaskView : INewTaskView
    {
        public NewTaskView()
        {
            InitializeComponent();
        }
    }
}
