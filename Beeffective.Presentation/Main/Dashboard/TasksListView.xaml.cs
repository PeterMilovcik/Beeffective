using System.Windows;

namespace Beeffective.Presentation.Main.Dashboard
{
    public partial class TasksListView
    {
        public TasksListView()
        {
            InitializeComponent();
        }

        private void UnfinishedDataGrid_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is DashboardViewModel viewModel)
            {
                viewModel.Core.Tasks.UnfinishedTasksRefresh = UnfinishedDataGrid.View.Refresh;
            }
        }

        private void FinishedDataGrid_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is DashboardViewModel viewModel)
            {
                viewModel.Core.Tasks.FinishedTasksRefresh = FinishedDataGrid.View.Refresh;
            }
        }
    }
}
