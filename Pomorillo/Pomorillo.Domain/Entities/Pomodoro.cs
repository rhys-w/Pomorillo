using System;

namespace Pomorillo.Domain.Entities
{
    public class Pomodoro : IntEntityBase
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
