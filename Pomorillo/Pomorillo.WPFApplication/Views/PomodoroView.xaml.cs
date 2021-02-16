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
            DataContext = new PomodoroViewModel();
        }
    }
}
