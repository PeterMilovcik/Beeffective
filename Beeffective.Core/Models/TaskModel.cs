using System;
using System.Collections.ObjectModel;

namespace Beeffective.Core.Models
{
    public class TaskModel : Observable, IEquatable<TaskModel>
    {
        private string title;
        private string description;
        private DateTime? dueTo;
        private ProjectModel project;
        private bool isFinished;

        public TaskModel()
        {
            Records = new ObservableCollection<RecordModel>();
            Labels = new ObservableCollection<LabelModel>();
        }

        public int Id { get; set; }

        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public ObservableCollection<LabelModel> Labels { get; }

        public ObservableCollection<RecordModel> Records { get; }

        public DateTime? DueTo
        {
            get => dueTo;
            set => SetProperty(ref dueTo, value);
        }

        public ProjectModel Project
        {
            get => project;
            set => SetProperty(ref project, value);
        }

        public bool IsFinished
        {
            get => isFinished;
            set => SetProperty(ref isFinished, value);
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
    }
}
