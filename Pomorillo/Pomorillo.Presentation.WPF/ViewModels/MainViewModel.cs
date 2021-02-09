using Prism.Commands;
using System;
using System.Threading.Tasks;

namespace Pomorillo.Presentation.WPF.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private TimeSpan _duration;
        public TimeSpan Duration
        {
            get { return _duration; }
            set 
            { 
                _duration = value;
                OnPropertyChanged(); 
            }
        }

        private DateTime? _startTime;
        public DateTime? StartTime
        {
            get { return _startTime; }
            private set
            {
                _startTime = value;
                OnPropertyChanged();
            }
        }

        private TimeSpan _remainingTime;
        public TimeSpan RemainingTime
        {
            get { return _remainingTime; }
            private set
            {
                _remainingTime= value;
                OnPropertyChanged();
            }
        }

        private bool _running;
        public bool Running
        {
            get { return _running; }
            private set
            {
                _running = value;
                OnPropertyChanged();
                OnRunningChanged(_running);
            }
        }

        private readonly DelegateCommand _startCountdownCommand;
        public DelegateCommand StartCountdownCommand { get { return _startCountdownCommand; } }

        public MainViewModel()
        {
            _startCountdownCommand = new DelegateCommand(StartCountdown, () => !Running);
        }

        private void OnRunningChanged(bool obj)
        {
            _startCountdownCommand.RaiseCanExecuteChanged();
        }

        private async void StartCountdown()
        {
            Running = true;

            // NOTE: UTC times used internally to ensure proper operation
            // across Daylight Saving Time changes. An IValueConverter can
            // be used to present the user a local time.

            // NOTE: RemainingTime is the raw data. It may be desirable to
            // use an IValueConverter to always round up to the nearest integer
            // value for whatever is the least-significant component displayed
            // (e.g. minutes, seconds, milliseconds), so that the displayed
            // value doesn't reach the zero value until the timer has completed.

            DateTime startTime = DateTime.UtcNow, endTime = startTime + Duration;
            TimeSpan remainingTime, interval = TimeSpan.FromMilliseconds(100);

            StartTime = startTime;
            remainingTime = endTime - startTime;

            while (remainingTime > TimeSpan.Zero)
            {
                RemainingTime = remainingTime;
                if (RemainingTime < interval)
                {
                    interval = RemainingTime;
                }

                // NOTE: arbitrary update rate of 100 ms (initialized above). This
                // should be a value at least somewhat less than the minimum precision
                // displayed (e.g. here it's 1/10th the displayed precision of one
                // second), to avoid potentially distracting/annoying "stutters" in
                // the countdown.

                await Task.Delay(interval);
                remainingTime = endTime - DateTime.UtcNow;
            }

            RemainingTime = TimeSpan.Zero;
            StartTime = null;
            Running = false;
        }
    }
}
