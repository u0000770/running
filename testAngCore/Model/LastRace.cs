using System;
using System.Collections.Generic;

namespace testAngCore.Model
{
    public partial class LastRace
    {
        public int Efkey { get; set; }
        public int RunnerId { get; set; }
        public double Distance { get; set; }
        public int Time { get; set; }
        public DateTime Date { get; set; }

        public virtual Runners Runner { get; set; }
    }
}
