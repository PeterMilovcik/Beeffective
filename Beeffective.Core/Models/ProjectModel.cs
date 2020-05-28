using System;

namespace Beeffective.Core.Models
{
    public class ProjectModel : Observable, IEquatable<ProjectModel>
    {
        private GoalModel goal;
        private string title;
        private string description;

        public int Id { get; set; }

        public GoalModel Goal
        {
            get => goal;
            set => SetProperty(ref goal, value);
        }

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

        public bool Equals(ProjectModel other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(goal, other.goal) &&
                   title == other.title &&
                   description == other.description &&
                   Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ProjectModel) obj);
        }

        public override int GetHashCode() => 
            HashCode.Combine(goal, title, description, Id);

        public static bool operator ==(ProjectModel left, ProjectModel right) => 
            Equals(left, right);

        public static bool operator !=(ProjectModel left, ProjectModel right) => 
            !Equals(left, right);
    }
}