using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Beeffective.Core.Models;
using Beeffective.Presentation.Common;
using Beeffective.Presentation.Main.Dialogs;
using Beeffective.Presentation.Main.Priority;
using Beeffective.Presentation.NewGoal;

namespace Beeffective.Presentation.Main.Goals
{
    [Export(typeof(NewGoalViewModel))]
    public class NewGoalViewModel : TaskCollectionViewModel
    {
        private readonly IDialogDisplay dialogDisplay;
        private GoalViewModel newGoal;

        [ImportingConstructor]
        public NewGoalViewModel(PriorityObservableCollection tasks, IDialogDisplay dialogDisplay) : base(tasks)
        {
            this.dialogDisplay = dialogDisplay;
            ShowNewGoalDialogCommand = new AsyncCommand(ShowNewGoalDialogAsync);
            SaveGoalCommand = new DelegateCommand(CanSaveGoal, SaveGoal);
        }

        [Import]
        public INewGoalView NewGoalView { get; set; }

        public IAsyncCommand ShowNewGoalDialogCommand { get; }

        private async Task ShowNewGoalDialogAsync()
        {
            NewGoal = CreateGoalViewModel();
            NewGoalView.DataContext = this;
            await dialogDisplay.ShowAsync(NewGoalView);
        }

        public GoalViewModel NewGoal
        {
            get => newGoal;
            set
            {
                if (Equals(newGoal, value)) return;
                if (newGoal != null) newGoal.Model.PropertyChanged -= OnGoalModelPropertyChanged;
                newGoal = value;
                newGoal.Model.PropertyChanged += OnGoalModelPropertyChanged;
                SaveGoalCommand.RaiseCanExecuteChanged();
                NotifyPropertyChange();
            }
        }

        private void OnGoalModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(NewGoal.Model.Title))
            {
                SaveGoalCommand.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand SaveGoalCommand { get; }

        private bool CanSaveGoal(object arg) =>
            !string.IsNullOrWhiteSpace(NewGoal?.Model.Title) &&
            !Tasks.Goals.Select(g => g.Model.Title).Contains(NewGoal.Model.Title);

        private void SaveGoal(object obj)
        {
            Tasks.Goals.Add(NewGoal);
            dialogDisplay.CloseDialog();
        }

        private static GoalViewModel CreateGoalViewModel() => new GoalViewModel(new GoalModel());
    }
}