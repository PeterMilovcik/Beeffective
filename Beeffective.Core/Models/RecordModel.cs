using System;

namespace Beeffective.Core.Models
{
    public class RecordModel
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime StopAt { get; set; }
        public TimeSpan Duration => StopAt - StartAt;
    }
}
