using System.ComponentModel.Composition;

namespace Beeffective.Presentation.Main
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
