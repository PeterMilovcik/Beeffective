using System;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Input;

namespace Beeffective.Presentation.AlwaysOnTop
{
    [Export(typeof(IAlwaysOnTopWindow))]
    public partial class AlwaysOnTopWindow : IAlwaysOnTopWindow
    {
        private const int LeftOffset = -200;

        public AlwaysOnTopWindow()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            UpdatePosition();
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
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

        private void AlwaysOnTopWindow_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) DragMove();
        }
    }
}
