using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using System.Windows;
using Beeffective.Presentation.Common;
using Beeffective.Presentation.Main.Dialogs;
using Beeffective.Presentation.Main.Goals;
using Beeffective.Presentation.Main.Labels;
using Beeffective.Presentation.Main.Projects;
using Beeffective.Presentation.Main.Tasks;
using Beeffective.Services.Repository;

namespace Beeffective.Presentation.Main
{
    [Export]
    public class Core : ViewModel
    {
        [ImportingConstructor]
        public Core(IRepositoryService repository, IDialogDisplay dialogDisplay)
        {
            Goals = new GoalsViewModel(this, repository);
            Projects = new ProjectsViewModel(this, repository);
            Labels = new LabelsViewModel(this, repository);
            Tasks = new TasksViewModel(this, repository);
        }

        public GoalsViewModel Goals { get; }

        public ProjectsViewModel Projects { get; }

        public LabelsViewModel Labels { get; }

        public TasksViewModel Tasks { get; }

        public async Task LoadAsync()
        {
            try
            {
                IsBusy = true;
                await Goals.LoadAsync();
                await Projects.LoadAsync();
                await Labels.LoadAsync();
                await Tasks.LoadAsync();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task SaveAsync()
        {
            try
            {
                IsBusy = true;
                await Goals.SaveAsync();
                await Projects.SaveAsync();
                await Labels.SaveAsync();
                await Tasks.SaveAsync();
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
