namespace Beeffective.Core.Models
{
    public class Workable : Observable
    {
        private bool isBusy;

        public bool IsBusy
        {
            get => isBusy;
            set => SetProperty(ref isBusy, value);
        }
    }
}