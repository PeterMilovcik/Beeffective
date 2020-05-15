using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Beeffective.Core
{
    public class Observable : INotifyPropertyChanged
    {
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value)) return false;
            storage = value;
            NotifyPropertyChange(propertyName);
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void NotifyPropertyChange([CallerMemberName] string propertyName = null) => 
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}