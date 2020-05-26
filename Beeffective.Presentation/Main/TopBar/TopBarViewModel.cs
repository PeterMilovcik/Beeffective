using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Beeffective.Presentation.Common;
using Beeffective.Presentation.Main.Priority;
using Beeffective.Presentation.NewGoal;
using MaterialDesignThemes.Wpf;

namespace Beeffective.Presentation.Main.TopBar
{
    [Export]
    public class TopBarViewModel : TaskCollectionViewModel
    {
        private string title;
        private bool isAddMenuOpen;
        private const string DefaultTitle = "Beeffective";

        [ImportingConstructor]
        public TopBarViewModel(PriorityObservableCollection tasks) : base(tasks)
        {
            Tasks.PropertyChanged += OnTasksPropertyChanged;
            AddCommand = new DelegateCommand(Add);
            AddGoalCommand = new DelegateCommand(async obj => await AddGoalAsync());
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

        public DelegateCommand AddGoalCommand { get; }

        private async Task AddGoalAsync()
        {
            var newGoalView = new NewGoalView();
            await DialogHost.Show(newGoalView);
        }
    }
}