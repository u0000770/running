using System;
using System.Collections.Generic;

namespace testAngCore.Model
{
    public partial class NextRace
    {
        public int Efkey { get; set; }
        public int RunnerId { get; set; }
        public double Distance { get; set; }
        public int Time { get; set; }
        public bool Active { get; set; }

        public virtual Runners Runner { get; set; }
    }
}
