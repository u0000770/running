namespace RaceApp.Models
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;

    public partial class Modelx : DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                 optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Blogging;Trusted_Connection=True;");
            }
        }


        public virtual DbSet<competition> competitions { get; set; }
        public virtual DbSet<Distance> Distances { get; set; }
        public virtual DbSet<form> forms { get; set; }
        public virtual DbSet<Race> Races { get; set; }
        public virtual DbSet<RaceCompetition> RaceCompetitions { get; set; }
        public virtual DbSet<RaceEvent> RaceEvents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<competition>().Property(e => e.Code);

            modelBuilder.Entity<competition>().HasMany(e => e.RaceCompetitions)  .HasForeignKey(e => e.Competition)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Distance>()
                .Property(e => e.Code);

            modelBuilder.Entity<Distance>()
                .HasMany(e => e.Races)
                .HasForeignKey(e => e.distance)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<form>()
                .Property(e => e.Code)
                .IsFixedLength();

            modelBuilder.Entity<form>()
                .HasMany(e => e.Races)
                .WithRequired(e => e.form1)
                .HasForeignKey(e => e.form)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Race>()
                .HasMany(e => e.RaceCompetitions)
                .WithRequired(e => e.Race1)
                .HasForeignKey(e => e.Race)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Race>()
                .HasMany(e => e.RaceEvents)
                .HasForeignKey(e => e.race)
                .WillCascadeOnDelete(false);
        }
    }
}
