using System.ComponentModel.Composition;
using Beeffective.Presentation.Main.Priority;

namespace Beeffective.Presentation.Common
{
    public class ContentViewModel : TaskCollectionViewModel
    {
        [ImportingConstructor]
        public ContentViewModel(PriorityObservableCollection tasks) : base(tasks)
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