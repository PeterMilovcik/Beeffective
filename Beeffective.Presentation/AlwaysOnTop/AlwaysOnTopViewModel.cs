using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Timers;
using Beeffective.Core.Extensions;
using Beeffective.Core.Models;
using Beeffective.Presentation.Common;
using Beeffective.Presentation.Extensions;
using Beeffective.Presentation.Main.Tasks;
using Beeffective.Services.Repository;
using Syncfusion.Windows.Shared;
using DelegateCommand = Beeffective.Presentation.Common.DelegateCommand;

namespace Beeffective.Presentation.AlwaysOnTop
{
    [Export]
    public class AlwaysOnTopViewModel : CoreViewModel
    {
        private readonly IAlwaysOnTopWindow view;
        private readonly IRepositoryService repository;
        private TimeSpan remainingTime;
        private readonly Timer timer;
        private bool isTimerElapsed;
        private TimeSpan defaultTimerInterval;
        private bool isTimerEnabled;
        private readonly Timer untilDueToTimer;
        private string startsIn;
        private RecordModel record;


        [ImportingConstructor]
        public AlwaysOnTopViewModel(Main.Core core, IAlwaysOnTopWindow view, IRepositoryService repository) : base(core)
        {
            this.repository = repository;
            this.view = view;
            this.view.DataContext = this;
            TimerCommand = new DelegateCommand(obj => TimeTrack());
            SetTimerCommand = new DelegateCommand(SetTimer);
            timer = new Timer();
            timer.Interval = 1000;
            timer.Elapsed += OnTimerElapsed;
            untilDueToTimer = new Timer();
            untilDueToTimer.Interval = 1000;
            untilDueToTimer.Elapsed += OnUntilDueToTimerElapsed;
            RemainingTime = TimeSpan.FromMinutes(30);
            Core.Tasks.PropertyChanged += OnTasksPropertyChanged;
        }

        public DelegateCommand TimerCommand { get; }

        public DelegateCommand SetTimerCommand { get; }

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
            set => SetProperty(ref isTimerEnabled, value).IfTrue(() =>
            {
                if (IsTimerEnabled)
                {
                    StartTimer();
                }
                else
                {
                    StopTimer();
                }

            });
        }

        private void StartTimer()
        {
            if (Core.Tasks.Selected == null) return;
            timer.Start();
            record = new RecordModel();
            record.TaskId = Core.Tasks.Selected.Id;
            record.StartAt = DateTime.Now;
        }

        private void StopTimer()
        {
            timer.Stop();
            if (Core.Tasks.Selected == null) return;
            if (record == null) return;
            record.StopAt = DateTime.Now;
            Core.Tasks.Selected.Records.Add(record);
        }

        private void SetTimer(object obj)
        {
            if (int.TryParse(obj.ToString(), out var minutes))
            {
                DefaultTimerInterval = TimeSpan.FromMinutes(minutes);
                RemainingTime = DefaultTimerInterval;
            }
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e) => 
            RemainingTime = RemainingTime.Add(TimeSpan.FromMilliseconds(-timer.Interval));


        public void Close()
        {
            untilDueToTimer.Stop();
            Core.Tasks.Selected = null;
            view.Close();
        }

        public void Hide()
        {
            untilDueToTimer.Stop();
            view.Hide();
        }

        public void Show()
        {
            if (Core.Tasks.Selected != null) view.Show();
            untilDueToTimer.Start();
        }

        public string StartsIn
        {
            get => startsIn;
            set => SetProperty(ref startsIn, value);
        }

        private void OnUntilDueToTimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (Core.Tasks.Selected == null) return;
            if (Core.Tasks.Selected.DueTo == null) return;
            var startsInTimeSpan = Core.Tasks.Selected.DueTo - DateTime.Now;
            StartsIn = startsInTimeSpan > TimeSpan.Zero 
                ? $"starts in {startsInTimeSpan.Value.ToFormattedString()}" 
                : string.Empty;
        }

        private void OnTasksPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(TasksViewModel.Selected))
            {
                if (Core.Tasks.Selected == null)
                {
                    Hide();
                }
                else
                {
                    Show();
                }
                RemainingTime = DefaultTimerInterval;
            }
        }
    }
}