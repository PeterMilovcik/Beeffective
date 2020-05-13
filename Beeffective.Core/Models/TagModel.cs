using System;
using System.Collections.Generic;

namespace Beeffective.Core.Models
{
    public class TagModel : Observable, IEquatable<TagModel>
    {
        private string name;
        private TimeSpan timeSpent;

        public TagModel()
        {
            Tasks = new List<TaskModel>();
        }

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

        public List<TaskModel> Tasks { get; }

        public bool Equals(TagModel other)
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
            return Equals((TagModel) obj);
        }

        public override int GetHashCode() => 
            name != null ? name.GetHashCode() : 0;

        public static bool operator ==(TagModel left, TagModel right) => 
            Equals(left, right);

        public static bool operator !=(TagModel left, TagModel right) => 
            !Equals(left, right);

        public override string ToString() => 
            $"{nameof(Name)}: {Name}, {nameof(TimeSpent)}: {TimeSpent}";
    }
}