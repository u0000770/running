using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RaceListService.Models
{
    public class NextRaceListVM
    {
        public List<PredictNextTimesVM> nextRaceList { get; set; }


        public class PredictTimeListItemVM
        {
            [Display(Name = "Race Type")]
            public string raceType { get; set; }
            [Display(Name = "H:M:S")]
            public string predictedTime { get; set; }
            public string camTime { get; set; }
            public int rawResult { get; set; }

        }

        public class PredictNextTimesVM
        {

            public string Name { get; set; }
            public string lastRaceDistance { get; set; }
            public int lastRaceRawTime { get; set; }
            public string lastRaceTime { get; set; }
            public double camRaw { get; set; }
            public double rigRaw { get; set; }
            public double predictedRaw { get; set; }
            public string predictedTime { get; set; }

            public static string formatResult(int result)
            {
                TimeSpan t = TimeSpan.FromSeconds(result);

                string resultString = string.Format("{0:D2}h:{1:D2}m:{2:D2}s",
                                t.Hours,
                                t.Minutes,
                                t.Seconds);


                return resultString;
            }

        }

        public class PredictTimesVM
        {

            public string Name { get; set; }
            public string lastRaceType { get; set; }
            public int lastRaceRawTime { get; set; }
            public string lastRaceTime { get; set; }

            public List<PredictTimeListItemVM> predictionList { get; set; }

            public static string formatResult(int result)
            {
                TimeSpan t = TimeSpan.FromSeconds(result);

                string resultString = string.Format("{0:D2}h:{1:D2}m:{2:D2}s",
                                t.Hours,
                                t.Minutes,
                                t.Seconds);


                return resultString;
            }

        }
    }
}