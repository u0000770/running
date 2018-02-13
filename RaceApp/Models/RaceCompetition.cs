namespace RaceApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("RaceCompetition")]
    public partial class RaceCompetition
    {
        public int Id { get; set; }

        public int Competition { get; set; }

        public int Race { get; set; }

        public virtual competition competition1 { get; set; }

        public virtual Race Race1 { get; set; }
    }
}
