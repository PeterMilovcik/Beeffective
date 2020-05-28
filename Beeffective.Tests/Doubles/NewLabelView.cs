using System.ComponentModel.Composition;
using Beeffective.Presentation.Main.Labels;

namespace Beeffective.Tests.Doubles
{
    [Export(typeof(INewLabelView))]
    public class NewLabelView : View, INewLabelView
    {
    }
}