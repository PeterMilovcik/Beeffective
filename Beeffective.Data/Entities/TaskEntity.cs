﻿using System;

namespace Beeffective.Data.Entities
{
    public class TaskEntity : Entity
    {
        public string Title { get; set; }
        public int Urgency { get; set; }
        public int Importance { get; set; }
        public string Goal { get; set; }
        public string Tags { get; set; }
        public bool IsFinished { get; set; }
        public DateTime? DueTo { get; set; }
    }
}