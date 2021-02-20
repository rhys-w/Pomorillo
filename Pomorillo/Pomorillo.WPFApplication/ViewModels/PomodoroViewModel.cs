using Pomorillo.WPFApplication.Interfaces;
using Prism.Commands;
using System;
using System.Media;
using System.Threading;
using System.Threading.Tasks;

namespace Pomorillo.WPFApplication.ViewModels
{
    public class PomodoroViewModel : ViewModelBase
    {
        #region private fields

        private int _workingTimeMins;
        private int _breakTimeMins;
        private bool _isRunning;
        private bool _isStopVisible;
        private TimeSpan _remainingTime;
        private int _maxWorkingTime;
        private int _maxBreakTime;
        private CancellationTokenSource _countdownTokenSource;
        private CancellationTokenSource _alarmTokenSource;
        private readonly INotificationService _notificationService;

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

        public bool IsStopVisible
        {
            get { return _isStopVisible; }
            set 
            { 
                _isStopVisible = value;
                OnPropertyChanged();
            }
        }

        public bool IsStartVisible => !IsRunning;
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

        public PomodoroViewModel(INotificationService notificationService)
        {
            StartTimerCommand = new DelegateCommand(OnStartClick);
            StopTimerCommand = new DelegateCommand(OnStopClick);
            CancelTimerCommand = new DelegateCommand(OnCancelClick);

            _maxWorkingTime = 120;
            _maxBreakTime = 30;
            _notificationService = notificationService;
        }

        private void OnStartClick()
        {
            if (!(IsTimeSpanOk(_workingTimeMins, _maxWorkingTime) && IsTimeSpanOk(_breakTimeMins, _maxBreakTime)))
                return;

            StartCountdown();
        }

        private void OnStopClick()
        {
            _alarmTokenSource?.Cancel();
        }

        private void OnCancelClick()
        {
            _countdownTokenSource?.Cancel();
            _alarmTokenSource?.Cancel();
        }

        private bool IsTimeSpanOk(int proposedMins, double maxMins)
        {
            return proposedMins <= maxMins || proposedMins > 0;
        }

        private async void StartCountdown()
        {
            _countdownTokenSource?.Cancel();
            _countdownTokenSource = new CancellationTokenSource();

            _alarmTokenSource?.Cancel();
            _alarmTokenSource = new CancellationTokenSource();

            var workingTime = TimeSpan.FromMinutes(_workingTimeMins);
            var breakTime = TimeSpan.FromMinutes(_breakTimeMins);

            IsRunning = true;

            await StartTimeCountdown(workingTime, _countdownTokenSource.Token);

            if (_countdownTokenSource.Token.IsCancellationRequested == false)
                await RunWorkTimeFinishedAlarm(_alarmTokenSource.Token);

            if (_alarmTokenSource.Token.IsCancellationRequested == true && _countdownTokenSource.Token.IsCancellationRequested == false)
                _alarmTokenSource = new CancellationTokenSource();

            if (_countdownTokenSource.Token.IsCancellationRequested == false)
                await StartTimeCountdown(breakTime, _countdownTokenSource.Token);

            if (_countdownTokenSource.Token.IsCancellationRequested == false)
                await RunBreakTimeFinishedAlarm(_alarmTokenSource.Token);

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

        private async Task RunWorkTimeFinishedAlarm(CancellationToken token)
        {
            IsStopVisible = true;

            await _notificationService.SoundWorkFinishedAlarmAsync(false, token);

            IsStopVisible = false;
        }

        private async Task RunBreakTimeFinishedAlarm(CancellationToken token)
        {
            IsStopVisible = true;

            await _notificationService.SoundBreakFinishedAlarmAsync(false, token);

            IsStopVisible = false;
        } 
    }
}