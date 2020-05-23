using System;
using System.Collections.Generic;
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
using Beeffective.Presentation.Main.TopBar;

namespace Beeffective.Presentation.Main
{
    [Export]
    public class MainViewModel : ViewModel
    {
        private readonly IMainView view;
        private ContentViewModel content;
        private List<ContentViewModel> contentViewModels;

        [ImportingConstructor]
        public MainViewModel(IMainView view)
        {
            this.view = view;
            this.view.Activated += OnViewActivated;
            this.view.Deactivated += OnViewDeactivated;
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
        public TopBarViewModel TopBar { get; set; }

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
        public AlwaysOnTopViewModel AlwaysOnTop { get; set; }

        public async Task ShowAsync()
        {
            view.Show();
            contentViewModels = new List<ContentViewModel>
                {New, Dashboard, Priority, Goals, Tags, Calendar, Settings};
            await LoadAsync();
        }

        [Import]
        public PriorityObservableCollection Tasks { get; set; }

        private async Task LoadAsync() => await Tasks.LoadAsync();

        private void OnViewActivated(object sender, EventArgs e) => AlwaysOnTop.Hide();

        private void OnViewDeactivated(object sender, EventArgs e) => AlwaysOnTop.Show();

        public async Task CloseAsync()
        {
            await Tasks.SaveAsync();
            AlwaysOnTop.Close();
        }
    }
}
