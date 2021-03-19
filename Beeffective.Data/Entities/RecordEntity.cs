using System;

namespace Beeffective.Data.Entities
{
    public class RecordEntity : Entity
    {
        public int TaskId { get; set; }
        public TaskEntity Task { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
