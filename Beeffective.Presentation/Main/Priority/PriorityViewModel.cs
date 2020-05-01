using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Beeffective.Presentation.Common;
using Beeffective.Presentation.Main.Dialogs;
using Beeffective.Presentation.Main.Tasks;
using Beeffective.Services.Repository;
using MaterialDesignThemes.Wpf;

namespace Beeffective.Presentation.Main.Priority
{
    [Export]
    public class PriorityViewModel : ContentViewModel
    {
        private readonly IRepositoryService repository;
        private ObservableCollection<TaskViewModel> priorityCollection;
        private ObservableCollection<TaskViewModel> urgencyCollection;
        private ObservableCollection<TaskViewModel> importanceCollection;
        private List<TaskViewModel> taskViewModels;

        [ImportingConstructor]
        public PriorityViewModel(IRepositoryService repository)
        {
            this.repository = repository;
        }

        public override async Task InitializeAsync()
        {
            var taskModels = await repository.LoadTaskAsync();
            taskViewModels = taskModels.Select(m => m.ToViewModel()).ToList();

            Subscribe();
            Update();
        }

        private void Subscribe() => taskViewModels.ForEach(Subscribe);

        private void Subscribe(TaskViewModel taskViewModel) => taskViewModel.Removing += OnTaskViewModelRemoving;

        private void Update()
        {
            PriorityCollection = new ObservableCollection<TaskViewModel>(
                taskViewModels.OrderBy(t => t.Priority));
            UrgencyCollection = new ObservableCollection<TaskViewModel>(
                taskViewModels.OrderBy(t => t.Urgency));
            ImportanceCollection = new ObservableCollection<TaskViewModel>(
                taskViewModels.OrderBy(t => t.Importance));
        }

        public ObservableCollection<TaskViewModel> PriorityCollection
        {
            get => priorityCollection;
            set => SetProperty(ref priorityCollection, value);
        }

        public ObservableCollection<TaskViewModel> UrgencyCollection
        {
            get => urgencyCollection;
            set => SetProperty(ref urgencyCollection, value);
        }

        public ObservableCollection<TaskViewModel> ImportanceCollection
        {
            get => importanceCollection;
            set => SetProperty(ref importanceCollection, value);
        }

        public async Task InsertImportanceBefore(TaskViewModel what, TaskViewModel before)
        {
            try
            {
                IsBusy = true;
                var oldIndex = ImportanceCollection.IndexOf(what);
                var newIndex = ImportanceCollection.IndexOf(before);
                ImportanceCollection.Move(oldIndex, newIndex);
                for (int i = 0; i < ImportanceCollection.Count; i++)
                {
                    var taskViewModel = ImportanceCollection[i];
                    taskViewModel.Importance = i;
                    await repository.UpdateTaskAsync(taskViewModel.ToModel());
                }

                Update();
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task InsertUrgencyBefore(TaskViewModel what, TaskViewModel before)
        {
            try
            {
                IsBusy = true;
                var oldIndex = UrgencyCollection.IndexOf(what);
                var newIndex = UrgencyCollection.IndexOf(before);
                UrgencyCollection.Move(oldIndex, newIndex);
                for (int i = 0; i < UrgencyCollection.Count; i++)
                {
                    var taskViewModel = UrgencyCollection[i];
                    taskViewModel.Urgency = i;
                    await repository.UpdateTaskAsync(taskViewModel.ToModel());
                }

                Update();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void OnTaskViewModelRemoving(object sender, EventArgs e)
        {
            if (sender is TaskViewModel taskViewModel)
            {
                var confirmationDialog = new AreYouSureDialog();
                var result = await DialogHost.Show(confirmationDialog);
                if (result is true)
                {
                    try
                    {
                        IsBusy = true;
                        await repository.RemoveTaskAsync(taskViewModel.ToModel());
                        Unsubscribe(taskViewModel);
                        taskViewModels.Remove(taskViewModel);
                        Update();
                    }
                    finally
                    {
                        IsBusy = false;
                    }
                }
            }
        }

        private void Unsubscribe(TaskViewModel taskViewModel) => 
            taskViewModel.Removing -= OnTaskViewModelRemoving;
    }
}
