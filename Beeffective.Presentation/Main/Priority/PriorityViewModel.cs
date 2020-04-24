using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Beeffective.Presentation.Common;
using Beeffective.Presentation.Main.Tasks;
using Beeffective.Services.Repository;

namespace Beeffective.Presentation.Main.Priority
{
    [Export]
    public class PriorityViewModel : ViewModel
    {
        private readonly IRepositoryService repository;
        private ObservableCollection<TaskViewModel> tasks;

        [ImportingConstructor]
        public PriorityViewModel(IRepositoryService repository)
        {
            this.repository = repository;
        }

        public override async Task InitializeAsync()
        {
            var taskModels = await repository.LoadTaskAsync();
            Tasks = new ObservableCollection<TaskViewModel>(taskModels.Select(m => m.ToViewModel()));
        }

        public ObservableCollection<TaskViewModel> Tasks
        {
            get => tasks;
            private set => SetProperty(ref tasks, value);
        }

        public void SwapImportance(TaskViewModel from, TaskViewModel to)
        {
            
        }

        public void SwapUrgency(TaskViewModel from, TaskViewModel to)
        {
            
        }
    }
}
