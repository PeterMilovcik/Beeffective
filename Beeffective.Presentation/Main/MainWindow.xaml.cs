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

        protected override async void OnClosing(CancelEventArgs e)
        {
            if (DataContext is MainViewModel viewModel)
            {
                await viewModel.Close();
            }
            base.OnClosing(e);
        }
    }
}
