using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Beeffective.Core.Models;
using Beeffective.Presentation.Common;
using Beeffective.Presentation.Main.Dialogs;
using Beeffective.Services.Repository;

namespace Beeffective.Presentation.Main.Labels
{
    public class NewLabelViewModel : CoreViewModel
    {
        private readonly IDialogDisplay dialogDisplay;
        private readonly IRepositoryService repository;
        private LabelModel newLabel;

        public NewLabelViewModel(Core core, IDialogDisplay dialogDisplay, IRepositoryService repository) : base(core)
        {
            this.dialogDisplay = dialogDisplay;
            this.repository = repository;
            ShowDialogCommand = new AsyncCommand(ShowDialogAsync);
            SaveCommand = new AsyncCommand(SaveAsync, CanSave);
        }

        public IAsyncCommand ShowDialogCommand { get; }

        private async Task ShowDialogAsync()
        {
            NewLabel = new LabelModel();
            await dialogDisplay.ShowNewLabelDialogAsync(this);
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
                SaveCommand.RaiseCanExecuteChanged();
                NotifyPropertyChange();
            }
        }

        private void OnGoalModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(NewLabel.Title))
            {
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public AsyncCommand SaveCommand { get; }

        private bool CanSave() =>
            !string.IsNullOrWhiteSpace(NewLabel?.Title) &&
            !Core.Labels.Collection.Select(labelModel => labelModel.Title).Contains(NewLabel.Title);

        private async Task SaveAsync()
        {
            var savedLabel = await repository.Labels.AddAsync(NewLabel);
            Core.Labels.Collection.Add(savedLabel);
            dialogDisplay.CloseDialog();
        }
    }
}