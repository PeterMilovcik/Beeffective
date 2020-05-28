using System.ComponentModel.Composition;
using Beeffective.Presentation.Main.Projects;

namespace Beeffective.Tests.Doubles
{
    [Export(typeof(INewProjectView))]
    public class NewProjectView : View, INewProjectView
    {
    }
}