using System;
using System.Collections.Generic;

namespace EFRRC.Models
{
    public partial class RaceMemberResult
    {
        public int Id { get; set; }
        public int AthleteId { get; set; }
        public int EventId { get; set; }
        public int Result { get; set; }

        public ClubMember Athlete { get; set; }
        public RunEvent Event { get; set; }
    }
}
