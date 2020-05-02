using System;

namespace Beeffective.Data.Entities
{
    public class RecordEntity
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime StopAt { get; set; }
    }
}
