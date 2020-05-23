using System.ComponentModel.Composition;
using Beeffective.Presentation.AlwaysOnTop;

namespace Beeffective.Tests.Doubles
{
    [Export(typeof(IAlwaysOnTopWindow))]
    public class AlwaysOnTopWindow : Window, IAlwaysOnTopWindow
    {
    }
}