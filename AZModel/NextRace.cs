//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AZModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class NextRace
    {
        public int EFKey { get; set; }
        public int RunnerId { get; set; }
        public double Distance { get; set; }
        public int Time { get; set; }
        public bool Active { get; set; }
    
        public virtual runner runner { get; set; }
    }
}