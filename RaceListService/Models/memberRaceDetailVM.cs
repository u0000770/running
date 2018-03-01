using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClassLibrary1;

namespace RaceListService.Models
{
    public class memberRaceDetailVM
    {
        public int RunnerId { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }

        public int lastRaceId { get; set; }
        [Display(Name = "Last Race Distance")]
        public string lastRaceDistance { get; set; }
        [Display(Name = "Last Race Title")]
        public string lastRaceTitle { get; set; }
        [Display(Name = "Last Race Actual Time")]
        public string lastRaceTime { get; set; }
        [Display(Name = "Last Race Date")]
        public DateTime lastRaceDate { get; set; }
        public List<EventRaceTimesVM> listOfRaces = new List<EventRaceTimesVM>();
    }

    public class EventRaceTimesVM
    {
        public int RaceId { get; set; }
        [Display(Name = "Event Race Distance")]
        public string RaceDistance { get; set; }
        [Display(Name = "Event Race Title")]
        public string RaceTitle { get; set; }
        [Display(Name = "Event Race Target")]
        public string RaceTargetTime { get; set; }
        [Display(Name = "Event Race Actual Time")]
        public string RaceActualTime { get; set; }
        [Display(Name = "Event Race Date")]
        public DateTime RaceDate { get; set; }

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