﻿using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Beeffective.Presentation.Main.Dialogs;
using Beeffective.Presentation.Main.Priority;
using Beeffective.Presentation.Main.Tasks;
using Beeffective.Services.Repository;
using MaterialDesignThemes.Wpf;

namespace Beeffective.Presentation.Common
{
    public class TaskCollectionViewModel : Initializable
    {
        [ImportingConstructor]
        public TaskCollectionViewModel(PriorityObservableCollection tasks)
        {
            Tasks = tasks;
        }

        public PriorityObservableCollection Tasks { get; }

        [Import]
        public IRepositoryService Repository { get; set; }

        public async Task LoadTaskAsync()
        {
            await Tasks.LoadAsync();
            Subscribe();
        }

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
                        await Repository.Tasks.RemoveAsync(taskViewModel.Model);
                        UnsubscribeFrom(taskViewModel);
                        Tasks.Remove(taskViewModel);
                    }
                    finally
                    {
                        IsBusy = false;
                    }
                }
            }
        }

        private void UnsubscribeFrom(TaskViewModel taskViewModel) =>
            taskViewModel.Removing -= OnTaskViewModelRemoving;

        public async Task SaveTaskAsync()
        {
            await Tasks.SaveAsync();
        }
    }
}