using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Beeffective.Core.Extensions;
using Beeffective.Core.Models;
using Beeffective.Presentation.Common;
using Beeffective.Services.Repository;

namespace Beeffective.Presentation.Main.Labels
{
    public class LabelsViewModel : CoreViewModel
    {
        private readonly IRepositoryService repository;
        private LabelModel selected;
        private ObservableCollection<LabelModel> selectedCollection;


        public LabelsViewModel(Core core, IRepositoryService repository) : base(core)
        {
            this.repository = repository;
            Collection = new ObservableCollection<LabelModel>();
            Collection.CollectionChanged += OnCollectionChanged;
            SelectAllCommand = new DelegateCommand(obj => Selected = null);
            AddNewCommand = new AsyncCommand(AddNewAsync);
        }

        public ObservableCollection<LabelModel> Collection { get; }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Selected = null;
            SelectedCollection = Collection;
        }

        public ObservableCollection<LabelModel> SelectedCollection
        {
            get => selectedCollection;
            set => SetProperty(ref selectedCollection, value);
        }

        public LabelModel Selected
        {
            get => selected;
            set => SetProperty(ref selected, value).IfTrue(() =>
            {
                Core.Tasks.SelectedCollection = Selected != null
                    ? new ObservableCollection<TaskModel>(
                        Core.Tasks.Collection.Where(p => p.Labels.Contains(Selected)))
                    : Core.Tasks.Collection;
            });
        }

        public DelegateCommand SelectAllCommand { get; }

        public AsyncCommand AddNewCommand { get; }
        
        public Action RefreshView { get; set; }

        private async Task AddNewAsync()
        {
            var added = await repository.Labels.AddAsync(new LabelModel());
            Collection.Add(added);
            SelectedCollection = Collection;
            Selected = added;
        }

        public async Task LoadAsync()
        {
            Collection.Clear();
            var labels = (await repository.Labels.LoadAsync())
                .OrderBy(gm => gm.Title).ToList();
            labels.ForEach(labelModel => Collection.Add(labelModel));
            SelectedCollection = Collection;
        }

        public async Task SaveAsync() => 
            await repository.Labels.SaveAsync(Collection.ToList());
    }
}