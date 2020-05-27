using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Beeffective.Core.Models;
using Beeffective.Presentation.Common;
using Beeffective.Presentation.Main.Dialogs;
using Beeffective.Presentation.Main.Goals;
using Beeffective.Presentation.Main.Priority;
using Beeffective.Presentation.NewGoal;

namespace Beeffective.Presentation.Main.TopBar
{
    [Export]
    public class TopBarViewModel : TaskCollectionViewModel
    {
        private readonly IDialogDisplay dialogDisplay;
        private string title;
        private bool isAddMenuOpen;
        private GoalViewModel newGoal;
        private const string DefaultTitle = "Beeffective";

        [ImportingConstructor]
        public TopBarViewModel(PriorityObservableCollection tasks, IDialogDisplay dialogDisplay) : base(tasks)
        {
            this.dialogDisplay = dialogDisplay;
            Tasks.PropertyChanged += OnTasksPropertyChanged;
            AddCommand = new DelegateCommand(Add);
            ShowAddGoalDialogCommand = new AsyncCommand(ShowAddGoalDialogAsync);
            AddGoalCommand = new DelegateCommand(CanAddGoal, AddGoal);
            Title = DefaultTitle;
        }

        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        private void OnTasksPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Title = Tasks.IsSelected ? Tasks.Selected.Model.Title : DefaultTitle;
        }

        public DelegateCommand AddCommand { get; }

        private void Add(object obj)
        {
            IsAddMenuOpen = true;
        }

        public bool IsAddMenuOpen
        {
            get => isAddMenuOpen;
            set => SetProperty(ref isAddMenuOpen, value);
        }

        public IAsyncCommand ShowAddGoalDialogCommand { get; }

        [Import]
        public INewGoalView NewGoalView { get; set; }

        private async Task ShowAddGoalDialogAsync()
        {
            NewGoal = new GoalViewModel(new GoalModel());
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
                NotifyPropertyChange();
            }
        }

        private void OnGoalModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(NewGoal.Model.Title))
            {
                AddGoalCommand.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand AddGoalCommand { get; }

        private bool CanAddGoal(object arg) => 
            !string.IsNullOrWhiteSpace(NewGoal?.Model.Title) && 
            Tasks.Goals.Select(g => g.Model.Title).All(t => t != NewGoal.Model.Title);

        private void AddGoal(object obj)
        {
            
        }
    }
}