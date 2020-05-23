using System;
using System.Collections.Generic;
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
        private bool isRepeatQuestionVisible;
        private bool isRepeatPickerVisible;
        private int repeatEvery;
        private string repeatInterval;
        private DateTime dueToDate;
        private TimeSpan dueToTime;
        private DateTime originalDueDate;
        private TimeSpan repeatIntervalTimeSpan;


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
            ShowRepeatQuestionCommand = new DelegateCommand(obj => ShowRepeatQuestion());
            ShowRepeatPickerCommand = new DelegateCommand(obj => ShowRepeatPicker());
            RepeatCommand = new DelegateCommand(obj => Repeat());
            timer = new Timer();
            timer.Interval = 1000;
            timer.Elapsed += OnTimerElapsed;
            RepeatEvery = 1;
            RepeatInterval = "week";
            RepeatIntervals = new List<string> {"day", "week", "month", "year"};
        }

        public PriorityObservableCollection Tasks { get; set; }

        public DelegateCommand TimeTrackerCommand { get; }

        public DelegateCommand StartTimerCommand { get; }

        public DelegateCommand ShowRepeatQuestionCommand { get; }

        public DelegateCommand ShowRepeatPickerCommand { get; }

        public DelegateCommand FinishCommand { get; }

        public DelegateCommand RepeatCommand { get; }

        public IEnumerable<string> RepeatIntervals { get; }

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
                UpdateViewVisibility();
            }
        }

        private void UpdateViewVisibility()
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

        private void OnTimerElapsed(object sender, ElapsedEventArgs e) => 
            RemainingTime = RemainingTime.Add(TimeSpan.FromMilliseconds(-timer.Interval));

        private void Finish()
        {
            if (Tasks.Selected == null) return;
            if (Tasks.Selected.Model.IsTimerEnabled) StopTimer();
            Tasks.Selected.Model.IsFinished = true;
            Tasks.Selected = null;
            IsRepeatQuestionVisible = false;
            IsRepeatPickerVisible = false;
            Tasks.NotifyPropertyChange(nameof(Tasks.Unfinished));
            Tasks.NotifyPropertyChange(nameof(Tasks.Finished));
            Tasks.NotifyChange();
            UpdateViewVisibility();
        }

        private void ShowRepeatQuestion()
        {
            IsRepeatQuestionVisible = true;
        }

        public bool IsRepeatQuestionVisible
        {
            get => isRepeatQuestionVisible;
            set => SetProperty(ref isRepeatQuestionVisible, value);
        }

        private void ShowRepeatPicker()
        {
            IsRepeatQuestionVisible = false;
            IsRepeatPickerVisible = true;
            if (Tasks.Selected == null) return;
            var dueTo = Tasks.Selected.Model.DueTo;
            if (dueTo == null)
            {
                DueToDate = DateTime.Now.Date;
                DueToTime = DateTime.Now.TimeOfDay;
            }
            else
            {
                DueToDate = dueTo.Value.Date;
                DueToTime = dueTo.Value.TimeOfDay;
            }

            originalDueDate = DueToDate.Add(DueToTime);
            UpdateRepeatIntervalTimeSpan();
        }

        public bool IsRepeatPickerVisible
        {
            get => isRepeatPickerVisible;
            set => SetProperty(ref isRepeatPickerVisible, value);
        }

        private void Repeat()
        {
            if (Tasks.Selected == null) return;
            if (Tasks.Selected.Model.IsTimerEnabled) StopTimer();
            Tasks.Selected.Model.DueTo = DueToDate.Add(DueToTime);
            Tasks.Selected = null;
            IsRepeatQuestionVisible = false;
            IsRepeatPickerVisible = false;
            Tasks.NotifyChange();
            UpdateViewVisibility();
        }

        public DateTime DueToDate
        {
            get => dueToDate;
            set => SetProperty(ref dueToDate, value);
        }

        public TimeSpan DueToTime
        {
            get => dueToTime;
            set => SetProperty(ref dueToTime, value);
        }

        public int RepeatEvery
        {
            get => repeatEvery;
            set => SetProperty(ref repeatEvery, value).IfTrue(UpdateRepeatIntervalTimeSpan);
        }

        public string RepeatInterval
        {
            get => repeatInterval;
            set => SetProperty(ref repeatInterval, value).IfTrue(UpdateRepeatIntervalTimeSpan);
        }

        private void UpdateRepeatIntervalTimeSpan()
        {
            var timeSpan = TimeSpan.Zero;
            switch (RepeatInterval)
            {
                case "day":
                    timeSpan = TimeSpan.FromDays(RepeatEvery);
                    break;
                case "week":
                    timeSpan = TimeSpan.FromDays(RepeatEvery * 7);
                    break;
                case "month":
                    timeSpan = TimeSpan.FromDays(RepeatEvery * 7 * 4);
                    break;
                case "year":
                    timeSpan = TimeSpan.FromDays(RepeatEvery * 365);
                    break;
            }

            repeatIntervalTimeSpan = timeSpan;
            DueToDate = originalDueDate.Add(repeatIntervalTimeSpan).Date;
            DueToTime = originalDueDate.Add(repeatIntervalTimeSpan).TimeOfDay;
        }

        public void Close() => view.Close();

        public void Hide() => view.Hide();

        public void Show()
        {
            if (Tasks.Selected != null) view.Show();
        }
    }
}