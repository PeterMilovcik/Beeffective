using Beeffective.Presentation.Common;

namespace Beeffective.Presentation.Main.Tasks
{
    public class TaskViewModel : ViewModel
    {
        private string title;
        private int urgency;
        private int importance;

        public int Id { get; set; }

        public string Title
        {
            get => title;
            set
            {
                if (SetProperty(ref title, value)) IsChanged = true;
            }
        }

        public int Urgency
        {
            get => urgency;
            set
            {
                if (SetProperty(ref urgency, value)) IsChanged = true;
            }
        }

        public int Importance
        {
            get => importance;
            set
            {
                if (SetProperty(ref importance, value)) IsChanged = true;
            }
        }
    }
}