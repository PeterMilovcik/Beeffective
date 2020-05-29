using Beeffective.Core.Models;
using Beeffective.Presentation.Common;

namespace Beeffective.Presentation.Main.Tasks
{
    public class LabelViewModel : ViewModel
    {
        private bool isSelected;

        public LabelViewModel(LabelModel model)
        {
            Model = model;
            DeselectCommand = new DelegateCommand(obj => IsSelected = false);
        }

        public LabelModel Model { get; }

        public bool IsSelected
        {
            get => isSelected;
            set => SetProperty(ref isSelected, value);
        }

        public DelegateCommand DeselectCommand { get; }
    }
}