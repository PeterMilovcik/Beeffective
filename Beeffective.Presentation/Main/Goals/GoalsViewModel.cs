using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Beeffective.Core.Extensions;
using Beeffective.Core.Models;
using Beeffective.Presentation.Common;
using Beeffective.Presentation.Main.Dialogs;
using Beeffective.Services.Repository;

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
            AddNewCommand = new AsyncCommand(AddNew);
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
        
        public Action Refresh { get; set; }

        private async Task AddNew()
        {
            var added = await repository.Goals.AddAsync(new GoalModel());
            Collection.Add(added);
            SelectedCollection = Collection;
            Selected = added;
        }

        public async Task LoadAsync()
        {
            Collection.Clear();
            var goals = (await repository.Goals.LoadAsync())
                .OrderBy(gm => gm.Title).ToList();
            goals.ForEach(goalModel => Collection.Add(goalModel));
            SelectedCollection = Collection;
        }

        public async Task SaveAsync() => 
            await repository.Goals.SaveAsync(Collection.ToList());
    }
}
