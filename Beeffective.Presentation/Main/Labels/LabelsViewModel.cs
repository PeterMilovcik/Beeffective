using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Beeffective.Core.Extensions;
using Beeffective.Core.Models;
using Beeffective.Presentation.Common;
using Beeffective.Presentation.Main.Dashboard;
using Beeffective.Services.Repository;
using MaterialDesignThemes.Wpf;

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
            EditCommand = new AsyncCommand(EditAsync);
            RemoveCommand = new AsyncCommand(RemoveAsync);
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

        private async Task AddNewAsync()
        {
            var model = await repository.Labels.AddAsync(new LabelModel());
            Add(model);
            SelectedCollection = Collection;
            Selected = model;
            await DialogHost.Show(
                new LabelView
                {
                    Width = 400,
                    Height = 300,
                    DataContext = this
                });
        }

        public AsyncCommand EditCommand { get; }

        private async Task EditAsync()
        {
            if (Selected == null) return;
            await DialogHost.Show(
                new LabelView
                {
                    Width = 400,
                    Height = 300,
                    DataContext = this
                });
        }

        public AsyncCommand RemoveCommand { get; }

        private async Task RemoveAsync()
        {
            if (Selected == null) return;
            Unsubscribe(Selected);
            await repository.Labels.RemoveAsync(Selected);
            Collection.Remove(Selected);
            SelectedCollection = Collection;
            Selected = null;
        }

        private void Add(LabelModel model)
        {
            Subscribe(model);
            Collection.Add(model);
        }

        private void Subscribe(LabelModel model) => model.PropertyChanged += OnModelPropertyChanged;

        private void Unsubscribe(LabelModel model) => model.PropertyChanged -= OnModelPropertyChanged;

        private void OnModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(LabelModel.TimeSpent))
            {
                RefreshView();
            }
        }

        public Action RefreshView { get; set; }

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