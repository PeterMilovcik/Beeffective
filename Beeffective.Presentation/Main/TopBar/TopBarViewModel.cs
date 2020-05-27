﻿using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Beeffective.Presentation.Common;
using Beeffective.Presentation.Main.Priority;
using Beeffective.Presentation.NewGoal;
using MaterialDesignThemes.Wpf;

namespace Beeffective.Presentation.Main.TopBar
{
    [Export]
    public class TopBarViewModel : TaskCollectionViewModel
    {
        private string title;
        private bool isAddMenuOpen;
        private bool isAddGoalDialogOpen;
        private const string DefaultTitle = "Beeffective";

        [ImportingConstructor]
        public TopBarViewModel(PriorityObservableCollection tasks) : base(tasks)
        {
            Tasks.PropertyChanged += OnTasksPropertyChanged;
            AddCommand = new DelegateCommand(Add);
            ShowAddGoalDialogCommand = new DelegateCommand(async obj => await ShowAddGoalDialogAsync());
            Title = DefaultTitle;
        }

        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        private void OnTasksPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Title = Tasks.IsSelected ? Tasks.Selected.Model.Title : DefaultTitle;
        }

        public DelegateCommand AddCommand { get; }

        private void Add(object obj)
        {
            IsAddMenuOpen = true;
        }

        public bool IsAddMenuOpen
        {
            get => isAddMenuOpen;
            set => SetProperty(ref isAddMenuOpen, value);
        }

        public DelegateCommand ShowAddGoalDialogCommand { get; }

        private async Task ShowAddGoalDialogAsync()
        {
            var newGoalView = new NewGoalView();
            await DialogHost.Show(newGoalView);
        }
    }
}