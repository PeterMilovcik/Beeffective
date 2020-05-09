using System;
using System.Windows.Input;
using Beeffective.Core.Models;
using Beeffective.Presentation.Common;

namespace Beeffective.Presentation.Main.Tasks
{
    public class TaskViewModel : ViewModel
    {
        public TaskViewModel(TaskModel model)
        {
            Model = model;
            RemoveCommand = new DelegateCommand(o => OnRemoving());
        }

        public TaskModel Model { get; }

        public ICommand RemoveCommand { get; }

        public event EventHandler Removing;

        protected virtual void OnRemoving() => 
            Removing?.Invoke(this, EventArgs.Empty);
    }
}