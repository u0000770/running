//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RunningModelA
{
    using System;
    using System.Collections.Generic;
    
    public partial class memberList
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public memberList()
        {
            this.races = new HashSet<race>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<double> Distance { get; set; }
        public int Time { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<race> races { get; set; }
    }
}
