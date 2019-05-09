using System;
using System.Collections.Generic;

namespace testAngCore.Model
{
    public partial class Events
    {
        public Events()
        {
            EventRunnerTimes = new HashSet<EventRunnerTimes>();
            RaceEvent = new HashSet<RaceEvent>();
        }

        public int Efkey { get; set; }
        public string Title { get; set; }
        public string Venue { get; set; }
        public string Discipline { get; set; }
        public string DistanceCode { get; set; }
        public bool? Active { get; set; }

        public virtual ICollection<EventRunnerTimes> EventRunnerTimes { get; set; }
        public virtual ICollection<RaceEvent> RaceEvent { get; set; }
    }
}
