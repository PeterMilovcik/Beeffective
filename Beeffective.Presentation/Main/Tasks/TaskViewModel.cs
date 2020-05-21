using System;
using System.Security.Permissions;
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
            AdjustTimeSpentCommand = new DelegateCommand(AdjustTimeSpent);
        }

        public TaskModel Model { get; }

        public ICommand RemoveCommand { get; }

        public event EventHandler Removing;

        protected virtual void OnRemoving() => 
            Removing?.Invoke(this, EventArgs.Empty);

        public DelegateCommand AdjustTimeSpentCommand { get; }

        private void AdjustTimeSpent(object obj)
        {
            if (int.TryParse(obj?.ToString(), out var value))
            {
                var newRecord = new RecordModel();
                newRecord.StopAt = DateTime.Now;
                newRecord.StartAt = newRecord.StopAt - TimeSpan.FromMinutes(value);
                newRecord.TaskId = Model.Id;
                Model.Records.Add(newRecord);
                Model.NotifyPropertyChange(nameof(Model.TimeSpent));
            }
        }
    }
}