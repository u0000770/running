using System;
using System.Collections.Generic;

namespace testAngCore.Model
{
    public partial class RaceEvent
    {
        public int Efkey { get; set; }
        public int EventId { get; set; }
        public DateTime Date { get; set; }
        public bool Active { get; set; }

        public virtual Events Event { get; set; }
    }
}
