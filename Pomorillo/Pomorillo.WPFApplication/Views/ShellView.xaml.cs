using Pomorillo.WPFApplication.Implementations;
using Pomorillo.WPFApplication.Interfaces;
using Pomorillo.WPFApplication.ViewModels;
using System.Windows.Controls;

namespace Pomorillo.WPFApplication.Views
{
    /// <summary>
    /// Interaction logic for ShellView.xaml
    /// </summary>
    public partial class ShellView : UserControl
    {
        public ShellView()
        {
            InitializeComponent();
            INotificationService notificationService = new SystemSoundNotiService();
            DataContext = new ShellViewModel(notificationService);
        }
    }
}
