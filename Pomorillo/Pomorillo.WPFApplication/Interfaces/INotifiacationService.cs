using System.Threading;
using System.Threading.Tasks;

namespace Pomorillo.WPFApplication.Interfaces
{
    public interface INotificationService
    {
        Task SoundWorkFinishedAlarmAsync(bool muted, CancellationToken token);
        Task SoundBreakFinishedAlarmAsync(bool muted, CancellationToken token);
    }
}
