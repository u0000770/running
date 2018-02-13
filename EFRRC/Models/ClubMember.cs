using System;
using System.Collections.Generic;

namespace EFRRC.Models
{
    public partial class ClubMember
    {
        public ClubMember()
        {
            RaceMemberResult = new HashSet<RaceMemberResult>();
        }

        public int Id { get; set; }
        public int AmNumber { get; set; }
        public bool Active { get; set; }

        public ICollection<RaceMemberResult> RaceMemberResult { get; set; }
    }
}
