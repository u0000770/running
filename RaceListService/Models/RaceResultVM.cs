using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RaceListService.Models
{
    public class RaceResultItemVM
    {
        public string RunnerName { get; set; }
        [Display(Name = "H:M:S")]
        public string predictedTime { get; set; }
        [Display(Name = "H:M:S")]
        public string actualTime { get; set; }
        public int differenceTime { get; set; }
        [Display(Name = "H:M:S")]
        public string trophyTime { get; set; }
        public int trophyPoints { get; set; }

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
