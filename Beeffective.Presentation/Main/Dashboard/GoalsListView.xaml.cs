using System.Windows;

namespace Beeffective.Presentation.Main.Dashboard
{
    public partial class GoalsListView
    {
        public GoalsListView()
        {
            InitializeComponent();
        }

        private void DataGrid_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is DashboardViewModel viewModel)
            {
                viewModel.Core.Goals.RefreshView = DataGrid.View.Refresh;
            }
        }
    }
}
