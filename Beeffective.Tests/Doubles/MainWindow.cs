using System.ComponentModel.Composition;
using Beeffective.Presentation.Main;

namespace Beeffective.Tests.Doubles
{
    [Export(typeof(IMainView))]
    public class MainWindow : Window, IMainView
    {
    }
}