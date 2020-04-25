using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Beeffective.Presentation.Adorners;
using Beeffective.Presentation.Main.Tasks;

namespace Beeffective.Presentation.Main.Priority
{
    public partial class PriorityView
    {
        private Point startPoint;
        private DragAdorner adorner;
        private AdornerLayer layer;
        private bool isDragOutOfScope = false;

        public PriorityView()
        {
            InitializeComponent();
        }

        private void ListView_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);
        }

        private void ListView_OnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var position = e.GetPosition(null);

                if (Math.Abs(position.X - startPoint.X) > SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(position.Y - startPoint.Y) > SystemParameters.MinimumVerticalDragDistance)
                {
                    BeginDrag(sender, e);
                }
            }
        }

        private void BeginDrag(object sender, MouseEventArgs e)
        {
            var listView = sender as ListView;
            var listViewItem = FindAncestor<ListViewItem>((DependencyObject)e.OriginalSource);

            if (listViewItem == null) return;

            var taskViewModel = (TaskViewModel)listView.ItemContainerGenerator.ItemFromContainer(listViewItem);

            InitialiseAdorner(listView, listViewItem);
            listView.PreviewDragOver += ListViewDragOver;
            listView.DragLeave += ListViewDragLeave;
            listView.DragEnter += ListViewDragEnter;

            var data = new DataObject("myFormat", taskViewModel);
            DragDrop.DoDragDrop(listView, data, DragDropEffects.Move);

            listView.PreviewDragOver -= ListViewDragOver;
            listView.DragLeave -= ListViewDragLeave;
            listView.DragEnter -= ListViewDragEnter;

            if (adorner != null)
            {
                AdornerLayer.GetAdornerLayer(listView)?.Remove(adorner);
                adorner = null;
            }
        }

        private async Task ListViewDropAsync(object sender, DragEventArgs e, bool isImportance)
        {
            var listView = sender as ListView;
            if (listView == null) return;
            if (e.Data.GetDataPresent("myFormat"))
            {
                var dragged = e.Data.GetData("myFormat") as TaskViewModel;
                if (dragged == null) return;
                var listViewItem = FindAncestor<ListViewItem>((DependencyObject)e.OriginalSource);

                if (listViewItem != null)
                {
                    var dropped = (TaskViewModel)listView.ItemContainerGenerator.ItemFromContainer(listViewItem);
                    Debug.WriteLine($"dragged = {dragged.Title}");
                    Debug.WriteLine($"dropped = {dropped.Title}");
                    if (DataContext is PriorityViewModel viewModel)
                    {
                        if (isImportance)
                            await viewModel.SwapImportanceAsync(dragged, dropped);
                        else
                            await viewModel.SwapUrgencyAsync(dragged, dropped);
                    }
                }
            }
        }


        private void InitialiseAdorner(ListView listView, ListViewItem listViewItem)
        {
            var brush = new VisualBrush(listViewItem);
            adorner = new DragAdorner(listViewItem, listViewItem.RenderSize, brush);
            adorner.Opacity = 0.75;
            layer = AdornerLayer.GetAdornerLayer(listView);
            layer?.Add(adorner);
        }


        private void ListViewDragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("myFormat") ||
                sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void ListViewQueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            if (isDragOutOfScope)
            {
                e.Action = DragAction.Cancel;
                e.Handled = true;
            }
        }


        private void ListViewDragLeave(object sender, DragEventArgs e)
        {
            var listView = sender as ListView;
            if (listView == null) return;
            if (Equals(e.OriginalSource, listView))
            {
                var p = e.GetPosition(listView);
                Rect r = VisualTreeHelper.GetContentBounds(listView);
                if (!r.Contains(p))
                {
                    isDragOutOfScope = true;
                    e.Handled = true;
                }
            }
        }

        private void ListViewDragOver(object sender, DragEventArgs args)
        {
            var listView = sender as ListView;
            if (listView == null) return;
            if (adorner != null)
            {
                adorner.OffsetLeft = args.GetPosition(listView).X;
                adorner.OffsetTop = args.GetPosition(listView).Y;
            }
        }

        private static T FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            do
            {
                if (current is T dependencyObject)
                {
                    return dependencyObject;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
        }

        private async void ImportanceListView_OnDrop(object sender, DragEventArgs e)
        {
            await ListViewDropAsync(sender, e, true);
        }

        private void ImportanceListView_OnDragEnter(object sender, DragEventArgs e)
        {
            ListViewDragEnter(sender, e);
        }

        private void UrgencyListView_OnDragEnter(object sender, DragEventArgs e)
        {
            ListViewDragEnter(sender, e);
        }

        private async void UrgencyListView_OnDrop(object sender, DragEventArgs e)
        {
            await ListViewDropAsync(sender, e, false);
        }
    }
}
