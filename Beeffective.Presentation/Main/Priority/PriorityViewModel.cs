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
    public class PriorityViewModel : ContentViewModel
    {
        private readonly IRepositoryService repository;
        private ObservableCollection<TaskViewModel> urgencyCollection;
        private ObservableCollection<TaskViewModel> importanceCollection;

        [ImportingConstructor]
        public PriorityViewModel(IRepositoryService repository)
        {
            this.repository = repository;
        }

        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();
            Update();
        }

        [Import]
        public PriorityObservableCollection PriorityCollection { get; set; }

        private void Update()
        {
            UrgencyCollection = new ObservableCollection<TaskViewModel>(
                PriorityCollection.OrderBy(t => t.Model.Urgency));
            ImportanceCollection = new ObservableCollection<TaskViewModel>(
                PriorityCollection.OrderBy(t => t.Model.Importance));
        }

        public ObservableCollection<TaskViewModel> UrgencyCollection
        {
            get => urgencyCollection;
            set => SetProperty(ref urgencyCollection, value);
        }

        public ObservableCollection<TaskViewModel> ImportanceCollection
        {
            get => importanceCollection;
            set => SetProperty(ref importanceCollection, value);
        }

        public async Task InsertImportanceBefore(TaskViewModel what, TaskViewModel before)
        {
            try
            {
                IsBusy = true;
                var oldIndex = ImportanceCollection.IndexOf(what);
                var newIndex = ImportanceCollection.IndexOf(before);
                ImportanceCollection.Move(oldIndex, newIndex);
                for (int i = 0; i < ImportanceCollection.Count; i++)
                {
                    var taskViewModel = ImportanceCollection[i];
                    taskViewModel.Model.Importance = i;
                    await repository.UpdateTaskAsync(taskViewModel.Model);
                }

                Update();
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task InsertUrgencyBefore(TaskViewModel what, TaskViewModel before)
        {
            try
            {
                IsBusy = true;
                var oldIndex = UrgencyCollection.IndexOf(what);
                var newIndex = UrgencyCollection.IndexOf(before);
                UrgencyCollection.Move(oldIndex, newIndex);
                for (int i = 0; i < UrgencyCollection.Count; i++)
                {
                    var taskViewModel = UrgencyCollection[i];
                    taskViewModel.Model.Urgency = i;
                    await repository.UpdateTaskAsync(taskViewModel.Model);
                }

                Update();
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
