using System.ComponentModel.Composition;

namespace Beeffective.Presentation.Main.Goals
{
    [Export(typeof(INewGoalView))]
    public partial class NewGoalView : INewGoalView
    {
        public NewGoalView()
        {
            InitializeComponent();
        }
    }
}
