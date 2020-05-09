namespace Beeffective.Core.Models
{
    public class Changeable : Workable
    {
        private bool isChanged;

        public bool IsChanged
        {
            get => isChanged;
            set => SetProperty(ref isChanged, value);
        }
    }
}