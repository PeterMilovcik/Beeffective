using System;

namespace Beeffective.Core.Models
{
    public class GoalModel : Observable, IEquatable<GoalModel>
    {
        private string name;
        private TimeSpan timeSpent;

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public TimeSpan TimeSpent
        {
            get => timeSpent;
            set => SetProperty(ref timeSpent, value);
        }

        public bool Equals(GoalModel other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return name == other.name;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((GoalModel) obj);
        }

        public override int GetHashCode() => 
            name != null ? name.GetHashCode() : 0;

        public static bool operator ==(GoalModel left, GoalModel right) => 
            Equals(left, right);

        public static bool operator !=(GoalModel left, GoalModel right) => 
            !Equals(left, right);

        public override string ToString() => 
            $"{nameof(Name)}: {Name}, {nameof(TimeSpent)}: {TimeSpent}";
    }
}