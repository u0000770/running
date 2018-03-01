using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RaceListService.Models
{
    public class EditEventRunnerTimeVM
    {
        public int RaceId { get; set; }
        public string RunnerName { get; set; }
        [Display(Name = "Event Race Distance")]
        public string RaceDistance { get; set; }
        [Display(Name = "Event Race Date")]
        [DataType(DataType.Date)]
        public DateTime RaceDate { get; set; }
        [Display(Name = "Event Race Title")]
        public string RaceTitle { get; set; }
        [Display(Name = "Event Race Target")]
        public string RaceTargetTime { get; set; }
        [Display(Name = "Event Race Actual Time")]
        public int? RaceActualTime;

        public TimeSpan RaceTimeSpan
        {

            get
            {
                int seconds = RaceActualTime ?? 0;
                return new TimeSpan(0, 0, seconds);
            }

            set
            {
                if (value != null)
                    RaceActualTime = (int)value.TotalSeconds;
            }
        }


        
     

        public static string formatResult(int result)
        {
            string resultString = "No Result";
            if (result > 0)
            {

                TimeSpan t = TimeSpan.FromSeconds(result);

                resultString = string.Format("{0:D2}h:{1:D2}m:{2:D2}s",
                                t.Hours,
                                t.Minutes,
                                t.Seconds);

            }

            return resultString;
        }

 
            public EditEventRunnerTimeVM()
            {

            }

            
        }

}