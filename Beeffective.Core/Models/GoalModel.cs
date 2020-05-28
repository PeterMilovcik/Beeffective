using System;

namespace Beeffective.Core.Models
{
    public class GoalModel : Observable, IEquatable<GoalModel>
    {
        private string title;
        private string description;

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

        public bool Equals(GoalModel other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return title == other.title && description == other.description && Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((GoalModel) obj);
        }

        public override int GetHashCode() => 
            HashCode.Combine(title, description, Id);

        public static bool operator ==(GoalModel left, GoalModel right) => 
            Equals(left, right);

        public static bool operator !=(GoalModel left, GoalModel right) => 
            !Equals(left, right);
    }
}