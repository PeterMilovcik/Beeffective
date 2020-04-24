using Beeffective.Presentation.Common;

namespace Beeffective.Presentation.Main.Tasks
{
    public class TaskViewModel : ViewModel
    {
        private string title;

        public int Id { get; set; }

        public string Title
        {
            get => title;
            set
            {
                if (SetProperty(ref title, value)) IsChanged = true;
            }
        }
    }
}