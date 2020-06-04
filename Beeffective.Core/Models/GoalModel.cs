using System;
using Beeffective.Core.Extensions;

namespace Beeffective.Core.Models
{
    public class GoalModel : Changeable, IEquatable<GoalModel>
    {
        private string title;
        private string description;
        private int importance;

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

        public int Importance
        {
            get => importance;
            set => SetProperty(ref importance, value).IfTrue(NotifyChange);
        }

        public bool Equals(GoalModel other)
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
            return Equals((GoalModel) obj);
        }

        public override int GetHashCode() => Id;

        public static bool operator ==(GoalModel left, GoalModel right) => Equals(left, right);

        public static bool operator !=(GoalModel left, GoalModel right) => !Equals(left, right);
    }
}