using Prism.Commands;

namespace Pomorillo.Presentation.WPF.ViewModels
{
    public class CreatePomodoroViewModel : ViewModelBase
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set 
            { 
                _name = value; 
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsOkEnabled));
                OnPropertyChanged(nameof(IsNameValidationVisible));
            }
        }

        private int _workTime;
        public int WorkTime
        {
            get { return _workTime; }
            set 
            { 
                _workTime = value; 
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsOkEnabled));
                OnPropertyChanged(nameof(IsWorkTimeValidationVisible));
            }
        }

        private int _breakTime;
        public int BreakTime
        {
            get { return _breakTime; }
            set 
            { 
                _breakTime = value; 
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsOkEnabled));
                OnPropertyChanged(nameof(IsBreakTimeValidationVisible));
            }
        }

        public bool IsOkEnabled => CanSubmit();
        public bool IsNameValidationVisible => string.IsNullOrEmpty(Name);
        public bool IsWorkTimeValidationVisible => IsWorkTimeInRange() == false;
        public bool IsBreakTimeValidationVisible => IsBreakTimeInRange() == false;

        public DelegateCommand OkCommand { get; private set; }

        public CreatePomodoroViewModel()
        {
            OkCommand = new DelegateCommand(OnOkCommand);
        }

        private void OnOkCommand()
        {
            
        }

        private bool CanSubmit()
        {
            return IsWorkTimeInRange() && IsBreakTimeInRange();
        }

        private bool IsWorkTimeInRange() => WorkTime > 0 && WorkTime < 60;
        private bool IsBreakTimeInRange() => BreakTime > 0 && BreakTime < 60;
    }
}
