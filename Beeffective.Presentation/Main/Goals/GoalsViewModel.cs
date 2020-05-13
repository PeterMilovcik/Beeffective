using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using Beeffective.Core.Extensions;
using Beeffective.Core.Models;
using Beeffective.Presentation.Common;
using Beeffective.Presentation.Main.Priority;
using Beeffective.Presentation.Main.Tasks;

namespace Beeffective.Presentation.Main.Goals
{
    [Export]
    public class GoalsViewModel : ContentViewModel
    {
        private GoalModel selectedGoal;
        private ObservableCollection<TaskViewModel> goalTasks;

        [ImportingConstructor]
        public GoalsViewModel(PriorityObservableCollection tasks) : base(tasks)
        {
        }

        public GoalModel SelectedGoal
        {
            get => selectedGoal;
            set => SetProperty(ref selectedGoal, value).IfTrue(() =>
            {
                GoalTasks = SelectedGoal != null
                    ? new ObservableCollection<TaskViewModel>(Tasks.Where(t => t.Model.Goal == SelectedGoal.Name))
                    : new ObservableCollection<TaskViewModel>();
            });
        }

        public ObservableCollection<TaskViewModel> GoalTasks
        {
            get => goalTasks;
            set => SetProperty(ref goalTasks, value);
        }
    }
}
