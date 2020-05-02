using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Beeffective.Presentation.Common;
using Beeffective.Presentation.Main.Dialogs;
using Beeffective.Presentation.Main.Priority;
using Beeffective.Presentation.Main.Tasks;
using Beeffective.Services.Repository;
using MaterialDesignThemes.Wpf;

namespace Beeffective.Presentation.Main.Dashboard
{
    [Export]
    public class DashboardViewModel : ContentViewModel
    {
        private readonly IRepositoryService repository;

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
                await Tasks.UpdateAsync();
                Subscribe();
            }
            finally
            {
                IsBusy = false;
            }
        }

        [Import]
        public PriorityObservableCollection Tasks { get; set; }

        private void Subscribe() => Tasks.ToList().ForEach(Subscribe);

        private void Subscribe(TaskViewModel taskViewModel) => taskViewModel.Removing += OnTaskViewModelRemoving;

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
                        Tasks.Remove(taskViewModel);
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
