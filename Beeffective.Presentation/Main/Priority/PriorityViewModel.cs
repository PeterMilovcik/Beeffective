using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Beeffective.Presentation.Common;
using Beeffective.Presentation.Main.Tasks;

namespace Beeffective.Presentation.Main.Priority
{
    [Export]
    public class PriorityViewModel : ContentViewModel
    {
        private ObservableCollection<TaskViewModel> priorityCollection;
        private ObservableCollection<TaskViewModel> urgencyCollection;
        private ObservableCollection<TaskViewModel> importanceCollection;

        [ImportingConstructor]
        public PriorityViewModel(PriorityObservableCollection tasks) : base(tasks)
        {
            Tasks.CollectionChanged += (sender, args) => Update();
        }

        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();
            Update();
        }

        private void Update()
        {
            PriorityCollection = new ObservableCollection<TaskViewModel>(
                Tasks.OrderBy(t => t.Model.Priority));
            UrgencyCollection = new ObservableCollection<TaskViewModel>(
                Tasks.OrderBy(t => t.Model.Urgency));
            ImportanceCollection = new ObservableCollection<TaskViewModel>(
                Tasks.OrderBy(t => t.Model.Importance));
        }

        public ObservableCollection<TaskViewModel> PriorityCollection
        {
            get => priorityCollection;
            set => SetProperty(ref priorityCollection, value);
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

        public void InsertImportanceBefore(TaskViewModel what, TaskViewModel before)
        {
            var oldIndex = ImportanceCollection.IndexOf(what);
            var newIndex = ImportanceCollection.IndexOf(before);
            ImportanceCollection.Move(oldIndex, newIndex);
            for (int i = 0; i < ImportanceCollection.Count; i++)
            {
                var taskViewModel = ImportanceCollection[i];
                taskViewModel.Model.Importance = i;
            }

            Update();
        }

        public void InsertUrgencyBefore(TaskViewModel what, TaskViewModel before)
        {
            var oldIndex = UrgencyCollection.IndexOf(what);
            var newIndex = UrgencyCollection.IndexOf(before);
            UrgencyCollection.Move(oldIndex, newIndex);
            for (int i = 0; i < UrgencyCollection.Count; i++)
            {
                var taskViewModel = UrgencyCollection[i];
                taskViewModel.Model.Urgency = i;
            }

            Update();
        }
    }
}
