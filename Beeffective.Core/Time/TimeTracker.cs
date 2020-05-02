using System;
using System.ComponentModel.Composition;
using System.Timers;

namespace Beeffective.Core.Time
{
    [Export]
    public class TimeTracker
    {
        private readonly Timer timer;
        
        public TimeTracker()
        {
            timer = new Timer();
            timer.Interval = 1000;
            timer.Elapsed += OnTimerElapsed;
        }

        public DateTime StartTime { get; private set; }

        public DateTime StopTime { get; private set; }

        public bool IsEnabled => timer.Enabled;

        public void StartTimer()
        {
            StartTime = DateTime.Now;
            timer.Start();
            OnStarted();
        }

        protected virtual void OnStarted() => 
            Started?.Invoke(this, EventArgs.Empty);

        public event EventHandler Started;

        public void StopTimer()
        {
            timer.Stop();
            StopTime = DateTime.Now;
            OnStopped();
        }

        public event EventHandler Stopped;

        protected virtual void OnStopped() => 
            Stopped?.Invoke(this, EventArgs.Empty);

        private void OnTimerElapsed(object sender, ElapsedEventArgs e) => 
            OnTicked();

        protected virtual void OnTicked() => 
            Ticked?.Invoke(this, EventArgs.Empty);

        public event EventHandler Ticked;
    }
}