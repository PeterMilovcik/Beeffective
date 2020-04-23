using System.ComponentModel.Composition;
using System.Windows.Input;
using Beeffective.Presentation.Common;
using Beeffective.Presentation.Main.Dashboard;
using Beeffective.Presentation.Main.Priority;

namespace Beeffective.Presentation.Main
{
    [Export]
    public class MainViewModel : ViewModel
    {
        private readonly IMainView view;
        private ViewModel contentViewModel;

        [ImportingConstructor]
        public MainViewModel(IMainView view)
        {
            this.view = view;
            view.DataContext = this;
            DashboardCommand = new DelegateCommand(o => ContentViewModel = Dashboard);
            PriorityCommand = new DelegateCommand(o => ContentViewModel = Priority);
        }

        public void Show()
        {
            view.Show();
            ContentViewModel = Dashboard;
        }

        [Import]
        public DashboardViewModel Dashboard { get; set; }

        public ICommand DashboardCommand { get; }

        [Import]
        public PriorityViewModel Priority { get; set; }

        public ICommand PriorityCommand { get; }

        public ViewModel ContentViewModel
        {
            get => contentViewModel;
            set => SetProperty(ref contentViewModel, value);
        }
    }
}
