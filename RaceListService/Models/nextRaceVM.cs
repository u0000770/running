using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RaceListService.Models
{
    public class nextRaceVM
    {
        public int RunnerId { get; set; }
        [Display(Name = "Name")]
        public string RunnerName { get; set; }
        public double Distance { get; set; }
        [Display(Name = "Predicted Time")]
        public string Time { get; set; }
        [Display(Name = "Distance of Last Race")]
        public string LastDistance { get; set; }
        [Display(Name = "Actual Time of Last Race")]
        public string LastTime { get; set; }
        public DateTime date {get;set; }
    }


    public class currentNextRaceVM
    {
        public int RaceId { get; set; }
        [Display(Name = "Race Name")]
        public string RaceName { get; set; }
        [Display(Name = "Distance")]
        public double Distance { get; set; }
        [Display(Name = "Date")]
        public DateTime date { get; set; }

        public List<newNextRaceVM> Preditions { get; set; }
    }


    public class newNextRaceVM
    {
        public int RunnerId { get; set; }

        [Display(Name = "Name")]
        public string RunnerName { get; set; }

        [Display(Name = "UKAN")]
        public string UKAN { get; set; }

        [Display(Name = "Predicted Time")]
        public string PredictedTime { get; set; }


    }

    public class RaceListItemVM
    {
        public int RunnerId { get; set; }
        [Display(Name = "Name")]
        public string RunnerName { get; set; }
        public double CurrentDistance { get; set; }
        [Display(Name = "Predicted Time")]
        public string PredictedTime { get; set; }
        [Display(Name = "Based upon Last Distance")]
        public string LastDistance { get; set; }
        [Display(Name = "Previous Time")]
        public string LastTime { get; set; }
        [Display(Name = "Actual Time")]
        public string ActualTime { get; set; }
    }


}