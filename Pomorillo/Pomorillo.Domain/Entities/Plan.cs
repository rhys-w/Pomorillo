using System;
using System.Collections.Generic;

namespace Pomorillo.Domain.Entities
{
    public class Plan : IntEntityBase
    {
        public string Name { get; set; }
        public List<Pomodoro> Pomodoros { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastModifiedOn { get; set; }
    }
}
