namespace EFRRC
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RaceMemberResult")]
    public partial class RaceMemberResult
    {
        public int Id { get; set; }

        public int AthleteId { get; set; }

        public int EventId { get; set; }

        public int Result { get; set; }

        public virtual ClubMember ClubMember { get; set; }

        public virtual RunEvent RunEvent { get; set; }
    }
}
