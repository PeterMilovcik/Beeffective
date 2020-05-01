namespace Beeffective.Presentation.Common
{
    public class ViewModel : Observable
    {
        private bool isChanged;
        private bool isBusy;
        
        public bool IsChanged
        {
            get => isChanged;
            set => SetProperty(ref isChanged, value);
        }

        public bool IsBusy
        {
            get => isBusy;
            set => SetProperty(ref isBusy, value);
        }
    }
}