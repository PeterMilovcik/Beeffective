using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Windows;
using Beeffective.Core.Models;
using Beeffective.Core.Time;
using Beeffective.Presentation.Common;
using Beeffective.Presentation.Extensions;
using Beeffective.Presentation.Main.Priority;
using Beeffective.Services.Repository;

namespace Beeffective.Presentation.AlwaysOnTop
{
    [Export]
    public class AlwaysOnTopViewModel : ViewModel
    {
        private readonly IAlwaysOnTopWindow view;
        private readonly TimeTracker timeTracker;
        private readonly IRepositoryService repository;
        private bool isTimeTrackerEnabled;
        private int? timedTaskId;
        private TimeSpan timeSpent;

        [ImportingConstructor]
        public AlwaysOnTopViewModel(
            IAlwaysOnTopWindow view, 
            PriorityObservableCollection tasks, 
            TimeTracker timeTracker,
            IRepositoryService repository)
        {
            this.view = view;
            this.view.DataContext = this;
            Tasks = tasks;
            Tasks.PropertyChanged += OnTasksPropertyChanged;
            this.timeTracker = timeTracker;
            this.repository = repository;
            this.timeTracker.Started += OnTimeTrackerStarted;
            this.timeTracker.Stopped += OnTimeTrackerStopped;
            this.timeTracker.Ticked += OnTimeTrackerTicked;
            TimeTrackerCommand = new DelegateCommand(obj => TimeTrack());
        }

        public DelegateCommand TimeTrackerCommand { get; }

        public PriorityObservableCollection Tasks { get; }

        public bool IsTimeTrackerEnabled
        {
            get => isTimeTrackerEnabled;
            set => SetProperty(ref isTimeTrackerEnabled, value);
        }

        private void OnTasksPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Tasks.Selected))
            {
                if (timeTracker.IsEnabled)
                {
                    timeTracker.Stop();
                    IsTimeTrackerEnabled = timeTracker.IsEnabled;
                }
            }
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

        public TimeSpan TimeSpent
        {
            get => timeSpent;
            set => SetProperty(ref timeSpent, value);
        }

        private void OnTimeTrackerStarted(object sender, EventArgs e)
        {
            if (Tasks.IsSelected)
            {
                timedTaskId = Tasks.Selected.Id;
            }
        }

        private void OnTimeTrackerTicked(object? sender, EventArgs e)
        {
            TimeSpent = DateTime.Now - timeTracker.StartTime;
        }

        private async void OnTimeTrackerStopped(object? sender, EventArgs e)
        {
            if (timedTaskId == null) return;
            try
            {
                await repository.AddRecordAsync(new RecordModel
                {
                    TaskId = timedTaskId.Value,
                    StartAt = timeTracker.StartTime,
                    StopAt = timeTracker.StopTime
                });
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }
        }

        private void TimeTrack()
        {
            timeTracker.Toggle();
            IsTimeTrackerEnabled = timeTracker.IsEnabled;
        }

        public void Close() => view.Close();
    }
}