using Pomorillo.Notifications.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Pomorillo.Notifications.SystemSound
{
    public class SystemSoundNotificationService : ITimerNotificationService
    {
        public async Task SoundBreakFinishedAlarmAsync(CancellationToken token, int? msBetweenSounds)
        {
            var cycleTime = msBetweenSounds.HasValue ? msBetweenSounds.Value : 2000;
            
            while (token.IsCancellationRequested == false)
            {
                
            }

        }

        public Task SoundWorkFinishedAlarmAsync(CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
