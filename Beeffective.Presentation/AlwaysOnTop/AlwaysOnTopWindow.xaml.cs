using System;
using System.ComponentModel.Composition;
using System.Windows;
using Beeffective.Presentation.Main;

namespace Beeffective.Presentation.AlwaysOnTop
{
    [Export(typeof(IAlwaysOnTopWindow))]
    public partial class AlwaysOnTopWindow : IAlwaysOnTopWindow
    {
        private const int LeftOffset = -200;

        [ImportingConstructor]
        public AlwaysOnTopWindow(MainViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            UpdatePosition();
        }

        private void AlwaysOnTopWindow_OnSizeChanged(object sender, SizeChangedEventArgs e) => 
            UpdatePosition();

        private void UpdatePosition()
        {
            Left = double.IsNaN(Width)
                ? SystemParameters.WorkArea.Width + LeftOffset
                : SystemParameters.WorkArea.Width + LeftOffset - Width;
            Top = 0;
        }
    }
}
