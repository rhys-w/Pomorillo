using Pomorillo.Presentation.WPF.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Pomorillo.Presentation.WPF.Views
{
    /// <summary>
    /// Interaction logic for CreatePomodoroView.xaml
    /// </summary>
    public partial class CreatePomodoroView : Window
    {
        private CreatePomodoroViewModel _viewModel;

        public CreatePomodoroView()
        {
            _viewModel = new CreatePomodoroViewModel();
            DataContext = _viewModel;

            InitializeComponent();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
