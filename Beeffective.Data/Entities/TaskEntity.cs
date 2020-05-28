using System;

namespace Beeffective.Data.Entities
{
    public class TaskEntity : Entity
    {
        public int ProjectId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsFinished { get; set; }
        public DateTime? DueTo { get; set; }
    }
}