using System.Collections.Generic;

namespace Beeffective.Data.Entities
{
    public class TaskEntity : Entity
    {
        public int ProjectId { get; set; }
        public ProjectEntity Project { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsFinished { get; set; }
        public List<RecordEntity> Records { get; set; }
    }
}