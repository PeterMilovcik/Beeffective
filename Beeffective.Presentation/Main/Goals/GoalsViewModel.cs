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

namespace Beeffective.Presentation.Main.Goals
{
    public class GoalsViewModel : ContentViewModel
    {
        private readonly IRepositoryService repository;
        private GoalModel selected;
        private ObservableCollection<GoalModel> selectedCollection;

        public GoalsViewModel(Core core, IRepositoryService repository) : base(core)
        {
            this.repository = repository;
            Collection = new ObservableCollection<GoalModel>();
            Collection.CollectionChanged += OnGoalsCollectionChanged;
            SelectAllCommand = new DelegateCommand((obj) => Selected = null);
            AddNewCommand = new AsyncCommand(AddNewAsync);
            EditCommand = new AsyncCommand(EditAsync);
            RemoveCommand = new AsyncCommand(RemoveAsync);
        }

        public ObservableCollection<GoalModel> Collection { get; }

        private void OnGoalsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Selected = null;
            SelectedCollection = Collection;
        }

        public ObservableCollection<GoalModel> SelectedCollection
        {
            get => selectedCollection;
            set => SetProperty(ref selectedCollection, value).IfTrue(() =>
                Core.Projects.SelectedCollection = new ObservableCollection<ProjectModel>(
                    Core.Projects.Collection.Where(p => SelectedCollection.Contains(p.Goal))));
        }

        public GoalModel Selected
        {
            get => selected;
            set => SetProperty(ref selected, value).IfTrue(() =>
            {
                Core.Projects.SelectedCollection = Selected != null
                    ? new ObservableCollection<ProjectModel>(
                        Core.Projects.Collection.Where(p => p.Goal == Selected))
                    : Core.Projects.Collection;
            });
        }

        public DelegateCommand SelectAllCommand { get; }

        public AsyncCommand AddNewCommand { get; }

        private async Task AddNewAsync()
        {
            var model = await repository.Goals.AddAsync(new GoalModel());
            Add(model);
            SelectedCollection = Collection;
            Selected = model;
            await DialogHost.Show(
                new GoalView
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
                new GoalView
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
            await repository.Goals.RemoveAsync(Selected);
            Collection.Remove(Selected);
            SelectedCollection = Collection;
            Selected = null;
        }

        public Action RefreshView { get; set; }

        public async Task LoadAsync()
        {
            Collection.ToList().ForEach(Unsubscribe);
            Collection.Clear();
            var goals = (await repository.Goals.LoadAsync())
                .OrderBy(gm => gm.Title).ToList();
            goals.ForEach(Add);
            SelectedCollection = Collection;
        }

        private void Add(GoalModel goalModel)
        {
            Subscribe(goalModel);
            Collection.Add(goalModel);
        }

        private void Subscribe(GoalModel model) => model.PropertyChanged += OnGoalModelPropertyChanged;
        
        private void Unsubscribe(GoalModel model) => model.PropertyChanged -= OnGoalModelPropertyChanged;

        private void OnGoalModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(GoalModel.Importance))
            {
                RefreshView();
            }
        }

        public async Task SaveAsync() => 
            await repository.Goals.SaveAsync(Collection.ToList());
    }
}
