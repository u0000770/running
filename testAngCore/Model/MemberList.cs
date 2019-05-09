using System;
using System.Collections.Generic;

namespace testAngCore.Model
{
    public partial class MemberList
    {
        public MemberList()
        {
            Race = new HashSet<Race>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public double? Distance { get; set; }
        public int Time { get; set; }

        public virtual ICollection<Race> Race { get; set; }
    }
}
