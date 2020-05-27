using System;
using Beeffective.Presentation.Common;

namespace Beeffective.Tests.Doubles
{
    public class Window : View, IWindow
    {
        public bool IsShown { get; set; }
        public bool IsClosed { get; set; }
        public void Show() => IsShown = true;
        public void Hide() => IsShown = false;
        public void Close() => IsClosed = true;
        public event EventHandler Activated;
        public event EventHandler Deactivated;
        public void Activate() => Activated?.Invoke(this, EventArgs.Empty);
        public void Deactivate() => Deactivated?.Invoke(this, EventArgs.Empty);
    }
}