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
        [Display(Name = "Based upon Last Distance")]
        public string LastDistance { get; set; }
        [Display(Name = "Previous Time")]
        public string LastTime { get; set; }
    }
}