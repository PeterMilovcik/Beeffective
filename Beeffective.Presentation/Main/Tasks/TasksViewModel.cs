using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
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
        private ObservableCollection<LabelViewModel> labelSelection;

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
        
        public NewTaskViewModel New { get; }

        public async Task LoadAsync()
        {
            Collection.Clear();
            var tasks = (await repository.Tasks.LoadAsync())
                .OrderBy(gm => gm.DueTo).ToList();
            tasks.ForEach(labelModel => Collection.Add(labelModel));
            SelectedCollection = Collection;
        }

        public async Task SaveAsync() => 
            await repository.Tasks.SaveAsync(Collection.ToList());
    }
}