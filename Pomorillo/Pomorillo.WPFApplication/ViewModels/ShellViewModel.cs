using Pomorillo.WPFApplication.Enums;
using Pomorillo.WPFApplication.Interfaces;
using Prism.Commands;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Pomorillo.WPFApplication.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        private int _pomodoroTime;
        private int _shortBreakTime;
        private int _longBreakTime;
        private int _maxCustomTime;
        private TimeSpan _currentTimeSpan;
        private CancellationTokenSource _countdownTokenSource;
        private CancellationTokenSource _notificationTokenSource;
        private CountdownType _currentCountdownType;

        private bool _isPomodoroSelected;
        public bool IsPomodoroSelected
        {
            get { return _isPomodoroSelected; }
            set 
            {
                if (value == _isPomodoroSelected) return;
                _isPomodoroSelected = value;
                OnPropertyChanged();
            }
        }

        private bool _isShortBreakSelected;
        public bool IsShortBreakSelected
        {
            get { return _isShortBreakSelected; }
            set 
            {
                if (value == _isShortBreakSelected) return;
                _isShortBreakSelected = value;
                OnPropertyChanged();
            }
        }

        private bool _isLongBreakSelected;
        public bool IsLongBreakSelected
        {
            get { return _isLongBreakSelected; }
            set 
            {
                if (value == _isLongBreakSelected) return;
                _isLongBreakSelected = value;
                OnPropertyChanged();
            }
        }

        private bool _isMuted;
        public bool IsMuted
        {
            get { return _isMuted; }
            set 
            {
                if (value == _isMuted) return;
                _isMuted = value;
                OnPropertyChanged();
            }
        }

        private bool _isCustomInUse;

        public bool IsCustomInUse
        {
            get { return _isCustomInUse; }
            set 
            {
                if (value == _isCustomInUse) return;
                _isCustomInUse = value;
                OnCustomInUseClick();
                OnPropertyChanged();
            }
        }

        private int _customTimeMins;
        public int CustomTimeMins
        {
            get { return _customTimeMins; }
            set
            {
                if (value == _customTimeMins) return;
                if (value < 0)
                    value = 0;
                else if (value > _maxCustomTime)
                    value = _maxCustomTime;

                _customTimeMins = value;
                OnPropertyChanged();
                RemainingTime = TimeSpan.FromMinutes(_customTimeMins);
            }
        }

        private TimeSpan _remainingTime;
        private readonly INotificationService _notificationService;

        public TimeSpan RemainingTime
        {
            get { return _remainingTime; }
            set
            {
                _remainingTime = value;
                OnPropertyChanged();
            }
        }

        public DelegateCommand RegPomCommand { get; }
        public DelegateCommand ShortBreakCommand { get; }
        public DelegateCommand LongBreakCommand { get; }
        public DelegateCommand StartCommand { get; }
        public DelegateCommand StopCommand { get; }
        public DelegateCommand PauseCommand { get; }
        public DelegateCommand ResetCommand { get; }

        public ShellViewModel(INotificationService notificationService)
        {
            _notificationService = notificationService;

            _maxCustomTime = 120;
            _pomodoroTime = 25;
            _shortBreakTime = 5;
            _longBreakTime = 10;

            RegPomCommand = new DelegateCommand(OnRegPomClick);
            ShortBreakCommand = new DelegateCommand(OnShortBreakClick);
            LongBreakCommand = new DelegateCommand(OnLongBreakClick);
            StartCommand = new DelegateCommand(async () => await OnStartClick());
            StopCommand = new DelegateCommand(OnStopClick);
            PauseCommand = new DelegateCommand(OnPauseClick);
            ResetCommand = new DelegateCommand(OnResetClick);

            RegPomCommand.Execute();
        }

        private void OnRegPomClick()
        {
            IsPomodoroSelected = true;
            IsShortBreakSelected = false;
            IsLongBreakSelected = false;
            IsCustomInUse = false;

            _currentTimeSpan = TimeSpan.FromMinutes(_pomodoroTime);
            RemainingTime = _currentTimeSpan;
            _currentCountdownType = CountdownType.Work;
        }


        private void OnShortBreakClick()
        {
            IsPomodoroSelected = false;
            IsShortBreakSelected = true;
            IsLongBreakSelected = false;
            IsCustomInUse = false;

            _currentTimeSpan = TimeSpan.FromMinutes(_shortBreakTime);
            RemainingTime = _currentTimeSpan;
            _currentCountdownType = CountdownType.Break;
        }

        private void OnLongBreakClick()
        {
            IsPomodoroSelected = false;
            IsShortBreakSelected = false;
            IsLongBreakSelected = true;
            IsCustomInUse = false;

            _currentTimeSpan = TimeSpan.FromMinutes(_longBreakTime);
            RemainingTime = _currentTimeSpan;
            _currentCountdownType = CountdownType.Break;
        }

        private void OnCustomInUseClick()
        {
            if (_isCustomInUse)
            {
                IsPomodoroSelected = false;
                IsShortBreakSelected = false;
                IsLongBreakSelected = false;
                RemainingTime = TimeSpan.FromMinutes(CustomTimeMins);
                _currentCountdownType = CountdownType.Work;
            }
            else
                OnRegPomClick();
        }

        private async Task OnStartClick()
        {
            _notificationTokenSource?.Cancel();
            _notificationTokenSource = new CancellationTokenSource();
            _countdownTokenSource?.Cancel();
            _countdownTokenSource = new CancellationTokenSource();

            var countdownToken = _countdownTokenSource.Token;
            var notificationToken = _notificationTokenSource.Token;

            await StartTimeCountdownAsync(countdownToken);

            if (countdownToken.IsCancellationRequested == false)
            {
                await InformUserTimeIsUp(notificationToken);
            }
        }

        private async Task InformUserTimeIsUp(CancellationToken notificationToken)
        {
            RemainingTime = TimeSpan.Zero;
            if (_currentCountdownType == CountdownType.Work)
                await _notificationService.SoundWorkFinishedAlarmAsync(IsMuted, notificationToken);
            else
                await _notificationService.SoundBreakFinishedAlarmAsync(IsMuted, notificationToken);
        }

        private async Task StartTimeCountdownAsync(CancellationToken token)
        {
            var interval = TimeSpan.FromMilliseconds(100);

            var startTime = DateTime.UtcNow;
            var endTime = startTime + RemainingTime;
            var remainingTime = endTime - startTime;

            while (remainingTime > TimeSpan.Zero && token.IsCancellationRequested == false)
            {
                RemainingTime = remainingTime;
                if (RemainingTime < interval)
                    interval = RemainingTime;

                await Task.Delay(interval);
                remainingTime = endTime - DateTime.UtcNow;
            }
        }

        private void OnStopClick()
        {
            _notificationTokenSource?.Cancel();
        }

        private void OnPauseClick()
        {
            _countdownTokenSource?.Cancel();
        }

        private void OnResetClick()
        {
            _countdownTokenSource?.Cancel();
            _notificationTokenSource?.Cancel();
            RemainingTime = _currentTimeSpan;
        }
    }
}
