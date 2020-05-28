using System.ComponentModel.Composition;

namespace Beeffective.Presentation.Common
{
    public class ContentViewModel : CoreViewModel
    {
        [ImportingConstructor]
        public ContentViewModel(Main.Core core) : base(core)
        {
        }

        private bool isSelected;

        public bool IsSelected
        {
            get => isSelected;
            set => SetProperty(ref isSelected, value);
        }
    }
}