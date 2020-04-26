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
    public class PriorityViewModel : ViewModel
    {
        private readonly IRepositoryService repository;
        private ObservableCollection<TaskViewModel> priorityCollection;
        private ObservableCollection<TaskViewModel> urgencyCollection;
        private ObservableCollection<TaskViewModel> importanceCollection;
        private List<TaskViewModel> tasks;

        [ImportingConstructor]
        public PriorityViewModel(IRepositoryService repository)
        {
            this.repository = repository;
        }

        public override async Task InitializeAsync()
        {
            var taskModels = await repository.LoadTaskAsync();
            tasks = taskModels.Select(m => m.ToViewModel()).ToList();
            SubscribeTo(tasks);
            Update(tasks);
        }

        private void SubscribeTo(List<TaskViewModel> taskViewModels) => 
            taskViewModels.ForEach(taskViewModel => taskViewModel.Removing += OnTaskViewModelRemoving);

        private void Update(List<TaskViewModel> tasks)
        {
            PriorityCollection = new ObservableCollection<TaskViewModel>(
                tasks.OrderBy(t => t.Priority));
            UrgencyCollection = new ObservableCollection<TaskViewModel>(
                tasks.OrderBy(t => t.Urgency));
            ImportanceCollection = new ObservableCollection<TaskViewModel>(
                tasks.OrderBy(t => t.Importance));
        }

        public ObservableCollection<TaskViewModel> PriorityCollection
        {
            get => priorityCollection;
            private set => SetProperty(ref priorityCollection, value);
        }

        public ObservableCollection<TaskViewModel> UrgencyCollection
        {
            get => urgencyCollection;
            private set => SetProperty(ref urgencyCollection, value);
        }

        public ObservableCollection<TaskViewModel> ImportanceCollection
        {
            get => importanceCollection;
            set => SetProperty(ref importanceCollection, value);
        }

        public async Task SwapImportanceAsync(TaskViewModel from, TaskViewModel to)
        {
            var importance = to.Importance;
            to.Importance = from.Importance;
            from.Importance = importance;
            await repository.UpdateTaskAsync(from.ToModel());
            await repository.UpdateTaskAsync(to.ToModel());
            Update(tasks);
        }

        public async Task SwapUrgencyAsync(TaskViewModel from, TaskViewModel to)
        {
            var urgency = to.Urgency;
            to.Urgency = from.Urgency;
            from.Urgency = urgency;
            await repository.UpdateTaskAsync(from.ToModel());
            await repository.UpdateTaskAsync(to.ToModel());
            Update(tasks);
        }

        private async void OnTaskViewModelRemoving(object sender, EventArgs e)
        {
            if (sender is TaskViewModel taskViewModel)
            {
                var confirmationDialog = new AreYouSureDialog();
                var result = await DialogHost.Show(confirmationDialog);
                if (result is true)
                {
                    await repository.RemoveTaskAsync(taskViewModel.ToModel());
                    UnsubscribeFrom(taskViewModel);
                    tasks.Remove(taskViewModel);
                    Update(tasks);
                }
            }
        }

        private void UnsubscribeFrom(TaskViewModel taskViewModel) => 
            taskViewModel.Removing -= OnTaskViewModelRemoving;
    }
}
