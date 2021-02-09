using Pomorillo.App.Dtos;

namespace Pomorillo.App.Interfaces
{
    public interface ICreateService
    {
        PomodoroDto CreatePomodoro(PomodoroCreateDto pomodoroToCreate);
    }
}
