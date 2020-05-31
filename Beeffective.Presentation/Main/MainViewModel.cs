using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using System.Windows.Input;
using Beeffective.Presentation.AlwaysOnTop;
using Beeffective.Presentation.Common;
using Beeffective.Presentation.Main.Calendar;
using Beeffective.Presentation.Main.Dashboard;
using Beeffective.Presentation.Main.Settings;
using Beeffective.Presentation.Main.TopBar;

namespace Beeffective.Presentation.Main
{
    [Export]
    public class MainViewModel : CoreViewModel
    {
        private readonly IMainView view;
        private ContentViewModel content;
        public List<ContentViewModel> ContentViewModels { get; private set; }

        [ImportingConstructor]
        public MainViewModel(Core core, IMainView view) : base(core)
        {
            this.view = view;
            this.view.Activated += OnViewActivated;
            this.view.Deactivated += OnViewDeactivated;
            view.DataContext = this;
            DashboardCommand = new DelegateCommand(async o => await ChangeContentAsync(Dashboard));
            CalendarCommand = new DelegateCommand(async o => await ChangeContentAsync(Calendar));
            SettingsCommand = new DelegateCommand(async o => await ChangeContentAsync(Settings));
            ContentViewModels = new List<ContentViewModel>();
        }

        public async Task ChangeContentAsync(ContentViewModel viewModel)
        {
            IsBusy = true;
            try
            {
                ContentViewModels.ForEach(vm => vm.IsSelected = false);
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
        public DashboardViewModel Dashboard { get; set; }

        public ICommand DashboardCommand { get; }

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
            ContentViewModels = new List<ContentViewModel> {Dashboard, Calendar, Settings};
            await LoadAsync();
        }

        private async Task LoadAsync() => await Core.LoadAsync();

        private void OnViewActivated(object sender, EventArgs e) => AlwaysOnTop.Hide();

        private void OnViewDeactivated(object sender, EventArgs e) => AlwaysOnTop.Show();

        public async Task CloseAsync()
        {
            await Core.SaveAsync();
            AlwaysOnTop.Close();
        }
    }
}
