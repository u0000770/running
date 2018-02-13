using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EFRRC.Models
{
    public partial class Event_ResultContext : DbContext
    {
        public virtual DbSet<ClubMember> ClubMember { get; set; }
        public virtual DbSet<RaceMemberResult> RaceMemberResult { get; set; }
        public virtual DbSet<RunEvent> RunEvent { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;initial catalog=Event_Result;integrated security=True;MultipleActiveResultSets=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClubMember>(entity =>
            {
                entity.Property(e => e.AmNumber).HasColumnName("AM_Number");
            });

            modelBuilder.Entity<RaceMemberResult>(entity =>
            {
                entity.HasOne(d => d.Athlete)
                    .WithMany(p => p.RaceMemberResult)
                    .HasForeignKey(d => d.AthleteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RaceMemberResult_ClubMember");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.RaceMemberResult)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RaceMemberResult_RunEvent");
            });

            modelBuilder.Entity<RunEvent>(entity =>
            {
                entity.Property(e => e.CompetitionCode)
                    .IsRequired()
                    .HasColumnType("nchar(10)");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.DistanceMetric)
                    .IsRequired()
                    .HasColumnType("char(10)");

                entity.Property(e => e.Location).IsRequired();

                entity.Property(e => e.Title).IsRequired();

                entity.Property(e => e.TypeCode)
                    .IsRequired()
                    .HasColumnType("nchar(10)");
            });
        }
    }
}
