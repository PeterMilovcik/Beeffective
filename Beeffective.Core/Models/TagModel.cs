using System;
using System.Collections.Generic;
using System.ComponentModel;
using Beeffective.Core.Extensions;

namespace Beeffective.Core.Models
{
    public class TagModel : Observable, IEquatable<TagModel>
    {
        private string name;
        private TimeSpan timeSpent;
        private List<TaskModel> tasks;

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

        public List<TaskModel> Tasks
        {
            get => tasks;
            set => SetProperty(ref tasks, value).IfTrue(() =>
            {
                foreach (var taskModel in tasks)
                {
                    taskModel.PropertyChanged += OnTaskPropertyChanged;
                }
            });
        }

        private void OnTaskPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(TaskModel.TimeSpent))
            {
                var newTimeSpent = TimeSpan.Zero;
                foreach (var taskModel in Tasks)
                {
                    newTimeSpent = newTimeSpent.Add(taskModel.TimeSpent);
                }

                TimeSpent = newTimeSpent;
            }
        }

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