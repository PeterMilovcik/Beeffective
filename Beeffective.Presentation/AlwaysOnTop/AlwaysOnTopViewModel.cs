using System.ComponentModel;
using System.ComponentModel.Composition;
using Beeffective.Presentation.Common;
using Beeffective.Presentation.Main.Priority;

namespace Beeffective.Presentation.AlwaysOnTop
{
    [Export]
    public class AlwaysOnTopViewModel : ViewModel
    {
        private readonly IAlwaysOnTopWindow view;

        [ImportingConstructor]
        public AlwaysOnTopViewModel(IAlwaysOnTopWindow view, PriorityObservableCollection tasks)
        {
            this.view = view;
            this.view.DataContext = this;
            Tasks = tasks;
            Tasks.PropertyChanged += OnTasksPropertyChanged;
        }

        public PriorityObservableCollection Tasks { get; }

        private void OnTasksPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Tasks.IsSelected))
            {
                if (Tasks.IsSelected)
                {
                    view.Show();
                }
                else
                {
                    view.Hide();
                }
            }
        }

    }
}