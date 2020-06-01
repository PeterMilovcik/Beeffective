using System;
using Beeffective.Core.Extensions;

namespace Beeffective.Core.Models
{
    public class RecordModel : Changeable
    {
        private DateTime startAt;
        private DateTime stopAt;
        public int Id { get; set; }
        
        public int TaskId { get; set; }

        public DateTime StartAt
        {
            get => startAt;
            set => SetProperty(ref startAt, value).IfTrue(() =>
            {
                NotifyPropertyChange(nameof(Duration));
            });
        }

        public DateTime StopAt
        {
            get => stopAt;
            set => SetProperty(ref stopAt, value).IfTrue(() =>
            {
                NotifyPropertyChange(nameof(Duration));
            });
        }

        public TimeSpan Duration => StopAt - StartAt;
    }
}
