namespace ClassLibrary1
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=ModelHC")
        {
        }

        public virtual DbSet<memberList> memberLists { get; set; }
        public virtual DbSet<race> races { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<memberList>()
                .HasMany(e => e.races)
                .WithRequired(e => e.memberList)
                .HasForeignKey(e => e.runner)
                .WillCascadeOnDelete(false);
        }
    }
}
