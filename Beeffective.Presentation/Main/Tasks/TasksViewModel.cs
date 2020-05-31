using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Beeffective.Core.Extensions;
using Beeffective.Core.Models;
using Beeffective.Presentation.Common;
using Beeffective.Presentation.Main.Dialogs;
using Beeffective.Services.Repository;

namespace Beeffective.Presentation.Main.Tasks
{
    public class TasksViewModel : CoreViewModel
    {
        private readonly IRepositoryService repository;
        private TaskModel selected;
        private ObservableCollection<TaskModel> selectedCollection;

        public TasksViewModel(Core core, IDialogDisplay dialogDisplay, IRepositoryService repository) : base(core)
        {
            this.repository = repository;
            Collection = new ObservableCollection<TaskModel>();
            Collection.CollectionChanged += OnCollectionChanged;
            SelectAllCommand = new DelegateCommand(obj => Selected = null);
            New = new NewTaskViewModel(core, dialogDisplay, repository);
        }


        public ObservableCollection<TaskModel> Collection { get; }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Selected = null;
            SelectedCollection = Collection;
        }

        public ObservableCollection<TaskModel> SelectedCollection
        {
            get => selectedCollection;
            set => SetProperty(ref selectedCollection, value);
        }

        public TaskModel Selected
        {
            get => selected;
            set => SetProperty(ref selected, value).IfTrue(() => NotifyPropertyChange(nameof(IsTaskSelected)));
        }

        public bool IsTaskSelected => Selected != null;

        public DelegateCommand SelectAllCommand { get; }
        
        public NewTaskViewModel New { get; }

        public async Task LoadAsync()
        {
            Collection.Clear();
            var tasks = (await repository.Tasks.LoadAsync())
                .OrderBy(gm => gm.Title).ToList();
            tasks.ForEach(labelModel => Collection.Add(labelModel));
            SelectedCollection = Collection;
        }

        public async Task SaveAsync() => 
            await repository.Tasks.SaveAsync(Collection.ToList());
    }
}