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

        public void Add(TaskViewModel item) => 
            collection.Add(item);

        public void Clear() => 
            collection.Clear();

        public bool Contains(TaskViewModel item) => 
            collection.Contains(item);

        public void CopyTo(TaskViewModel[] array, int arrayIndex) => 
            collection.CopyTo(array, arrayIndex);

        public bool Remove(TaskViewModel item) => 
            collection.Remove(item);

        public int Count => collection.Count;

        public bool IsReadOnly => false;

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
