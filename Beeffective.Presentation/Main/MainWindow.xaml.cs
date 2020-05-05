using System.ComponentModel;
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

        protected override void OnClosing(CancelEventArgs e)
        {
            if (DataContext is MainViewModel viewModel)
            {
                viewModel.Close();
            }
            base.OnClosing(e);
        }
    }
}
