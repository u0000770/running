namespace raceListModel
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ModelRaceList : DbContext
    {
        public ModelRaceList()
            : base("name=ModelRaceList")
        {
        }

        public virtual DbSet<memberList> memberLists { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
