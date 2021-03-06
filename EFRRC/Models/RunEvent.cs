﻿using System;
using System.Collections.Generic;

namespace EFRRC.Models
{
    public partial class RunEvent
    {
        public RunEvent()
        {
            RaceMemberResult = new HashSet<RaceMemberResult>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public DateTime Date { get; set; }
        public int DistanceLength { get; set; }
        public string DistanceMetric { get; set; }
        public string CompetitionCode { get; set; }
        public string TypeCode { get; set; }

        public ICollection<RaceMemberResult> RaceMemberResult { get; set; }
    }
}
