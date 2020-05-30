﻿using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Beeffective.Core.Models;
using Beeffective.Presentation.Common;
using Beeffective.Presentation.Main.Dialogs;
using Beeffective.Services.Repository;

namespace Beeffective.Presentation.Main.Goals
{
    public class NewGoalViewModel : CoreViewModel
    {
        private readonly IDialogDisplay dialogDisplay;
        private readonly IRepositoryService repository;
        private GoalModel newGoal;

        public NewGoalViewModel(Core core, IDialogDisplay dialogDisplay, IRepositoryService repository) : base(core)
        {
            this.dialogDisplay = dialogDisplay;
            this.repository = repository;
            ShowDialogCommand = new AsyncCommand(ShowNewGoalDialogAsync);
            SaveCommand = new AsyncCommand(SaveGoalAsync, CanSaveGoal);
        }

        public IAsyncCommand ShowDialogCommand { get; }

        private async Task ShowNewGoalDialogAsync()
        {
            NewGoal = new GoalModel();
            await dialogDisplay.ShowNewGoalDialogAsync(this);
        }

        public GoalModel NewGoal
        {
            get => newGoal;
            set
            {
                if (Equals(newGoal, value)) return;
                if (newGoal != null) newGoal.PropertyChanged -= OnGoalModelPropertyChanged;
                newGoal = value;
                if (newGoal != null) newGoal.PropertyChanged += OnGoalModelPropertyChanged;
                SaveCommand.RaiseCanExecuteChanged();
                NotifyPropertyChange();
            }
        }

        private void OnGoalModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(NewGoal.Title))
            {
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public AsyncCommand SaveCommand { get; }

        private bool CanSaveGoal() =>
            !string.IsNullOrWhiteSpace(NewGoal?.Title) &&
            !Core.GoalsCollection.Select(goalModel => goalModel.Title).Contains(NewGoal.Title);

        private async Task SaveGoalAsync()
        {
            var savedGoal = await repository.Goals.AddAsync(NewGoal);
            Core.GoalsCollection.Add(savedGoal);
            dialogDisplay.CloseDialog();
        }
    }
}