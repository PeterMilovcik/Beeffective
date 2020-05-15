using System;
using System.Collections.ObjectModel;
using System.Timers;
using Beeffective.Core.Extensions;

namespace Beeffective.Core.Models
{
    public class TaskModel : Changeable
    {
        private string title;
        private int urgency;
        private int importance;
        private string goal;
        private string tags;
        private RecordModel currentRecord;
        private readonly Timer timer;
        private bool isTimerEnabled;
        private bool isFinished;

        public TaskModel()
        {
            timer = new Timer();
            timer.Interval = 500;
            timer.Elapsed += OnTimerElapsed;
            Records = new ObservableCollection<RecordModel>();
        }

        public int Id { get; set; }

        public string Title
        {
            get => title;
            set => SetProperty(ref title, value).IfTrue(() => IsChanged = true);
        }

        public int Urgency
        {
            get => urgency;
            set
            {
                SetProperty(ref urgency, value).IfTrue(() =>
                {
                    OnPropertyChanged(nameof(Priority));
                    IsChanged = true;
                });
            }
        }

        public int Importance
        {
            get => importance;
            set => SetProperty(ref importance, value).IfTrue(() =>
            {
                OnPropertyChanged(nameof(Priority));
                IsChanged = true;
            });
        }

        public double Priority => Math.Sqrt(Math.Pow(Urgency, 2) + Math.Pow(Importance, 2));

        public string Goal
        {
            get => goal;
            set => SetProperty(ref goal, value).IfTrue(() => IsChanged = true);
        }

        public string Tags
        {
            get => tags;
            set => SetProperty(ref tags, value).IfTrue(() => IsChanged = true);
        }

        public TimeSpan TimeSpent
        {
            get
            {
                var result = new TimeSpan();
                foreach (var recordModel in Records)
                {
                    result = result.Add(recordModel.Duration);
                }

                if (currentRecord != null)
                {
                    result = result.Add(currentRecord.Duration);
                }

                return result;
            }
        }

        public bool IsTimerEnabled
        {
            get => isTimerEnabled;
            set => SetProperty(ref isTimerEnabled, value).IfTrue(() =>
            {
                if (isTimerEnabled) StartTimer();
                else StopTimer();
            });
        }

        private void StartTimer()
        {
            currentRecord = new RecordModel {TaskId = Id, StartAt = DateTime.Now};
            timer.Start();
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (currentRecord == null) return;
            currentRecord.StopAt = DateTime.Now;
            OnPropertyChanged(nameof(TimeSpent));
        }

        private void StopTimer()
        {
            timer.Stop();
            currentRecord.StopAt = DateTime.Now;
            Records.Add(currentRecord);
            IsChanged = true;
            currentRecord = null;
            OnPropertyChanged(nameof(TimeSpent));
        }

        public ObservableCollection<RecordModel> Records { get; }

        public bool IsFinished
        {
            get => isFinished;
            set => SetProperty(ref isFinished, value);
        }
    }
}
