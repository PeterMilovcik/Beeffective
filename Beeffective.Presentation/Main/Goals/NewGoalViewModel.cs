using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Beeffective.Core.Models;
using Beeffective.Presentation.Common;
using Beeffective.Presentation.Main.Dialogs;

namespace Beeffective.Presentation.Main.Goals
{
    [Export(typeof(NewGoalViewModel))]
    public class NewGoalViewModel : CoreViewModel
    {
        private readonly IDialogDisplay dialogDisplay;
        private GoalModel newGoal;

        [ImportingConstructor]
        public NewGoalViewModel(Core core, IDialogDisplay dialogDisplay) : base(core)
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
            NewGoal = new GoalModel();
            NewGoalView.DataContext = this;
            await dialogDisplay.ShowAsync(NewGoalView);
        }

        public GoalModel NewGoal
        {
            get => newGoal;
            set
            {
                if (Equals(newGoal, value)) return;
                if (newGoal != null) newGoal.PropertyChanged -= OnGoalModelPropertyChanged;
                newGoal = value;
                if (newGoal != null) newGoal.PropertyChanged += OnGoalModelPropertyChanged;
                SaveGoalCommand.RaiseCanExecuteChanged();
                NotifyPropertyChange();
            }
        }

        private void OnGoalModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(NewGoal.Title))
            {
                SaveGoalCommand.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand SaveGoalCommand { get; }

        private bool CanSaveGoal(object arg) =>
            !string.IsNullOrWhiteSpace(NewGoal?.Title) &&
            !Core.Goals.Select(goalModel => goalModel.Title).Contains(NewGoal.Title);

        private void SaveGoal(object obj)
        {
            Core.Goals.Add(NewGoal);
            dialogDisplay.CloseDialog();
        }
    }
}