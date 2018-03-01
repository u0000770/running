using RunningModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaceListService.Models
{
    public class EditLastRaceVM
    {
        public int? sTime;
        public int RunnerId { get; set; }
        public double Lastdistance { get; set; }
        public System.DateTime Date { get; set; }

        public TimeSpan RaceTimeSpan
        {

            get
            {
                int seconds = sTime ?? 0;
                return new TimeSpan(0, 0, seconds);
            }

            set
            {
                if (value != null)
                    sTime = (int)value.TotalSeconds;
            }
        }

        public EditLastRaceVM()
        {

        }

        public EditLastRaceVM(LastRace race)
        {
            this.Date = race.Date;
            this.sTime = race.Time;
            this.Lastdistance = race.Distance;
            this.RunnerId = race.RunnerId;
        }
    }
}
