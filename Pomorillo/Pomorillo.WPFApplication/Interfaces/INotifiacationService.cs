using System.Threading;
using System.Threading.Tasks;

namespace Pomorillo.WPFApplication.Interfaces
{
    public interface INotificationService
    {
        Task SoundWorkFinishedAlarmAsync(CancellationToken token);
        Task SoundBreakFinishedAlarmAsync(CancellationToken token);
    }
}
