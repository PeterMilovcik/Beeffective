using System;

namespace Beeffective.Presentation.Common
{
    public interface IWindow : IView
    {
        void Show();
        void Hide();
        void Close();
        event EventHandler Activated;
        event EventHandler Deactivated;
    }
}