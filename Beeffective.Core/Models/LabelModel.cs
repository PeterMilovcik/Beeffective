using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Beeffective.Core.Extensions;

namespace Beeffective.Core.Models
{
    public class LabelModel : Changeable, IEquatable<LabelModel>
    {
        private string title;
        private string description;

        public LabelModel()
        {
            Records = new ObservableCollection<RecordModel>();
            Records.CollectionChanged += OnRecordsCollectionChanged;
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

        public TimeSpan TimeSpent =>
            Records.Aggregate(TimeSpan.Zero, (current, record) => current + record.Duration);

        public ObservableCollection<RecordModel> Records { get; }

        private void OnRecordsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) =>
            NotifyPropertyChange(nameof(TimeSpent));

        public bool Equals(LabelModel other)
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
            return Equals((LabelModel) obj);
        }

        public override int GetHashCode() => Id;

        public static bool operator ==(LabelModel left, LabelModel right) => Equals(left, right);

        public static bool operator !=(LabelModel left, LabelModel right) => !Equals(left, right);
    }
}