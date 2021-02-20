using Pomorillo.WPFApplication.Interfaces;
using System.Media;
using System.Threading;
using System.Threading.Tasks;

namespace Pomorillo.WPFApplication.Implementations
{
    public class SystemSoundNotiService : INotificationService
    {
        private int _delayBetweenSounds = 5000;

        public async Task SoundBreakFinishedAlarmAsync(CancellationToken token)
        {
            while (token.IsCancellationRequested == false)
            {
                SystemSounds.Beep.Play();
                await Task.Delay(_delayBetweenSounds);
            }
        }

        public async Task SoundWorkFinishedAlarmAsync(CancellationToken token)
        {
            while (token.IsCancellationRequested == false)
            {
                SystemSounds.Beep.Play();
                await Task.Delay(_delayBetweenSounds);
            }
        }
    }
}
