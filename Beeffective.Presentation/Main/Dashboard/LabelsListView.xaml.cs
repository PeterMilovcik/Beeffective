using System.Windows;

namespace Beeffective.Presentation.Main.Dashboard
{
    public partial class LabelsListView
    {
        public LabelsListView()
        {
            InitializeComponent();
        }

        private void DataGrid_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is DashboardViewModel viewModel)
            {
                viewModel.Core.Labels.RefreshView = DataGrid.View.Refresh;
            }
        }
    }
}
