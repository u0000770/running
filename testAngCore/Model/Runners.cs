using System;
using System.Collections.Generic;

namespace testAngCore.Model
{
    public partial class Runners
    {
        public Runners()
        {
            EventRunnerTimes = new HashSet<EventRunnerTimes>();
            LastRace = new HashSet<LastRace>();
            NextRace = new HashSet<NextRace>();
        }

        public int Efkey { get; set; }
        public string Firstname { get; set; }
        public string Secondname { get; set; }
        public string Ukan { get; set; }
        public DateTime? Dob { get; set; }
        public string Email { get; set; }
        public bool? Active { get; set; }
        public string AgeGradeCode { get; set; }

        public virtual ICollection<EventRunnerTimes> EventRunnerTimes { get; set; }
        public virtual ICollection<LastRace> LastRace { get; set; }
        public virtual ICollection<NextRace> NextRace { get; set; }
    }
}
