﻿using System;

namespace Beeffective.Core.Models
{
    public class LabelModel : Observable, IEquatable<LabelModel>
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

        public bool Equals(LabelModel other)
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
            return Equals((LabelModel) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(title, description, Id);
        }

        public static bool operator ==(LabelModel left, LabelModel right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(LabelModel left, LabelModel right)
        {
            return !Equals(left, right);
        }
    }
}