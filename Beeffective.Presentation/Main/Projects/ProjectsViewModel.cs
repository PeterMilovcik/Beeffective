using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Beeffective.Core.Extensions;
using Beeffective.Core.Models;
using Beeffective.Presentation.Common;
using Beeffective.Services.Repository;

namespace Beeffective.Presentation.Main.Projects
{
    public class ProjectsViewModel : CoreViewModel
    {
        private readonly IRepositoryService repository;
        private ProjectModel selected;
        private ObservableCollection<ProjectModel> selectedCollection;

        public ProjectsViewModel(Core core, IRepositoryService repository) : base(core)
        {
            this.repository = repository;
            Collection = new ObservableCollection<ProjectModel>();
            Collection.CollectionChanged += OnCollectionChanged;
            SelectAllCommand = new DelegateCommand((obj) => Selected = null);
            AddNewCommand = new AsyncCommand(AddNew);
        }

        public ObservableCollection<ProjectModel> Collection { get; }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Selected = null;
            SelectedCollection = Collection;
        }

        public ObservableCollection<ProjectModel> SelectedCollection
        {
            get => selectedCollection;
            set => SetProperty(ref selectedCollection, value).IfTrue(() =>
                Core.Tasks.SelectedCollection = new ObservableCollection<TaskModel>(
                    Core.Tasks.Collection.Where(t => SelectedCollection.Contains(t.Project))));
        }

        public ProjectModel Selected
        {
            get => selected;
            set => SetProperty(ref selected, value).IfTrue(() =>
            {
                Core.Tasks.SelectedCollection = Selected != null
                    ? new ObservableCollection<TaskModel>(
                        Core.Tasks.Collection.Where(p => p.Project == Selected))
                    : Core.Tasks.Collection;
            });
        }

        public DelegateCommand SelectAllCommand { get; }

        public AsyncCommand AddNewCommand { get; }
        
        public Action RefreshView { get; set; }

        private async Task AddNew()
        {
            var added = await repository.Projects.AddAsync(new ProjectModel());
            Collection.Add(added);
            SelectedCollection = Collection;
            Selected = added;
        }

        public async Task LoadAsync()
        {
            Collection.Clear();
            var projects = (await repository.Projects.LoadAsync())
                .OrderBy(gm => gm.Title).ToList();
            projects.ForEach(projectModel => Collection.Add(projectModel));
            SelectedCollection = Collection;
        }

        public async Task SaveAsync() => 
            await repository.Projects.SaveAsync(Collection.ToList());
    }
}