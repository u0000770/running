﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RunningModel
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class RunningModelEntities : DbContext
    {
        public RunningModelEntities()
            : base("name=RunningModelEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Discipline> Disciplines { get; set; }
        public virtual DbSet<distance> distances { get; set; }
        public virtual DbSet<memberList> memberLists { get; set; }
        public virtual DbSet<race> races { get; set; }
        public virtual DbSet<runner> runners { get; set; }
        public virtual DbSet<Comp> Comps { get; set; }
        public virtual DbSet<EventRunnerTime> EventRunnerTimes { get; set; }
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<LastRace> LastRaces { get; set; }
        public virtual DbSet<NextRace> NextRaces { get; set; }
        public virtual DbSet<RaceEvent> RaceEvents { get; set; }
    }
}
