using System.Threading.Tasks;

namespace Beeffective.Presentation.Common
{
    public class ContentViewModel : ViewModel
    {
        private bool isSelected;

        public bool IsSelected
        {
            get => isSelected;
            set => SetProperty(ref isSelected, value);
        }

        public virtual Task InitializeAsync() => Task.CompletedTask;
    }
}