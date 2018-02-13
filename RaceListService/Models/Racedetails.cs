using RaceDistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace RaceListService.Models
{
    public class RaceDetails
    {
        public string title { get; set; }
        public double meters { get; set; }

        public List<SelectListItem> selectList { get; set; }

        public List<RaceDetails> list = new List<RaceDetails>();

        public void BuildTestList()
        {
            this.list.Add(new RaceDetails
            {
                title = "1 Miles",
                meters = 1609.34

            });
            this.list.Add(new RaceDetails
            {
                title = "5 Miles",
                meters = 8046.72

            });
            this.list.Add(new RaceDetails
            {
                title = "10 Miles",
                meters = 16093.4

            });
            this.list.Add(new RaceDetails
            {
                title = "13.1 Miles",
                meters = 21082.41


            });
            this.list.Add(new RaceDetails
            {
                title = "20 Miles",
                meters = 32186.9

            });
            this.list.Add(new RaceDetails
            {
                title = "26.2 Miles",
                meters = 42164.81

            });
            this.list.Add(new RaceDetails
            {
                title = "5 Km",
                meters = 5000.00

            });
            this.list.Add(new RaceDetails
            {
                title = "10 Km",
                meters = 10000.00

            });
        }

        public List<SelectListItem> buildSelectList(double targetdistance)
        {
            List<SelectListItem> slist = new List<SelectListItem>();
            this.BuildTestList();
            foreach (var item in this.list)
            {
                if (item.meters == targetdistance)
                {
                    var option = new SelectListItem()
                    {
                        Text = item.title,
                        Value = item.meters.ToString(),
                        Selected = true
                    };
                    slist.Add(option);
                }
                else
                {
                    var option = new SelectListItem()
                    {
                        Text = item.title,
                        Value = item.meters.ToString(),
                    };
                    slist.Add(option);
                }
                
            };
                            
            return slist;
      }

        public static string formatResult(int result)
        {
            TimeSpan t = TimeSpan.FromSeconds(result);

            string resultString = string.Format("{0:D2}h:{1:D2}m:{2:D2}s",
                            t.Hours,
                            t.Minutes,
                            t.Seconds);


            return resultString;
        }

// T2=T1×(D2÷D1)1.06
//T1 is the time achieved for D1.
//T2 is the time predicted for D2.
//D1 is the distance over which the initial time is achieved.
//D2 is the distance for which the time is to be predicted.

        public static double  calcPredictedTime(double old_distance, double next_distance, int old_time)
        {
            double con = 1.06;
            //t2 = t1 * (d2 / d1) ^ 1.06
            // d3 = (d2 / d1)
            double d3 = next_distance / old_distance;
            // w = d3^ 1.06
            double w = Math.Pow(d3, con);
            // t2 = t1 * w
            double new_time = old_time * w;
            return new_time;
        }

        public static double cameron(double old_dist, double next_distance, double old_time)
        {
            //a = 13.49681 - (0.000030363 * old_dist) + (835.7114 / (old_dist ^ 0.7905))
            //b = 13.49681 - (0.000030363 * new_dist) + (835.7114 / (new_dist ^ 0.7905))
            //new_time = (old_time / old_dist) * (a / b) * new_dist

            // x = (0.000030363 * old_dist)
            double x = (0.000030363 * old_dist);
            // y = (old_dist ^ 0.7905)
            double y = Math.Pow(old_dist, 0.7905);
            // z = (835.7114 / y )
            double z = 835.7114 / y;
            double a = 13.49681 - x + z;

            double w = Math.Pow(next_distance, 0.7905);
            double v = 835.7114 / w;
            double t = 0.000030363 * next_distance;


            double b = 13.49681 - t + v;

            double new_time = (old_time / old_dist) * (a / b) * next_distance;
            return new_time;

        }

        //public static double GetMetersByName(string race)
        //{
        //    var fred = race.ToLower().Trim();
        //    var str = Regex.Replace(fred, @"\s", "");

        //    switch (str)
        //    {
        //        case "1m": return 1609.34;
        //        case "5m": return 8046.72;
        //        case "10m": return 16093.4;
        //        case "13m": return 21082.41;
        //        case "20m": return 32186.9;
        //        case "26m": return 42164.81;
        //        case "5km": return 5000;
        //        case "10km": return 10000;
        //        default: return -1;
        //    }
        //}

        public static double GetMetersByCode(string code)
        {
            RaceDistance.rrcmlistEntities db = new RaceDistance.rrcmlistEntities();
            var fred = code.ToLower().Trim();
            var str = Regex.Replace(fred, @"\s", "");
            var allDistances = db.distances;
            var distance = allDistances.SingleOrDefault(d => d.Code == str).Value;
            return distance;
        }

        public static string GetByRaceNameByMeters(double distance)
        {

            var distanctStr = "";
            try
            {
                RaceDistance.rrcmlistEntities db = new RaceDistance.rrcmlistEntities();
                var allDistances = db.distances;
                distanctStr = allDistances.SingleOrDefault(d => d.Value == distance).Name;
            }
            catch
            {
                distanctStr = "not known";
            }
            return distanctStr;
        }

        //public static string GetByRaceNameByMeters(double distance)
        //{
        //    switch (distance)
        //    {
        //        //case "1 Miles": return 1609.34;
        //        case 1609.34: return "1 Mile";
        //        //case "5 Miles": return 8046.72;
        //        case 8046.72: return " 5 Mile";
        //        //case "10 Miles": return 16093.4;
        //        case 16093.4: return "10 Mile";
        //        //case "13 Miles": return 21082.41;
        //        case 21082.41: return "13.1 Mile";
        //        //case "20 Miles": return 32186.9;
        //        case 32186.9: return "20 Mile";
        //        //case "26 Miles": return 42164.81;
        //        case 42164.81: return "26.2 Mile";
        //        //case "5 KM": return 5000;
        //        case 5000: return "5 KM";
        //        //case "10 KM": return 10000;
        //        case 10000: return "10 KM";
        //        default: return "";
        //    }
        //}


    }
}