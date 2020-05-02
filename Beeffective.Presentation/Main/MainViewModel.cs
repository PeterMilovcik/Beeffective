using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using System.Windows.Input;
using Beeffective.Presentation.AlwaysOnTop;
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
        private ContentViewModel content;
        private List<ContentViewModel> contentViewModels;
        private IAlwaysOnTopWindow alwaysOnTopWindow;

        [ImportingConstructor]
        public MainViewModel(IMainView view)
        {
            this.view = view;
            view.DataContext = this;
            NewCommand = new DelegateCommand(async o => await ChangeContentAsync(New));
            DashboardCommand = new DelegateCommand(async o => await ChangeContentAsync(Dashboard));
            PriorityCommand = new DelegateCommand(async o => await ChangeContentAsync(Priority));
            GoalsCommand = new DelegateCommand(async o => await ChangeContentAsync(Goals));
            TagsCommand = new DelegateCommand(async o => await ChangeContentAsync(Tags));
            CalendarCommand = new DelegateCommand(async o => await ChangeContentAsync(Calendar));
            SettingsCommand = new DelegateCommand(async o => await ChangeContentAsync(Settings));
        }

        public async Task ChangeContentAsync(ContentViewModel viewModel)
        {
            IsBusy = true;
            try
            {
                contentViewModels.ForEach(vm => vm.IsSelected = false);
                Content = viewModel;
                Content.IsSelected = true;
                await Content.InitializeAsync();

            }
            finally
            {
                IsBusy = false;
            }
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

        public ContentViewModel Content
        {
            get => content;
            set => SetProperty(ref content, value);
        }

        [Import]
        public PriorityObservableCollection Tasks { get; set; }

        public IAlwaysOnTopWindow AlwaysOnTopWindow
        {
            get
            {
                if (alwaysOnTopWindow == null)
                {
                    // TODO: This is very bad! Find a good way how to fix this!
                    // Couldn't import it because of cyclic dependencies.
                    // maybe the PriorityObservableCollection could be passed there instead of this.
                    // Is the reference to MainViewModel really necessary?
                    alwaysOnTopWindow = new AlwaysOnTopWindow(this);
                }
                return alwaysOnTopWindow;
            }
        }

        public void Show()
        {
            view.Show();
            contentViewModels = new List<ContentViewModel>
                {New, Dashboard, Priority, Goals, Tags, Calendar, Settings};
            Tasks.PropertyChanged += OnTasksPropertyChanged;
        }

        private void OnTasksPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Tasks.IsSelected))
            {
                if (Tasks.IsSelected)
                {
                    AlwaysOnTopWindow.Show();
                }
                else
                {
                    AlwaysOnTopWindow.Hide();
                }
            }
        }
    }
}
