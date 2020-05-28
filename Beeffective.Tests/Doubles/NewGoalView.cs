using System.ComponentModel.Composition;
using Beeffective.Presentation.Main.Goals;

namespace Beeffective.Tests.Doubles
{
    [Export(typeof(INewGoalView))]
    public class NewGoalView : View, INewGoalView
    {
    }
}