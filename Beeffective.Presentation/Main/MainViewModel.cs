using System.ComponentModel.Composition;
using System.Windows.Input;
using Beeffective.Presentation.Common;
using Beeffective.Presentation.Main.Calendar;
using Beeffective.Presentation.Main.Dashboard;
using Beeffective.Presentation.Main.Goals;
using Beeffective.Presentation.Main.New;
using Beeffective.Presentation.Main.Priority;
using Beeffective.Presentation.Main.Settings;
using Beeffective.Presentation.Main.Tags;

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
            NewCommand = new DelegateCommand(o => Content = New);
            DashboardCommand = new DelegateCommand(o => Content = Dashboard);
            PriorityCommand = new DelegateCommand(o => Content = Priority);
            GoalsCommand = new DelegateCommand(o => Content = Goals);
            TagsCommand = new DelegateCommand(o => Content = Tags);
            CalendarCommand = new DelegateCommand(o => Content = Calendar);
            SettingsCommand = new DelegateCommand(o => Content = Settings);
        }

        public void Show()
        {
            view.Show();
            Content = Dashboard;
        }

        [Import]
        public NewViewModel New { get; set; }

        public ICommand NewCommand { get; }

        [Import]
        public DashboardViewModel Dashboard { get; set; }

        public ICommand DashboardCommand { get; }

        [Import]
        public PriorityViewModel Priority { get; set; }

        public ICommand PriorityCommand { get; }

        [Import]
        public GoalsViewModel Goals { get; set; }

        public ICommand GoalsCommand { get; }

        [Import]
        public TagsViewModel Tags { get; set; }

        public ICommand TagsCommand { get; }

        [Import]
        public CalendarViewModel Calendar { get; set; }

        public ICommand CalendarCommand { get; }

        [Import]
        public SettingsViewModel Settings { get; set; }

        public ICommand SettingsCommand { get; }

        public ViewModel Content
        {
            get => content;
            set => SetProperty(ref content, value);
        }
    }
}
