using System.Windows;

namespace Beeffective.Presentation.Main.Dashboard
{
    public partial class ProjectsListView
    {
        public ProjectsListView()
        {
            InitializeComponent();
        }

        private void DataGrid_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is DashboardViewModel viewModel)
            {
                viewModel.Core.Projects.RefreshView = DataGrid.View.Refresh;
            }
        }
    }
}
