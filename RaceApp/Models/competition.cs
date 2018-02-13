namespace RaceApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    [Table("competition")]
    public partial class competition
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public competition()
        {
            RaceCompetitions = new HashSet<RaceCompetition>();
        }

        [Key]
        public int EfId { get; set; }

        [StringLength(10)]
        public string Code { get; set; }

        public string Title { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RaceCompetition> RaceCompetitions { get; set; }
    }
}
