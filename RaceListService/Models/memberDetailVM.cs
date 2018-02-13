using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClassLibrary1;

namespace RaceListService.Models
{
    public class memberDetailVM
    {
        public int Id { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Simple Average Predicted")]
        public string simpleAverage { get; set; }

        [Display(Name = "Top and Tail Average Predicted")]
        public string topAndtailAverage { get; set; }

        [Display(Name = "Middle 3 Average Predicted")]
        public string currentTarget { get; set; }

        public List<int> SelectedRaceIDs { get; set; }

        public List<RaceTimesVM> listOfRaces = new List<RaceTimesVM>();

        public List<RaceTimesVM> selectedRaces = new List<RaceTimesVM>();


        public static string TopandTailAve(List<RaceTimesVM> list)
        {
            double avg;
            if (list.Count > 1)
            { 
                List<RaceTimesVM> mylist = new List<RaceTimesVM>();
                mylist = list;
                double sum = 0.0;
                double count = 0.0;
                mylist.OrderBy(s => s.predictedRaw);
                if (mylist.Count > 3)
                {
                    mylist.RemoveAt(0);
                }
                if (list.Count > 3)
                {
                    list.RemoveAt(list.Count - 1);
                }
                foreach (var r in list)
                {
                    sum = r.predictedRaw + sum;
                    count++;
                }
            avg = sum / count;
            }
            else
            {
                avg = list.First().predictedRaw;
            }
            return RaceDetails.formatResult(Convert.ToInt32(avg));
        }


        public static string middleThreeAve(List<RaceTimesVM> list)
        {
            double avg;
            if (list.Count > 1)
            {
                List<RaceTimesVM> mylist = new List<RaceTimesVM>();
                mylist = list;
                mylist.OrderByDescending(s => s.predictedRaw);
                int mid = Convert.ToInt32(mylist.Count / 2);
                List<RaceTimesVM> newList = mylist.GetRange(mid, 3);
                double sum = 0.0;
                double count = 0.0;

                foreach (var r in newList)
                {
                    sum = r.predictedRaw + sum;
                    count++;
                }
                avg = sum / count;
            }
            else
            {
                avg = list.First().predictedRaw;           
            }
            return RaceDetails.formatResult(Convert.ToInt32(avg));
        }

        public static string CalcSimpleAve(List<RaceTimesVM> list)
        {
            double avg;
            if (list.Count > 1)
            {
                double sum = 0.0;
                double count = 0.0;
                foreach (var r in list)
                {
                    sum = r.predictedRaw + sum;
                    count++;
                }
                avg = sum / count;
            }
            else
            {
                avg = list.First().predictedRaw;
            }
            return RaceDetails.formatResult(Convert.ToInt32(avg));
        }

        public static List<int> buildSelectedList(IEnumerable<RaceTimesVM> listofraces)
        {
            List<int> list = new List<int>();
            foreach (var race in listofraces)
                if (race.isSelected)
                {
                    list.Add(race.lastRaceId);
                }
            return list;

        }

        public static List<RaceTimesVM> buildList(IEnumerable<race> listofraces, double targetdistance)
        {
            List<RaceTimesVM> list = new List<RaceTimesVM>();
            list = listofraces.Select(s => new RaceTimesVM
            {
                 lastRaceId = s.Id,
                 lastRaceDistance = RaceDetails.GetByRaceNameByMeters(s.distance),
                  lastRaceTime = RaceDetails.formatResult(s.time),
                //predictedTime = RaceDetails.formatResult(Convert.ToInt32(RaceDetails.calcPredictedTime(s.distance, , s.time) + RaceDetails.cameron(s.distance, 8046.72, s.time)) / 2),
                //predictedRaw = (RaceDetails.calcPredictedTime(s.distance, 8046.72, s.time) + RaceDetails.cameron(s.distance, 8046.72, s.time)) / 2
                predictedTime = RaceDetails.formatResult(Convert.ToInt32(RaceDetails.calcPredictedTime(s.distance, targetdistance, s.time) + RaceDetails.cameron(s.distance, targetdistance, s.time)) / 2),
                predictedRaw = (RaceDetails.calcPredictedTime(s.distance, targetdistance, s.time) + RaceDetails.cameron(s.distance, targetdistance, s.time)) / 2
            }

            ).ToList();

            return list;

        }
    }

    public class RaceTimesVM
    {
        public int lastRaceId { get; set; }
        public string lastRaceDistance { get; set; }
        public int lastRaceRawTime { get; set; }
        public string lastRaceTime { get; set; }
        public double camRaw { get; set; }
        public double rigRaw { get; set; }
        public double predictedRaw { get; set; }
        public string predictedTime { get; set; }
        public bool isSelected { get; set; }

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