using System.Collections.Generic;

namespace Beeffective.Core.Models
{
    public class TaskModel
    {
        public TaskModel()
        {
            Records = new List<RecordModel>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public int Urgency { get; set; }
        public int Importance { get; set; }
        public string Goal { get; set; }
        public string Tags { get; set; }
        public List<RecordModel> Records { get; set; }
    }
}
