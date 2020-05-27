using Beeffective.Presentation.Common;

namespace Beeffective.Presentation.Main.Goals
{
    public class GoalViewModel : ViewModel
    {
        private string title;
        private string description;

        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }
    }
}