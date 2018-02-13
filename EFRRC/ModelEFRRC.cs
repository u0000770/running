namespace EFRRC
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ModelEFRRC : DbContext
    {
        public ModelEFRRC()
            : base("name=ModelEFRRC")
        {
        }

        public virtual DbSet<ClubMember> ClubMembers { get; set; }
        public virtual DbSet<RaceMemberResult> RaceMemberResults { get; set; }
        public virtual DbSet<RunEvent> RunEvents { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClubMember>()
                .HasMany(e => e.RaceMemberResults)
                .WithRequired(e => e.ClubMember)
                .HasForeignKey(e => e.AthleteId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RunEvent>()
                .Property(e => e.CompetitionCode)
                .IsFixedLength();

            modelBuilder.Entity<RunEvent>()
                .Property(e => e.TypeCode)
                .IsFixedLength();

            modelBuilder.Entity<RunEvent>()
                .HasMany(e => e.RaceMemberResults)
                .WithRequired(e => e.RunEvent)
                .HasForeignKey(e => e.EventId)
                .WillCascadeOnDelete(false);
        }
    }
}
