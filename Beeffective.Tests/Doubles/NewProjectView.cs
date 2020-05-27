using System.ComponentModel.Composition;
using Beeffective.Presentation.NewProject;

namespace Beeffective.Tests.Doubles
{
    [Export(typeof(INewProjectView))]
    public class NewProjectView : View, INewProjectView
    {

    }
}