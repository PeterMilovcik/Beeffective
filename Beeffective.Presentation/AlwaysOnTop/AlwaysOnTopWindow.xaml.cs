using System.ComponentModel.Composition;
using System.Windows.Input;

namespace Beeffective.Presentation.AlwaysOnTop
{
    [Export(typeof(IAlwaysOnTopWindow))]
    public partial class AlwaysOnTopWindow : IAlwaysOnTopWindow
    {
        public AlwaysOnTopWindow()
        {
            InitializeComponent();
        }

        private void AlwaysOnTopWindow_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) DragMove();
        }
    }
}
