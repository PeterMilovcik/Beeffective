using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Beeffective.Core.Models;
using Beeffective.Presentation.Common;
using Beeffective.Presentation.Main.Dialogs;

namespace Beeffective.Presentation.Main.Labels
{
    [Export]
    public class NewLabelViewModel : CoreViewModel
    {
        private readonly IDialogDisplay dialogDisplay;
        private LabelModel newLabel;

        [ImportingConstructor]
        public NewLabelViewModel(Core core, IDialogDisplay dialogDisplay) : base(core)
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
            !Core.Labels.Select(labelModel => labelModel.Title).Contains(NewLabel.Title);

        private void SaveLabel(object obj)
        {
            Core.Labels.Add(NewLabel);
            dialogDisplay.CloseDialog();
        }
    }
}