﻿using System.ComponentModel;
using System.ComponentModel.Composition;
using Beeffective.Presentation.Common;
using Beeffective.Presentation.Main.Priority;

namespace Beeffective.Presentation.Main.TopBar
{
    [Export]
    public class TopBarViewModel : TaskCollectionViewModel
    {
        private string title;
        private bool isAddMenuOpen;

        [ImportingConstructor]
        public TopBarViewModel(PriorityObservableCollection tasks) : base(tasks)
        {
            Tasks.PropertyChanged += OnTasksPropertyChanged;
            AddCommand = new DelegateCommand(Add);
        }

        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        private void OnTasksPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Title = Tasks.IsSelected ? Tasks.Selected.Model.Title : "Beeffective";
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
    }
}