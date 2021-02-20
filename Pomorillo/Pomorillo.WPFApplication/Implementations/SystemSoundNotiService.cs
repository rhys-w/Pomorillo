using Pomorillo.WPFApplication.Interfaces;
using System.Media;
using System.Threading;
using System.Threading.Tasks;

namespace Pomorillo.WPFApplication.Implementations
{
    public class SystemSoundNotiService : INotificationService
    {
        private int _delayBetweenSounds = 10000;

        public async Task SoundBreakFinishedAlarmAsync(bool muted, CancellationToken token)
        {
            while (token.IsCancellationRequested == false)
            {
                if (muted == false)
                    SystemSounds.Beep.Play();
                await Task.Delay(_delayBetweenSounds, token).ContinueWith((t) => { }); // TODO - code smell... fine for now.
            }
        }

        public async Task SoundWorkFinishedAlarmAsync(bool muted, CancellationToken token)
        {
            while (token.IsCancellationRequested == false)
            {
                if (muted == false)
                    SystemSounds.Beep.Play();
                await Task.Delay(_delayBetweenSounds, token).ContinueWith((t) => { });
            }
        }
    }
}
