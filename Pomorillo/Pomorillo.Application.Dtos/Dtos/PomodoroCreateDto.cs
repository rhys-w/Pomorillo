using System;

namespace Pomorillo.Application.Dtos
{
    public class PomodoroCreateDto
    {
        public int? Index { get; set; }
        public string Name { get; set; }
        public int WorkTime { get; set; }
        public int BreakTime { get; set; }
    }

    public class PomodoroDto
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public int WorkTime { get; set; }
        public int BreakTime { get; set; }
        public int TotalTime => WorkTime + BreakTime;
        public DateTime CreatedOn { get; set; }
        public DateTime LastModifiedOn { get; set; } 
    }
}
