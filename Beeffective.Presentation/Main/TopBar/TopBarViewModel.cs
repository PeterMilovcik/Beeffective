using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Beeffective.Presentation.Common;
using Beeffective.Presentation.Main.Dialogs;
using Beeffective.Presentation.Main.Goals;
using Beeffective.Presentation.Main.Priority;
using Beeffective.Presentation.NewGoal;
using MaterialDesignThemes.Wpf;

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
            NewGoal = new GoalViewModel();
            NewGoalView.DataContext = this;
            await dialogDisplay.ShowAsync(NewGoalView);
        }

        public GoalViewModel NewGoal
        {
            get => newGoal;
            set => SetProperty(ref newGoal, value);
        }
    }
}