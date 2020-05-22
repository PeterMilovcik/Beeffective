using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Threading.Tasks;
using Beeffective.Presentation.Common;
using Beeffective.Presentation.Main.Priority;
using Beeffective.Presentation.Main.Tasks;

namespace Beeffective.Presentation.Main.Dashboard
{
    [Export]
    public class DashboardViewModel : ContentViewModel
    {
        private ObservableCollection<TaskViewModel> priorityCollection;

        [ImportingConstructor]
        public DashboardViewModel(PriorityObservableCollection tasks) : base(tasks)
        {
            Tasks.CollectionChanged += (sender, args) => Update();
            Tasks.Changed += (sender, args) => Update();
        }

        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();
            Update();
        }

        private void Update()
        {
            PriorityCollection = new ObservableCollection<TaskViewModel>(Tasks
                .Where(t => !t.Model.IsFinished)
                .OrderBy(t => t.Model.Priority));
        }

        public ObservableCollection<TaskViewModel> PriorityCollection
        {
            get => priorityCollection;
            set => SetProperty(ref priorityCollection, value);
        }
    }
}
