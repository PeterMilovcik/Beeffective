using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Timers;
using Beeffective.Core.Extensions;
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
            FinishCommand = new DelegateCommand(obj => Finish());
            timer = new Timer();
            timer.Interval = 1000;
            timer.Elapsed += OnTimerElapsed;
        }

        public PriorityObservableCollection Tasks { get; set; }

        public DelegateCommand TimeTrackerCommand { get; }

        public DelegateCommand StartTimerCommand { get; }

        public DelegateCommand FinishCommand { get; }

        public bool IsTimePickerVisible
        {
            get => isTimePickerVisible;
            set => SetProperty(ref isTimePickerVisible, value);
        }

        public TimeSpan RemainingTime
        {
            get => remainingTime;
            set => SetProperty(ref remainingTime, value)
                .IfTrue(() => IsTimerElapsed = RemainingTime < TimeSpan.Zero);
        }

        public bool IsTimerElapsed
        {
            get => isTimerElapsed;
            set => SetProperty(ref isTimerElapsed, value);
        }

        private void TimeTrack()
        {
            if (Tasks.Selected.Model.IsTimerEnabled)
            {
                StopTimer();
            }
            else
            {
                IsTimePickerVisible = true;
            }
        }

        private void StopTimer()
        {
            Tasks.Selected.Model.IsTimerEnabled = false;
            timer.Stop();
            RemainingTime = TimeSpan.Zero;
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

        private void OnTimerElapsed(object sender, ElapsedEventArgs e) => 
            RemainingTime = RemainingTime.Add(TimeSpan.FromMilliseconds(-timer.Interval));

        private void Finish()
        {
            if (Tasks.Selected == null) return;
            if (Tasks.Selected.Model.IsTimerEnabled) StopTimer();
            Tasks.Selected.Model.IsFinished = true;
        }

        public void Close() => view.Close();
    }
}