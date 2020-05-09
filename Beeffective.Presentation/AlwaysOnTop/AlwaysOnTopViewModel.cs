using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using Beeffective.Core.Models;
using Beeffective.Core.Time;
using Beeffective.Presentation.Common;
using Beeffective.Presentation.Main.Priority;
using Beeffective.Services.Repository;

namespace Beeffective.Presentation.AlwaysOnTop
{
    [Export]
    public class AlwaysOnTopViewModel : ViewModel
    {
        private readonly IAlwaysOnTopWindow view;

        [ImportingConstructor]
        public AlwaysOnTopViewModel(IAlwaysOnTopWindow view, PriorityObservableCollection tasks)
        {
            this.view = view;
            this.view.DataContext = this;
            Tasks = tasks;
            Tasks.PropertyChanged += OnTasksPropertyChanged;
            TimeTrackerCommand = new DelegateCommand(obj => TimeTrack());
        }

        public PriorityObservableCollection Tasks { get; set; }

        public DelegateCommand TimeTrackerCommand { get; }

        private void TimeTrack() => 
            Tasks.Selected.Model.IsTimerEnabled = !Tasks.Selected.Model.IsTimerEnabled;

        private void OnTasksPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Tasks.IsSelected))
            {
                if (Tasks.IsSelected)
                {
                    view.Show();
                }
                else
                {
                    view.Hide();
                }
            }
        }

        public void Close() => view.Close();
    }
}