﻿using System.ComponentModel;
using System.ComponentModel.Composition;
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
            ShowNewLabelDialogCommand = new AsyncCommand(ShowNewLabelDialogAsync);
            SaveLabelCommand = new DelegateCommand(CanSaveLabel, async obj => await SaveLabelAsync());
        }

        public IAsyncCommand ShowNewLabelDialogCommand { get; }

        private async Task ShowNewLabelDialogAsync()
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
            !Core.LabelsCollection.Select(labelModel => labelModel.Title).Contains(NewLabel.Title);

        private async Task SaveLabelAsync()
        {
            var savedLabel = await repository.Labels.AddAsync(NewLabel);
            Core.LabelsCollection.Add(savedLabel);
            dialogDisplay.CloseDialog();
        }
    }
}