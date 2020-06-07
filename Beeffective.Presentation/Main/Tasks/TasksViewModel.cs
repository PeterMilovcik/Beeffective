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

namespace Beeffective.Presentation.Main.Tasks
{
    public class TasksViewModel : CoreViewModel
    {
        private readonly IRepositoryService repository;
        private TaskModel selected;
        private ObservableCollection<TaskModel> selectedCollection;
        private ObservableCollection<LabelViewModel> labelSelection;
        private ObservableCollection<TaskModel> unfinishedCollection;
        private ObservableCollection<TaskModel> finishedCollection;

        public TasksViewModel(Core core, IRepositoryService repository) : base(core)
        {
            this.repository = repository;
            Collection = new ObservableCollection<TaskModel>();
            Collection.CollectionChanged += OnCollectionChanged;
            SelectAllCommand = new DelegateCommand(obj => Selected = null);
            AddNewCommand = new AsyncCommand(AddNewAsync);
            EditCommand = new AsyncCommand(EditAsync);
            RemoveCommand = new AsyncCommand(RemoveAsync);
            FinishCommand = new AsyncCommand(FinishTaskAsync);
            AddDueToCommand = new DelegateCommand(AddDueTo);
        }

        public ObservableCollection<TaskModel> Collection { get; }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Selected = null;
            SelectedCollection = new ObservableCollection<TaskModel>(Collection);
        }

        public ObservableCollection<TaskModel> SelectedCollection
        {
            get => selectedCollection;
            set => SetProperty(ref selectedCollection, value).IfTrue(UpdateFinishedUnfinishedCollection);
        }

        private void UpdateFinishedUnfinishedCollection()
        {
            UnfinishedCollection = new ObservableCollection<TaskModel>(
                SelectedCollection.Where(t => t.IsFinished == false));
            FinishedCollection = new ObservableCollection<TaskModel>(
                SelectedCollection.Where(t => t.IsFinished));
        }

        public ObservableCollection<TaskModel> UnfinishedCollection
        {
            get => unfinishedCollection;
            set => SetProperty(ref unfinishedCollection, value);
        }

        public ObservableCollection<TaskModel> FinishedCollection
        {
            get => finishedCollection;
            set => SetProperty(ref finishedCollection, value);
        }

        public TaskModel Selected
        {
            get => selected;
            set => SetProperty(ref selected, value)
                .IfTrue(() =>
                {
                    NotifyPropertyChange(nameof(IsTaskSelected));
                    if (Selected != null)
                    {
                        LabelSelection = new ObservableCollection<LabelViewModel>(
                            Core.Labels.Collection.Select(l =>
                            {
                                var labelViewModel = new LabelViewModel(l);
                                labelViewModel.IsSelected = Selected.Labels.Contains(labelViewModel.Model);
                                return labelViewModel;
                            }));
                    }
                });
        }

        public ObservableCollection<LabelViewModel> LabelSelection
        {
            get => labelSelection;
            set
            {
                if (Equals(labelSelection, value)) return;
                labelSelection?.ToList().ForEach(label => label.PropertyChanged -= OnLabelPropertyChanged);
                labelSelection = value;
                labelSelection?.ToList().ForEach(label => label.PropertyChanged += OnLabelPropertyChanged);
                NotifyPropertyChange();
            }
        }

        private void OnLabelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (Core.Tasks.Selected == null) return;
            if (e.PropertyName == nameof(LabelViewModel.IsSelected))
            {
                if (sender is LabelViewModel labelViewModel)
                {
                    if (labelViewModel.IsSelected)
                    {
                        Core.Tasks.Selected.Labels.Add(labelViewModel.Model);
                    }
                    else
                    {
                        Core.Tasks.Selected.Labels.Remove(labelViewModel.Model);
                    }
                }
            }
        }

        public bool IsTaskSelected => Selected != null;

        public DelegateCommand SelectAllCommand { get; }

        public AsyncCommand AddNewCommand { get; }

        private async Task AddNewAsync()
        {
            var model = await repository.Tasks.AddAsync(new TaskModel());
            Add(model);
            SelectedCollection = new ObservableCollection<TaskModel>(Collection);
            Selected = model;
            await DialogHost.Show(
                new TaskView
                {
                    Width = 500,
                    Height = 500,
                    DataContext = this
                });
        }

        public AsyncCommand EditCommand { get; }

        private async Task EditAsync()
        {
            if (Selected == null) return;
            await DialogHost.Show(
                new TaskView
                {
                    Width = 500,
                    Height = 500,
                    DataContext = this
                });
        }

        public AsyncCommand RemoveCommand { get; }

        private async Task RemoveAsync()
        {
            if (Selected == null) return;
            Unsubscribe(Selected);
            await repository.Tasks.RemoveAsync(Selected);
            Collection.Remove(Selected);
            SelectedCollection = Collection;
            Selected = null;
        }

        public async Task LoadAsync()
        {
            Collection.ToList().ForEach(Unsubscribe);
            Collection.Clear();
            var tasks = (await repository.Tasks.LoadAsync())
                .OrderBy(gm => gm.DueTo).ToList();
            tasks.ForEach(Add);
            SelectedCollection = new ObservableCollection<TaskModel>(Collection);
        }

        public async Task SaveAsync() => 
            await repository.Tasks.SaveAsync(Collection.ToList());

        public AsyncCommand FinishCommand { get; }

        public Action UnfinishedTasksRefreshView { get; set; }

        public Action FinishedTasksRefreshView { get; set; }

        private async Task FinishTaskAsync()
        {
            if (Selected == null) return;
            Selected.IsFinished = !Selected.IsFinished;
            await repository.Tasks.UpdateAsync(Selected);
            Selected = null;
        }

        private void Add(TaskModel model)
        {
            Subscribe(model);
            Collection.Add(model);
        }

        private void Subscribe(TaskModel model) => model.PropertyChanged += OnTaskPropertyChanged;

        private void Unsubscribe(TaskModel model) => model.PropertyChanged -= OnTaskPropertyChanged;

        private void OnTaskPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(TaskModel.DueTo))
            {
                UnfinishedTasksRefreshView();
            }

            if (e.PropertyName == nameof(TaskModel.IsFinished))
            {
                UpdateFinishedUnfinishedCollection();
            }
        }

        public DelegateCommand AddDueToCommand { get; }

        private void AddDueTo(object obj)
        {
            if (Selected == null) return;
            if (int.TryParse(obj.ToString(), out var days))
            {
                Selected.DueTo = Selected.DueTo + TimeSpan.FromDays(days);
            }
        }
    }
}