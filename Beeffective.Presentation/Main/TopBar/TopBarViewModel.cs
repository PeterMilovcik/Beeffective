using System.ComponentModel;
using System.ComponentModel.Composition;
using Beeffective.Presentation.Common;
using Beeffective.Presentation.Main.Goals;
using Beeffective.Presentation.Main.Labels;
using Beeffective.Presentation.Main.Priority;
using Beeffective.Presentation.Main.Projects;

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
            AddCommand = new DelegateCommand(obj => IsAddMenuOpen = true);
            Title = DefaultTitle;
        }

        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        private void OnTasksPropertyChanged(object sender, PropertyChangedEventArgs e) => 
            Title = Tasks.IsSelected ? Tasks.Selected.Model.Title : DefaultTitle;

        public DelegateCommand AddCommand { get; }

        public bool IsAddMenuOpen
        {
            get => isAddMenuOpen;
            set => SetProperty(ref isAddMenuOpen, value);
        }

        [Import]
        public NewGoalViewModel NewGoal { get; set; }

        [Import]
        public NewProjectViewModel NewProject { get; set; }

        [Import]
        public NewLabelViewModel NewLabel { get; set; }
    }
}