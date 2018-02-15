using RunningModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaceListService.Models
{
    public class EditLastRaceVM
    {
        private LastRace race;


        public int? sTime;
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

        public EditLastRaceVM(LastRace race)
        {
            this.race = race;
        }
    }
}
}