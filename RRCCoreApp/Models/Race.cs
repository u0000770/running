using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRCCoreApp.Models
{
    public class Race
    {
        public string title { get; set; }
        public double meters { get; set; }

        public List<SelectListItem> selectList { get; set; }

        public List<Race> list = new List<Race>();

        public void BuildTestList()
        {
            this.list.Add(new Race
            {
                title = "1 Miles",
                 meters = 1609.34

            });
            this.list.Add(new Race
            {
                title = "5 Miles",
                meters= 8046.72

        });
            this.list.Add(new Race
            {
                title = "10 Miles",
                meters = 16093.4

        });
            this.list.Add(new Race
            {
                title = "13.1 Miles",
                meters = 21082.41


            });
            this.list.Add(new Race
            {
                title = "20 Miles",
                meters = 32186.9

            });
            this.list.Add(new Race
            {
                title = "26.2 Miles",
                meters = 42164.81

            });
            this.list.Add(new Race
            {
                title = "5 Km",
                meters = 5000.00

            });
            this.list.Add(new Race
            {
                title = "10 Km",
                meters = 10000.00

            });
        }

        public List<SelectListItem> buildSelectList()
        {
            List<SelectListItem> slist = new List<SelectListItem>();
            this.BuildTestList();
            foreach(var item in this.list)
            {
                var option = new SelectListItem()
                {
                    Text = item.title,
                    Value = item.meters.ToString()
                };
                slist.Add(option);
            }
            return slist;
        }

        public static double GetMetersByName(string race)
        {
            switch (race)
            {
                case "1 Miles": return 1609.34;
                case "5 Miles": return 8046.72;
                case "10 Miles": return 16093.4;
                case "13 Miles": return 21082.41;
                case "20 Miles": return 32186.9;
                case "26 Miles": return 42164.81;
                case "5 KM": return 5000;
                case "10 KM": return 10000;
                default: return -1;
            }
        }

        public static string GetByRaceNameByMeters(double distance)
        {
            switch (distance)
            {
                //case "1 Miles": return 1609.34;
                case 1609.34: return "1 Mile";
                //case "5 Miles": return 8046.72;
                case 8046.72: return " 5 Mile";
                //case "10 Miles": return 16093.4;
                case 16093.4: return "10 Mile";
                //case "13 Miles": return 21082.41;
                case 21082.41: return "13.1 Mile";
                //case "20 Miles": return 32186.9;
                case 32186.9: return "20 Mile";
                //case "26 Miles": return 42164.81;
                case 42164.81: return "26.2 Mile";
                //case "5 KM": return 5000;
                case 5000: return "5 KM";
                //case "10 KM": return 10000;
                case 10000: return "10 KM";
                default: return "";
            }
        }


    }
}
