using System;
using System.Collections.Generic;

namespace RRCCoreApp.Models
{
    public partial class memberList
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double? Distance { get; set; }

        public int Time { get; set; }
    }


    public partial class ClubMember
    {
        public ClubMember()
        {
            RaceMemberResult = new HashSet<RaceMemberResult>();
        }

        public int Id { get; set; }
        public int AmNumber { get; set; }
        public bool Active { get; set; }
        public string Name { get; set; }

        public ICollection<RaceMemberResult> RaceMemberResult { get; set; }
    }
}
