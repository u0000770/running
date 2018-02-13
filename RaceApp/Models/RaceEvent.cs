namespace RaceApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("RaceEvent")]
    public partial class RaceEvent
    {
        public int Id { get; set; }

        public string description { get; set; }

        public DateTime? date { get; set; }

        public int race { get; set; }

        public virtual Race Race1 { get; set; }
    }
}
