using System.ComponentModel.Composition;

namespace Beeffective.Presentation.NewGoal
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
