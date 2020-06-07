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
            AddNewCommand = new AsyncCommand(AddNewAsync);
            EditCommand = new AsyncCommand(EditAsync);
            RemoveCommand = new AsyncCommand(RemoveAsync);
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

        private async Task AddNewAsync()
        {
            var model = await repository.Projects.AddAsync(new ProjectModel());
            Add(model);
            SelectedCollection = Collection;
            Selected = model;
            await DialogHost.Show(
                new ProjectView
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
                new ProjectView
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
            await repository.Projects.RemoveAsync(Selected);
            Collection.Remove(Selected);
            SelectedCollection = Collection;
            Selected = null;
        }

        private void Add(ProjectModel projectModel)
        {
            Subscribe(projectModel);
            Collection.Add(projectModel);
        }

        private void Subscribe(ProjectModel model) => model.PropertyChanged += OnModelPropertyChanged;

        private void Unsubscribe(ProjectModel model) => model.PropertyChanged -= OnModelPropertyChanged;

        private void OnModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ProjectModel.TimeSpent))
            {
                RefreshView();
            }
        }

        public Action RefreshView { get; set; }

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