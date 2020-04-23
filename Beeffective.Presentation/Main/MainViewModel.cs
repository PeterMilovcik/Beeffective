using System.ComponentModel.Composition;
using System.Windows.Input;
using Beeffective.Presentation.Common;
using Beeffective.Presentation.Main.Dashboard;
using Beeffective.Presentation.Main.Goals;
using Beeffective.Presentation.Main.Priority;

namespace Beeffective.Presentation.Main
{
    [Export]
    public class MainViewModel : ViewModel
    {
        private readonly IMainView view;
        private ViewModel content;

        [ImportingConstructor]
        public MainViewModel(IMainView view)
        {
            this.view = view;
            view.DataContext = this;
            DashboardCommand = new DelegateCommand(o => Content = Dashboard);
            PriorityCommand = new DelegateCommand(o => Content = Priority);
            GoalsCommand = new DelegateCommand(o => Content = Goals);
        }

        public void Show()
        {
            view.Show();
            Content = Dashboard;
        }

        [Import]
        public DashboardViewModel Dashboard { get; set; }

        public ICommand DashboardCommand { get; }

        [Import]
        public PriorityViewModel Priority { get; set; }

        public ICommand PriorityCommand { get; }

        [Import]
        public GoalsViewModel Goals { get; set; }

        public ICommand GoalsCommand { get; }

        public ViewModel Content
        {
            get => content;
            set => SetProperty(ref content, value);
        }
    }
}
