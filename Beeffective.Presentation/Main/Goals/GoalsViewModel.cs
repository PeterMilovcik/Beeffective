using System.ComponentModel.Composition;
using Beeffective.Core.Models;
using Beeffective.Presentation.Common;
using Beeffective.Presentation.Main.Priority;

namespace Beeffective.Presentation.Main.Goals
{
    [Export]
    public class GoalsViewModel : ContentViewModel
    {
        private GoalModel selectedGoal;

        [ImportingConstructor]
        public GoalsViewModel(PriorityObservableCollection tasks) : base(tasks)
        {
        }

        public GoalModel SelectedGoal
        {
            get => selectedGoal;
            set => SetProperty(ref selectedGoal, value);
        }
    }
}
