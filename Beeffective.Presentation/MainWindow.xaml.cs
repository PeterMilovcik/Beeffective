using System.ComponentModel.Composition;
using Beeffective.Presentation.Main;

namespace Beeffective.Presentation
{
    [Export(typeof(IMainView))]
    public partial class MainWindow : IMainView
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
