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

namespace Beeffective.Presentation.Main.Dashboard
{
    [Export]
    public class DashboardViewModel : ContentViewModel
    {
        private readonly IRepositoryService repository;
        private List<TaskViewModel> taskViewModels;
        private ObservableCollection<TaskViewModel> tasks;
        private TaskViewModel selectedTask;

        [ImportingConstructor]
        public DashboardViewModel(IRepositoryService repository)
        {
            this.repository = repository;
        }

        public override async Task InitializeAsync()
        {
            try
            {
                IsBusy = true;  
                var taskModels = await repository.LoadTaskAsync();
                taskViewModels = taskModels.Select(m => m.ToViewModel()).ToList();

                Subscribe();
                Update();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void Subscribe() => taskViewModels.ForEach(Subscribe);

        private void Subscribe(TaskViewModel taskViewModel) => taskViewModel.Removing += OnTaskViewModelRemoving;

        private void Update()
        {
            Tasks = new ObservableCollection<TaskViewModel>(
                taskViewModels.OrderBy(t => t.Priority));
        }

        public ObservableCollection<TaskViewModel> Tasks
        {
            get => tasks;
            set => SetProperty(ref tasks, value);
        }

        public TaskViewModel SelectedTask
        {
            get => selectedTask;
            set => SetProperty(ref selectedTask, value);
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
