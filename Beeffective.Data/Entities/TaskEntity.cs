﻿namespace Beeffective.Data.Entities
{
    public class TaskEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Urgency { get; set; }
        public int Importance { get; set; }
    }
}