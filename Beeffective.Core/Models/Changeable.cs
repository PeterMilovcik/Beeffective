using System;

namespace Beeffective.Core.Models
{
    public class Changeable : Workable
    {
        public event EventHandler Changed;

        public virtual void NotifyChange() => 
            Changed?.Invoke(this, EventArgs.Empty);
    }
}