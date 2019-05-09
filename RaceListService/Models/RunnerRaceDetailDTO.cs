using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaceListService.Models
{
    public class RunnerRaceDetailDTO
    {
        public string ukaNumber { get; set; }

        public string Name { get; set; }

        public List<EventRaceTimesDTO> listOfRaces = new List<EventRaceTimesDTO>();

    }


    public class EventRaceTimesDTO
    {
        public int RaceId { get; set; }
       
        public string RaceDistance { get; set; }
       
        public string RaceTitle { get; set; }
       
        public string RaceTargetTime { get; set; }
      
        public string RaceActualTime { get; set; }

        public string RaceDate { get; set; }
     
        public int TargetTime { get; set; }

        //public static string formatResult(int result)
        //{
        //    string resultString = "No Result";
        //    if (result > 0)
        //    {

        //        TimeSpan t = TimeSpan.FromSeconds(result);

        //        resultString = string.Format("{0:D2}h:{1:D2}m:{2:D2}s",
        //                        t.Hours,
        //                        t.Minutes,
        //                        t.Seconds);

        //    }

        //    return resultString;
        //}

    }



}