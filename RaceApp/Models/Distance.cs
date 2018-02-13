namespace RaceApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Distance")]
    public partial class Distance
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Distance()
        {
            Races = new HashSet<Race>();
        }

        public int Id { get; set; }

        [StringLength(10)]
        public string Code { get; set; }

        public string Name { get; set; }

        public double? Meters { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Race> Races { get; set; }
    }
}
