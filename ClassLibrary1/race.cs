namespace ClassLibrary1
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("race")]
    public partial class race
    {
        public int Id { get; set; }

        public double distance { get; set; }

        public int time { get; set; }

        public int runner { get; set; }

        public virtual memberList memberList { get; set; }
    }
}
