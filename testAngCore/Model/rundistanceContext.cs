using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace testAngCore.Model
{
    public partial class rundistanceContext : DbContext
    {
        public rundistanceContext()
        {
        }

        public rundistanceContext(DbContextOptions<rundistanceContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Comps> Comps { get; set; }
        public virtual DbSet<Discipline> Discipline { get; set; }
        public virtual DbSet<Distance> Distance { get; set; }
        public virtual DbSet<EventRunnerTimes> EventRunnerTimes { get; set; }
        public virtual DbSet<Events> Events { get; set; }
        public virtual DbSet<LastRace> LastRace { get; set; }
        public virtual DbSet<MemberList> MemberList { get; set; }
        public virtual DbSet<NextRace> NextRace { get; set; }
        public virtual DbSet<Race> Race { get; set; }
        public virtual DbSet<RaceEvent> RaceEvent { get; set; }
        public virtual DbSet<Runners> Runners { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=rundistance.database.windows.net,1433;Database=rundistance;Trusted_Connection=False;Encrypt=True;;user id=run_barry_run;password=Tel@503141;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.3-servicing-35854");

            modelBuilder.Entity<Comps>(entity =>
            {
                entity.HasKey(e => e.Efkey);

                entity.Property(e => e.Efkey).HasColumnName("EFKey");
            });

            modelBuilder.Entity<Discipline>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code).IsRequired();

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<Distance>(entity =>
            {
                entity.HasKey(e => e.Efkey);

                entity.ToTable("distance");

                entity.Property(e => e.Efkey).HasColumnName("EFKey");

                entity.Property(e => e.Code).IsRequired();

                entity.Property(e => e.Distance1).HasColumnName("Distance");

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<EventRunnerTimes>(entity =>
            {
                entity.HasKey(e => e.Efkey);

                entity.Property(e => e.Efkey).HasColumnName("EFKey");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.EventRunnerTimes)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EventRunnerTimes_Events");

                entity.HasOne(d => d.Runner)
                    .WithMany(p => p.EventRunnerTimes)
                    .HasForeignKey(d => d.RunnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EventRunnerTimes_runners");
            });

            modelBuilder.Entity<Events>(entity =>
            {
                entity.HasKey(e => e.Efkey);

                entity.Property(e => e.Efkey).HasColumnName("EFKey");
            });

            modelBuilder.Entity<LastRace>(entity =>
            {
                entity.HasKey(e => e.Efkey);

                entity.Property(e => e.Efkey).HasColumnName("EFKey");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.HasOne(d => d.Runner)
                    .WithMany(p => p.LastRace)
                    .HasForeignKey(d => d.RunnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LastRace_runners");
            });

            modelBuilder.Entity<MemberList>(entity =>
            {
                entity.ToTable("memberList");
            });

            modelBuilder.Entity<NextRace>(entity =>
            {
                entity.HasKey(e => e.Efkey);

                entity.Property(e => e.Efkey).HasColumnName("EFKey");

                entity.HasOne(d => d.Runner)
                    .WithMany(p => p.NextRace)
                    .HasForeignKey(d => d.RunnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NextRace_runners");
            });

            modelBuilder.Entity<Race>(entity =>
            {
                entity.ToTable("race");

                entity.Property(e => e.Distance).HasColumnName("distance");

                entity.Property(e => e.Runner).HasColumnName("runner");

                entity.Property(e => e.Time).HasColumnName("time");

                entity.HasOne(d => d.RunnerNavigation)
                    .WithMany(p => p.Race)
                    .HasForeignKey(d => d.Runner)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_race_memberList");
            });

            modelBuilder.Entity<RaceEvent>(entity =>
            {
                entity.HasKey(e => e.Efkey);

                entity.Property(e => e.Efkey).HasColumnName("EFKey");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.RaceEvent)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RaceEvent_Events1");
            });

            modelBuilder.Entity<Runners>(entity =>
            {
                entity.HasKey(e => e.Efkey);

                entity.ToTable("runners");

                entity.Property(e => e.Efkey).HasColumnName("EFKey");

                entity.Property(e => e.AgeGradeCode)
                    .HasColumnName("ageGradeCode")
                    .HasMaxLength(10);

                entity.Property(e => e.Dob)
                    .HasColumnName("dob")
                    .HasColumnType("date");

                entity.Property(e => e.Email).HasColumnName("email");

                entity.Property(e => e.Firstname)
                    .IsRequired()
                    .HasColumnName("firstname");

                entity.Property(e => e.Secondname)
                    .IsRequired()
                    .HasColumnName("secondname");

                entity.Property(e => e.Ukan).HasColumnName("ukan");
            });
        }
    }
}
