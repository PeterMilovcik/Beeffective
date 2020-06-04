using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Beeffective.Core.Extensions;

namespace Beeffective.Core.Models
{
    public class TaskModel : Changeable, IEquatable<TaskModel>
    {
        private string title;
        private string description;
        private DateTime? dueTo;
        private ProjectModel project;
        private bool isFinished;

        public TaskModel()
        {
            Records = new ObservableCollection<RecordModel>();
            Records.CollectionChanged += OnRecordsCollectionChanged;
            Labels = new ObservableCollection<LabelModel>();
            Labels.CollectionChanged += OnLabelsCollectionChanged;
        }

        public int Id { get; set; }

        public string Title
        {
            get => title;
            set => SetProperty(ref title, value).IfTrue(NotifyChange);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value).IfTrue(NotifyChange);
        }

        public ObservableCollection<LabelModel> Labels { get; }

        public ObservableCollection<RecordModel> Records { get; }

        public DateTime? DueTo
        {
            get => dueTo;
            set => SetProperty(ref dueTo, value).IfTrue(NotifyChange);
        }

        public ProjectModel Project
        {
            get => project;
            set => SetProperty(ref project, value).IfTrue(NotifyChange);
        }

        public bool IsFinished
        {
            get => isFinished;
            set => SetProperty(ref isFinished, value).IfTrue(NotifyChange);
        }

        public bool Equals(TaskModel other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TaskModel) obj);
        }

        public override int GetHashCode() => Id;

        public static bool operator ==(TaskModel left, TaskModel right) => Equals(left, right);

        public static bool operator !=(TaskModel left, TaskModel right) => !Equals(left, right);

        private void OnLabelsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var labelModel in e.NewItems.OfType<LabelModel>())
                {
                    OnLabelAdded(labelModel);
                }
            }

            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var labelModel in e.OldItems.OfType<LabelModel>())
                {
                    OnLabelRemoved(labelModel);
                }
            }
        }

        public EventHandler<LabelEventArgs> LabelAdded;

        private void OnLabelAdded(LabelModel labelModel) => 
            LabelAdded?.Invoke(this, new LabelEventArgs(labelModel));

        public EventHandler<LabelEventArgs> LabelRemoved;

        private void OnLabelRemoved(LabelModel labelModel) =>
            LabelRemoved?.Invoke(this, new LabelEventArgs(labelModel));

        private void OnRecordsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var recordModel in e.NewItems.OfType<RecordModel>())
                {
                    OnRecordAdded(recordModel);
                }
            }

            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var recordModel in e.OldItems.OfType<RecordModel>())
                {
                    OnRecordRemoved(recordModel);
                }
            }
            NotifyPropertyChange(nameof(TimeSpent));
        }

        public TimeSpan TimeSpent => 
            Records.Aggregate(TimeSpan.Zero, (current, record) => current + record.Duration);

        public EventHandler<RecordEventArgs> RecordAdded;

        private void OnRecordAdded(RecordModel recordModel) =>
            RecordAdded?.Invoke(this, new RecordEventArgs(recordModel));

        public EventHandler<RecordEventArgs> RecordRemoved;

        private void OnRecordRemoved(RecordModel recordModel) =>
            RecordRemoved?.Invoke(this, new RecordEventArgs(recordModel));
    }

    public class LabelEventArgs : EventArgs
    {
        public LabelEventArgs(LabelModel labelModel)
        {
            LabelModel = labelModel ?? throw new ArgumentNullException(nameof(labelModel));
        }

        public LabelModel LabelModel { get; }
    }

    public class RecordEventArgs : EventArgs
    {
        public RecordEventArgs(RecordModel recordModel)
        {
            RecordModel = recordModel ?? throw new ArgumentNullException(nameof(recordModel));
        }

        public RecordModel RecordModel { get; }
    }
}
