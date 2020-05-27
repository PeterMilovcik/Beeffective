using System.ComponentModel.Composition;
using Beeffective.Presentation.NewGoal;

namespace Beeffective.Tests.Doubles
{
    [Export(typeof(INewGoalView))]
    public class NewGoalView : View, INewGoalView
    {
    }
}