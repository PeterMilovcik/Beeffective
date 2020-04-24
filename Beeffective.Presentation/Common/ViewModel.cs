using System.Threading.Tasks;

namespace Beeffective.Presentation.Common
{
    public class ViewModel : Observable
    {
        private bool isChanged;

        public bool IsChanged
        {
            get => isChanged;
            set => SetProperty(ref isChanged, value);
        }

        public virtual Task InitializeAsync() => Task.CompletedTask;
    }
}