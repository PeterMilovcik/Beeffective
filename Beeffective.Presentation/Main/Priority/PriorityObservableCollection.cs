using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Beeffective.Core;
using Beeffective.Presentation.Main.Tasks;
using Beeffective.Services.Repository;

namespace Beeffective.Presentation.Main.Priority
{
    [Export]
    public class PriorityObservableCollection :
        Observable,
        ICollection<TaskViewModel>,
        INotifyCollectionChanged
    {
        private readonly IRepositoryService repository;
        private TaskViewModel selected;
        private bool isSelected;
        private readonly ObservableCollection<TaskViewModel> collection;
        private ObservableCollection<string> goals;
        private ObservableCollection<string> tags;

        [ImportingConstructor]
        public PriorityObservableCollection(IRepositoryService repository)
        {
            this.repository = repository;
            collection = new ObservableCollection<TaskViewModel>();
            collection.CollectionChanged += OnCollectionChanged;
        }

        public TaskViewModel Selected
        {
            get => selected;
            set
            {
                if (SetProperty(ref selected, value))
                {
                    IsSelected = selected != null;
                    OnPropertyChanged(nameof(Selected));
                }
            }
        }

        public bool IsSelected
        {
            get => isSelected;
            set => SetProperty(ref isSelected, value);
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        protected virtual void OnCollectionChanged(
            object sender, NotifyCollectionChangedEventArgs e) =>
            CollectionChanged?.Invoke(this, e);

        public IEnumerator<TaskViewModel> GetEnumerator() => 
            collection.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => 
            GetEnumerator();

        public void Add(TaskViewModel item)
        {
            collection.Add(item);
            UpdateGoalsAndTasks();
        }

        public void Clear()
        {
            collection.Clear();
            UpdateGoalsAndTasks();
        }

        public bool Remove(TaskViewModel item)
        {
            bool result = collection.Remove(item);
            UpdateGoalsAndTasks();
            return result;
        }

        private void UpdateGoalsAndTasks()
        {
            Goals = new ObservableCollection<string>(GetGoals());
            Tags = new ObservableCollection<string>(GetTags());
        }

        private IEnumerable<string> GetGoals() => collection
            .Where(t => !string.IsNullOrWhiteSpace(t.Model.Goal))
            .Select(t => t.Model.Goal).Distinct();

        private IEnumerable<string> GetTags()
        {
            var result = new List<string>();
            foreach (var taskModel in collection.Where(t => !string.IsNullOrWhiteSpace(t.Model.Tags)))
            {
                result.AddRange(taskModel.Model.Tags.Trim().Split(" "));
            }

            return result.Distinct();
        }

        public bool Contains(TaskViewModel item) => 
            collection.Contains(item);

        public void CopyTo(TaskViewModel[] array, int arrayIndex) => 
            collection.CopyTo(array, arrayIndex);

        public int Count => collection.Count;

        public bool IsReadOnly => false;

        public ObservableCollection<string> Goals
        {
            get => goals;
            set => SetProperty(ref goals, value);
        }

        public ObservableCollection<string> Tags
        {
            get => tags;
            set => SetProperty(ref tags, value);
        }

        public async Task LoadAsync()
        {
            var list = (await repository.LoadTaskAsync())
                .Select(taskModel => new TaskViewModel(taskModel)).ToList()
                .OrderBy(vm => vm.Model.Priority).ToList();
            collection.Clear();
            list.ForEach(Add);
        }

        public async Task SaveAsync() => 
            await repository.SaveTaskAsync(collection.Select(taskViewModel => taskViewModel.Model).ToList());
    }
}
