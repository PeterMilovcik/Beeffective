using System;
using Beeffective.Presentation.Common;

namespace Beeffective.Presentation.Main.Record
{
    public class RecordViewModel : ViewModel
    {
        private DateTime startAt;
        private DateTime stopAt;

        public int Id { get; set; }
        
        public int TaskId { get; set; }

        public DateTime StartAt
        {
            get => startAt;
            set
            {
                if (SetProperty(ref startAt, value)) NotifyPropertyChange(nameof(Duration));
            }
        }

        public DateTime StopAt
        {
            get => stopAt;
            set
            {
                if (SetProperty(ref stopAt, value)) NotifyPropertyChange(nameof(Duration));
            }
        }

        public TimeSpan Duration => StopAt - StartAt;
    }
}