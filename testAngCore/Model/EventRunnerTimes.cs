using System;
using System.Collections.Generic;

namespace testAngCore.Model
{
    public partial class EventRunnerTimes
    {
        public int Efkey { get; set; }
        public int RunnerId { get; set; }
        public int EventId { get; set; }
        public int? Target { get; set; }
        public int? Actual { get; set; }
        public DateTime? Date { get; set; }
        public bool? Active { get; set; }
        public int? RaceEventId { get; set; }

        public virtual Events Event { get; set; }
        public virtual Runners Runner { get; set; }
    }
}
