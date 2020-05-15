using System;

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

        public event EventHandler Changed;

        public virtual void NotifyChange() => 
            Changed?.Invoke(this, EventArgs.Empty);
    }
}