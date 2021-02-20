using Pomorillo.WPFApplication.Implementations;
using Pomorillo.WPFApplication.Interfaces;
using Pomorillo.WPFApplication.ViewModels;
using System.Windows.Controls;

namespace Pomorillo.WPFApplication.Views
{
    /// <summary>
    /// Interaction logic for PomodoroRunView.xaml
    /// </summary>
    public partial class PomodoroView : UserControl
    {
        public PomodoroView()
        {
            InitializeComponent();

            // TODO - add unity / any DI container if this project grows.
            INotificationService notificationService = new SystemSoundNotiService();
            DataContext = new PomodoroViewModel(notificationService);
        }
    }
}
