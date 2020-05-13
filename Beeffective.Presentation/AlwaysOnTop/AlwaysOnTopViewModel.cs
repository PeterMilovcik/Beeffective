using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Timers;
using Beeffective.Presentation.Common;
using Beeffective.Presentation.Main.Priority;

namespace Beeffective.Presentation.AlwaysOnTop
{
    [Export]
    public class AlwaysOnTopViewModel : ViewModel
    {
        private readonly IAlwaysOnTopWindow view;
        private bool isTimePickerVisible;
        private TimeSpan remainingTime;
        private readonly Timer timer;
        private bool isTimerElapsed;

        [ImportingConstructor]
        public AlwaysOnTopViewModel(IAlwaysOnTopWindow view, PriorityObservableCollection tasks)
        {
            this.view = view;
            this.view.DataContext = this;
            Tasks = tasks;
            Tasks.PropertyChanged += OnTasksPropertyChanged;
            TimeTrackerCommand = new DelegateCommand(obj => TimeTrack());
            StartTimerCommand = new DelegateCommand(StartTimer);
            timer = new Timer();
            timer.Interval = 1000;
            timer.Elapsed += OnTimerElapsed;
        }

        public PriorityObservableCollection Tasks { get; set; }

        public DelegateCommand TimeTrackerCommand { get; }

        public DelegateCommand StartTimerCommand { get; }

        public bool IsTimePickerVisible
        {
            get => isTimePickerVisible;
            set => SetProperty(ref isTimePickerVisible, value);
        }

        public TimeSpan RemainingTime
        {
            get => remainingTime;
            set => SetProperty(ref remainingTime, value);
        }

        public bool IsTimerElapsed
        {
            get => isTimerElapsed;
            set => SetProperty(ref isTimerElapsed, value);
        }

        private void TimeTrack()
        {
            IsTimePickerVisible = !IsTimePickerVisible;
            if (Tasks.Selected.Model.IsTimerEnabled)
            {
                Tasks.Selected.Model.IsTimerEnabled = false;
                timer.Stop();
            }
        }

        private void StartTimer(object obj)
        {
            if (obj is int minutes)
            {
                Tasks.Selected.Model.IsTimerEnabled = true;
                IsTimePickerVisible = false;
                RemainingTime = TimeSpan.FromMinutes(minutes);
                timer.Start();
            }
        }

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

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            RemainingTime = RemainingTime.Add(TimeSpan.FromMilliseconds(-timer.Interval));
            if (RemainingTime <= TimeSpan.Zero)
            {
                IsTimerElapsed = true;
            }
        }
    }
}