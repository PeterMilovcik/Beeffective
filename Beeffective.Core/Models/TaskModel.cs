using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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

        public TaskModel()
        {
            Records = new ObservableCollection<RecordModel>();
            Records.CollectionChanged += OnRecordsCollectionChanged;
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
                foreach (var recordViewModel in Records)
                {
                    result = result.Add(recordViewModel.Duration);
                }

                return result;
            }
        }

        private void OnRecordsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(TimeSpent));
        }

        public ObservableCollection<RecordModel> Records { get; }
    }
}
