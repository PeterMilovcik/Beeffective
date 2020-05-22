using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Beeffective.Core.Models;
using Beeffective.Presentation.Common;
using Beeffective.Presentation.EditTask;
using MaterialDesignThemes.Wpf;

namespace Beeffective.Presentation.Main.Tasks
{
    public class TaskViewModel : ViewModel
    {
        public TaskViewModel(TaskModel model)
        {
            Model = model;
            RemoveCommand = new DelegateCommand(o => OnRemoving());
            AdjustTimeSpentCommand = new DelegateCommand(AdjustTimeSpent);
            EditTaskCommand = new DelegateCommand(async obj => await EditTaskAsync());
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

        public DelegateCommand EditTaskCommand { get; }

        private async Task EditTaskAsync()
        {
            var editTaskView = new EditTaskView();
            editTaskView.DataContext = this;
            await DialogHost.Show(editTaskView, "MainDialogHost");
        }
    }
}