using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Timers;
using Beeffective.Core.Extensions;
using Beeffective.Presentation.Common;

namespace Beeffective.Presentation.AlwaysOnTop
{
    [Export]
    public class AlwaysOnTopViewModel : CoreViewModel
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
        private TimeSpan defaultTimerInterval;
        private bool isTimerEnabled;


        [ImportingConstructor]
        public AlwaysOnTopViewModel(Main.Core core, IAlwaysOnTopWindow view) : base(core)
        {
            this.view = view;
            this.view.DataContext = this;
            TimerCommand = new DelegateCommand(obj => TimeTrack());
            SetTimerCommand = new DelegateCommand(SetTimer);
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

        public DelegateCommand TimerCommand { get; }

        public DelegateCommand SetTimerCommand { get; }

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

        public TimeSpan DefaultTimerInterval
        {
            get => defaultTimerInterval;
            set => SetProperty(ref defaultTimerInterval, value);
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
            IsTimerEnabled = !IsTimerEnabled;
        }

        public bool IsTimerEnabled
        {
            get => isTimerEnabled;
            set => SetProperty(ref isTimerEnabled, value).IfTrue(() => timer.Enabled = IsTimerEnabled);
        }

        private void StopTimer()
        {
            //Tasks.Selected.Model.IsTimerEnabled = false;
            //timer.Stop();
            //RemainingTime = TimeSpan.Zero;
        }

        private void SetTimer(object obj)
        {
            if (int.TryParse(obj.ToString(), out var minutes))
            {
                DefaultTimerInterval = TimeSpan.FromMinutes(minutes);
                RemainingTime = DefaultTimerInterval;
            }
        }

        private void OnTasksPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //if (e.PropertyName == nameof(Tasks.IsSelected))
            //{
            //    UpdateViewVisibility();
            //}
        }

        private void UpdateViewVisibility()
        {
            if (Core.Tasks.IsTaskSelected)
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
            //if (Tasks.Selected == null) return;
            //if (Tasks.Selected.Model.IsTimerEnabled) StopTimer();
            //Tasks.Selected.Model.IsFinished = true;
            //Tasks.Selected = null;
            //IsRepeatQuestionVisible = false;
            //IsRepeatPickerVisible = false;
            //Tasks.NotifyPropertyChange(nameof(Tasks.Unfinished));
            //Tasks.NotifyPropertyChange(nameof(Tasks.Finished));
            //Tasks.NotifyChange();
            //UpdateViewVisibility();
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
            //IsRepeatQuestionVisible = false;
            //IsRepeatPickerVisible = true;
            //if (Tasks.Selected == null) return;
            //var dueTo = Tasks.Selected.Model.DueTo;
            //if (dueTo == null)
            //{
            //    DueToDate = DateTime.Now.Date;
            //    DueToTime = DateTime.Now.TimeOfDay;
            //}
            //else
            //{
            //    DueToDate = dueTo.Value.Date;
            //    DueToTime = dueTo.Value.TimeOfDay;
            //}

            //originalDueDate = DueToDate.Add(DueToTime);
            //UpdateRepeatIntervalTimeSpan();
        }

        public bool IsRepeatPickerVisible
        {
            get => isRepeatPickerVisible;
            set => SetProperty(ref isRepeatPickerVisible, value);
        }

        private void Repeat()
        {
            //if (Tasks.Selected == null) return;
            //if (Tasks.Selected.Model.IsTimerEnabled) StopTimer();
            //Tasks.Selected.Model.DueTo = DueToDate.Add(DueToTime);
            //Tasks.Selected = null;
            //IsRepeatQuestionVisible = false;
            //IsRepeatPickerVisible = false;
            //Tasks.NotifyChange();
            //UpdateViewVisibility();
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

        public void Close()
        {
            Core.Tasks.Selected = null;
            view.Close();
        }

        public void Hide() => view.Hide();

        public void Show()
        {
            if (Core.Tasks.Selected != null) view.Show();
        }
    }
}