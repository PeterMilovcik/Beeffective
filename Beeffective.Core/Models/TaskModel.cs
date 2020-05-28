using System;
using System.Collections.ObjectModel;

namespace Beeffective.Core.Models
{
    public class TaskModel : Observable
    {
        private string title;
        private string description;
        private DateTime? dueTo;
        private ProjectModel project;
        private bool isFinished;

        public TaskModel()
        {
            Records = new ObservableCollection<RecordModel>();
        }

        public int Id { get; set; }

        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public ObservableCollection<RecordModel> Records { get; }

        public DateTime? DueTo
        {
            get => dueTo;
            set => SetProperty(ref dueTo, value);
        }

        public ProjectModel Project
        {
            get => project;
            set => SetProperty(ref project, value);
        }

        public bool IsFinished
        {
            get => isFinished;
            set => SetProperty(ref isFinished, value);
        }
    }
}
