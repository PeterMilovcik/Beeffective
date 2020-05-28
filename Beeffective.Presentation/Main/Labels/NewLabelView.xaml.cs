using System.ComponentModel.Composition;

namespace Beeffective.Presentation.Main.Labels
{
    [Export(typeof(INewLabelView))]
    public partial class NewLabelView : INewLabelView
    {
        public NewLabelView()
        {
            InitializeComponent();
        }
    }
}
