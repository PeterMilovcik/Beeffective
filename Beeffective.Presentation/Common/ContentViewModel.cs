namespace Beeffective.Presentation.Common
{
    public class ContentViewModel : TaskCollectionViewModel
    {
        private bool isSelected;

        public bool IsSelected
        {
            get => isSelected;
            set => SetProperty(ref isSelected, value);
        }
    }
}