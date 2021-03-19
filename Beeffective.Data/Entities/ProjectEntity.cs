namespace Beeffective.Data.Entities
{
    public class ProjectEntity : Entity
    {
        public GoalEntity Goal { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}