using System.ComponentModel.Composition;

namespace Beeffective.Presentation.NewLabel
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
