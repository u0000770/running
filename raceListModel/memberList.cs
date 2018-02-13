namespace raceListModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("memberList")]
    public partial class memberList
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double? Distance { get; set; }

        public int Time { get; set; }
    }
}
