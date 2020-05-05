using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Beeffective.Presentation.Common;
using Beeffective.Presentation.Main.Record;

namespace Beeffective.Presentation.Main.Tasks
{
    public class TaskViewModel : ViewModel
    {
        private string title;
        private int urgency;
        private int importance;
        private string goal;
        private string tags;
        private ObservableCollection<RecordViewModel> records;

        public TaskViewModel()
        {
            RemoveCommand = new DelegateCommand(o => OnRemoving());
            Records = new ObservableCollection<RecordViewModel>();
        }


        public int Id { get; set; }

        public string Title
        {
            get => title;
            set
            {
                if (SetProperty(ref title, value)) IsChanged = true;
            }
        }

        public int Urgency
        {
            get => urgency;
            set
            {
                if (SetProperty(ref urgency, value))
                {
                    IsChanged = true;
                    OnPropertyChanged(nameof(Priority));
                }
            }
        }

        public int Importance
        {
            get => importance;
            set
            {
                if (SetProperty(ref importance, value))
                {
                    IsChanged = true;
                    OnPropertyChanged(nameof(Priority));
                }
            }
        }


        public double Priority => Math.Sqrt(Math.Pow(Urgency, 2) + Math.Pow(Importance, 2));

        public string Goal
        {
            get => goal;
            set
            {
                if (SetProperty(ref goal, value)) IsChanged = true;
            }
        }

        public string Tags
        {
            get => tags;
            set
            {
                if (SetProperty(ref tags, value)) IsChanged = true;
            }
        }

        public TimeSpan TimeSpent
        {
            get
            {
                var result = new TimeSpan();
                foreach (var recordViewModel in Records)
                {
                    result = result.Add(recordViewModel.Duration);
                }

                return result;
            }
        }


        public ICommand RemoveCommand { get; }

        public event EventHandler Removing;

        protected virtual void OnRemoving() => 
            Removing?.Invoke(this, EventArgs.Empty);

        public ObservableCollection<RecordViewModel> Records
        {
            get => records;
            set => SetProperty(ref records, value);
        }
    }
}