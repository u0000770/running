using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RRCCoreApp.Models;
using System.Net.Http;
using System.Diagnostics;
using RRCCoreApp.ViewModel;

namespace RRCCoreApp.Controllers
{
    public class MemberListController : Controller
    {
        public IActionResult Index(double? selectedRace)
        {

            var runners = new List<memberList>().AsEnumerable(); // why do this?
            HttpClient client = new HttpClient();
            client.BaseAddress = new System.Uri("http://localhost:53010");
            client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
            HttpResponseMessage response = client.GetAsync("api/memberLists").Result;
            if (response.IsSuccessStatusCode)
            {
                runners = response.Content.ReadAsAsync<IEnumerable<memberList>>().Result; ;
            }
            else
            {
                Debug.WriteLine("Index received a bad response from the web service.");
            }
            double nextRace = 8046.72;
            if (selectedRace != null)
            {
                nextRace = selectedRace.Value;
            }
              
            NextRaceListVM nrvm = new NextRaceListVM();
            var next = runners.Select(r => new PredictNextTimesVM()
            {
               
                AthleteNumber = r.Id,
                 Name = r.Name,
                lastRaceRawTime = r.Time,
                camRaw = cameron((double)r.Distance, nextRace, r.Time),
                 rigRaw = rigle((double)r.Distance, nextRace, r.Time), 
                  lastRaceDistance = Race.GetByRaceNameByMeters((double)r.Distance)
            }
            
            ).ToList();

            foreach(var r in next)
            {
                r.predictedRaw = (r.rigRaw + r.camRaw) / 2;
                r.predictedTime = formatResult((int)r.predictedRaw);
                r.lastRaceDistance = r.lastRaceDistance + " " + "@" + " " + formatResult(r.lastRaceRawTime);

            }

            nrvm.nextRaceList = new List<PredictNextTimesVM>();
            nrvm.nextRaceList = next.OrderBy(b => b.predictedRaw).ToList();

            
            nrvm.selectList = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
            nrvm.selectList = nrvm.buildSelectList();
            return View(nrvm);

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

        public double rigle(double d1, double d2, int t1)
        {
            double con = 1.06;
            //t2 = t1 * (d2 / d1) ^ 1.06
            // d3 = (d2 / d1)
            double d3 = d2 / d1;
            // w = d3^ 1.06
            double w = Math.Pow(d3, con);
            // t2 = t1 * w
            double t2 = t1 * w;
            return t2;
        }

        public double cameron(double old_dist, double new_distance, double old_time)
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

            double w = Math.Pow(new_distance, 0.7905);
            double v = 835.7114 / w;
            double t = 0.000030363 * new_distance;


            double b = 13.49681 - t + v;

            double new_time = (old_time / old_dist) * (a / b) * new_distance;
            return new_time;

        }

    }
}