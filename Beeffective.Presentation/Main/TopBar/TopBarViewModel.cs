using System.ComponentModel.Composition;
using Beeffective.Presentation.Common;
using Beeffective.Presentation.Main.Goals;
using Beeffective.Presentation.Main.Labels;
using Beeffective.Presentation.Main.Projects;
using Beeffective.Presentation.Main.Tasks;

namespace Beeffective.Presentation.Main.TopBar
{
    [Export]
    public class TopBarViewModel : CoreViewModel
    {
        private string title;
        private bool isAddMenuOpen;
        private const string DefaultTitle = "Beeffective";

        [ImportingConstructor]
        public TopBarViewModel(Core core) : base(core)
        {
            AddCommand = new DelegateCommand(obj => IsAddMenuOpen = true);
            Title = DefaultTitle;
        }

        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        public DelegateCommand AddCommand { get; }

        public bool IsAddMenuOpen
        {
            get => isAddMenuOpen;
            set => SetProperty(ref isAddMenuOpen, value);
        }
    }
}