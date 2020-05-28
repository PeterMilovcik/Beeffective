using System.ComponentModel.Composition;
using Beeffective.Presentation.Main.Tasks;

namespace Beeffective.Tests.Doubles
{
    [Export(typeof(INewTaskView))]
    public class NewTaskView : View, INewTaskView
    {
    }
}