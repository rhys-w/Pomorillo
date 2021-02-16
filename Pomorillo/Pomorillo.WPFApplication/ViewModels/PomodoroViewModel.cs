using Prism.Commands;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Pomorillo.WPFApplication.ViewModels
{
    public class PomodoroViewModel : ViewModelBase
    {
        #region private fields

        private int _workingTimeMins;
        private int _breakTimeMins;
        private bool _isCancelEnabled;
        private bool _isRunning;
        private TimeSpan _remainingTime;
        private int _maxWorkingTime;
        private TimeSpan _workingTime;
        private int _maxBreakTime;
        private TimeSpan _breakTime;
        private CancellationTokenSource _tokenSource;

        #endregion

        #region Properties

        public int WorkingTimeMins
        {
            get { return _workingTimeMins; }
            set 
            {
                if (value == _workingTimeMins) return;
                if (value < 0)
                    value = 0;
                else if (value > _maxWorkingTime)
                    value = _maxWorkingTime;

                _workingTimeMins = value;
                OnPropertyChanged();
            }
        }

        public int BreakTimeMins
        {
            get { return _breakTimeMins; }
            set 
            {
                if (value == _breakTimeMins) return;
                if (value < 0)
                    value = 0;
                else if (value > _maxBreakTime)
                    value = _maxWorkingTime;

                _breakTimeMins = value;
                OnPropertyChanged();
            }
        }

        public bool IsRunning
        {
            get { return _isRunning; }
            set 
            { 
                _isRunning = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsStartVisible));
                OnPropertyChanged(nameof(IsCancelEnabled));
                OnPropertyChanged(nameof(IsSetupEnabled));
                OnPropertyChanged(nameof(IsCountdownVisible));
            }
        }

        public bool IsStartVisible => !IsRunning;
        //public bool IsStopVisible => IsRunning;
        public bool IsSetupEnabled => !IsRunning;
        public bool IsCountdownVisible => IsRunning;
        public bool IsCancelEnabled => IsRunning;

        public TimeSpan RemainingTime
        {
            get { return _remainingTime; }
            set 
            { 
                _remainingTime = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public DelegateCommand StartTimerCommand { get; }
        public DelegateCommand StopTimerCommand { get; }
        public DelegateCommand CancelTimerCommand { get; }

        #endregion

        public PomodoroViewModel()
        {
            StartTimerCommand = new DelegateCommand(OnStartClick);
            StopTimerCommand = new DelegateCommand(OnStopClick);
            CancelTimerCommand = new DelegateCommand(OnCancelClick);

            _maxWorkingTime = 120;
            _maxBreakTime = 30;
        }

        private void OnStartClick()
        {
            if (!(IsTimeSpanOk(_workingTimeMins, _maxWorkingTime) && IsTimeSpanOk(_breakTimeMins, _maxBreakTime)))
                return;

            StartCountdown();
        }

        private void OnStopClick()
        {
            
        }

        private void OnCancelClick()
        {
            _tokenSource?.Cancel();
        }

        private bool IsTimeSpanOk(int proposedMins, double maxMins)
        {
            return proposedMins <= maxMins || proposedMins > 0;
        }

        private async void StartCountdown()
        {
            _tokenSource?.Cancel();
            _tokenSource = new CancellationTokenSource();
            var workingTime = TimeSpan.FromMinutes(_workingTimeMins);
            var breakTime = TimeSpan.FromMinutes(_breakTimeMins);

            IsRunning = true;

            await StartTimeCountdown(workingTime, _tokenSource.Token);

            if (_tokenSource.Token.IsCancellationRequested == false)
                await StartTimeCountdown(breakTime, _tokenSource.Token);

            IsRunning = false;
        }

        private async Task StartTimeCountdown(TimeSpan duration, CancellationToken token)
        {
            DateTime startTime = DateTime.UtcNow;
            DateTime endTime = startTime + duration;

            TimeSpan remainingTime = endTime - startTime;
            TimeSpan interval = TimeSpan.FromMilliseconds(100);

            while (remainingTime > TimeSpan.Zero && token.IsCancellationRequested == false)
            {
                RemainingTime = remainingTime;
                if (RemainingTime < interval)
                    interval = RemainingTime;

                await Task.Delay(interval);
                remainingTime = endTime - DateTime.UtcNow;
            }

            RemainingTime = TimeSpan.Zero;
        }
    }
}