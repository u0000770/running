namespace ClassLibrary2
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class RCLIst : DbContext
    {
        public RCLIst()
            : base("name=RCLIst")
        {
        }

        public virtual DbSet<memberList> memberLists { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
