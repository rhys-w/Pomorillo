using System.Threading;
using System.Threading.Tasks;

namespace Pomorillo.Notifications.Interfaces
{
    public interface ITimerNotificationService
    {
        Task SoundWorkFinishedAlarmAsync(CancellationToken token);
        Task SoundBreakFinishedAlarmAsync(CancellationToken token);
    }
}
