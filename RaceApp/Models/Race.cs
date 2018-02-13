namespace RaceApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Race")]
    public partial class Race
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Race()
        {
            RaceCompetitions = new HashSet<RaceCompetition>();
            RaceEvents = new HashSet<RaceEvent>();
        }

        public int Id { get; set; }

        public int distance { get; set; }

        public int form { get; set; }

        public string title { get; set; }

        public string location { get; set; }

        public string series { get; set; }

        public virtual Distance Distance1 { get; set; }

        public virtual form form1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RaceCompetition> RaceCompetitions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RaceEvent> RaceEvents { get; set; }
    }
}
