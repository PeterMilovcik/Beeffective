using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Beeffective.Core.Models;
using Beeffective.Presentation.Common;
using Beeffective.Presentation.Main.Dialogs;
using Beeffective.Presentation.Main.Priority;

namespace Beeffective.Presentation.Main.Labels
{
    [Export]
    public class NewLabelViewModel : TaskCollectionViewModel
    {
        private readonly IDialogDisplay dialogDisplay;
        private LabelModel newLabel;

        [ImportingConstructor]
        public NewLabelViewModel(PriorityObservableCollection tasks, IDialogDisplay dialogDisplay) : base(tasks)
        {
            this.dialogDisplay = dialogDisplay;
            ShowNewLabelDialogCommand = new AsyncCommand(ShowNewLabelDialogAsync);
            SaveLabelCommand = new DelegateCommand(CanSaveLabel, SaveLabel);
        }

        [Import]
        public INewLabelView NewLabelView { get; set; }

        public IAsyncCommand ShowNewLabelDialogCommand { get; }

        private async Task ShowNewLabelDialogAsync()
        {
            NewLabel = new LabelModel();
            NewLabelView.DataContext = this;
            await dialogDisplay.ShowAsync(NewLabelView);
        }

        public LabelModel NewLabel
        {
            get => newLabel;
            set
            {
                if (Equals(newLabel, value)) return;
                if (newLabel != null) newLabel.PropertyChanged -= OnGoalModelPropertyChanged;
                newLabel = value;
                if (newLabel != null) newLabel.PropertyChanged += OnGoalModelPropertyChanged;
                SaveLabelCommand.RaiseCanExecuteChanged();
                NotifyPropertyChange();
            }
        }

        private void OnGoalModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(NewLabel.Title))
            {
                SaveLabelCommand.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand SaveLabelCommand { get; }

        private bool CanSaveLabel(object arg) =>
            !string.IsNullOrWhiteSpace(NewLabel?.Title) &&
            !Tasks.Labels.Select(labelModel => labelModel.Title).Contains(NewLabel.Title);

        private void SaveLabel(object obj)
        {
            Tasks.Labels.Add(NewLabel);
            dialogDisplay.CloseDialog();
        }
    }
}