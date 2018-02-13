namespace EFRRC
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RunEvent")]
    public partial class RunEvent
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RunEvent()
        {
            RaceMemberResults = new HashSet<RaceMemberResult>();
        }

        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Location { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        public int DistanceLength { get; set; }

        public bool DistanceMetric { get; set; }

        [StringLength(10)]
        public string CompetitionCode { get; set; }

        [StringLength(10)]
        public string TypeCode { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RaceMemberResult> RaceMemberResults { get; set; }
    }
}
