using Pomorillo.Application.Dtos;

namespace Pomorillo.Application.Interfaces
{
    public interface ICreateService
    {
        PomodoroDto CreatePomodoro(PomodoroCreateDto pomodoroToCreate);
    }
}
