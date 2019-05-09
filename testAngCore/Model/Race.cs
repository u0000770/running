using System;
using System.Collections.Generic;

namespace testAngCore.Model
{
    public partial class Race
    {
        public int Id { get; set; }
        public double Distance { get; set; }
        public int Time { get; set; }
        public int Runner { get; set; }

        public virtual MemberList RunnerNavigation { get; set; }
    }
}
