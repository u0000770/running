using RaceListService.Models;
using RunningModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RaceListService.Controllers
{
    public class myTimeController : Controller
    {

        private RunningModelEntities db = new RunningModelEntities();
        // GET: myTime
        public ActionResult Index(RaceListService.Models.memberRaceDetailVM vmIn,int? id,string elistValue)
        {
            /// if we dont have a valid runner id return to get one.
            if (id == null)
            {
                return RedirectToAction("Index", "Default");
            }
            
            /// so lets get this runner
            runner thisRunner = db.runners.Find(id);
            if (thisRunner == null)
            {
                return HttpNotFound();
            }

            // set the next race distance either from the input form or the first in the list
            var elist = db.RaceEvents.Where(d => d.Date > DateTime.Now).OrderBy(r => r.Date).Take(4);
            var dlist = db.distances;
            double distance = 0;
            if (elistValue == null)
            {
                distance = (int)dlist.FirstOrDefault(d => d.Code == elist.FirstOrDefault().Event.DistanceCode).Value;
            }
            else
            {
                var ekey = Convert.ToInt32(elistValue);
                distance = (int)dlist.FirstOrDefault(d => d.Code == elist.FirstOrDefault(r =>r.EFKey == ekey).Event.DistanceCode).Value;
            }


            var vm = new RaceListService.Models.memberRaceDetailVM();
            BuildmemberRaceDetailVM(thisRunner, vm);

            List<RaceListService.Models.EventRaceTimesVM> listOfRacesVM = BuildEventRaceList(thisRunner,(int)distance);
            var total = 0;
            var numberOfRaces = 0;
            foreach (RaceListService.Models.EventRaceTimesVM ev in listOfRacesVM)
            {
                total = total + ev.TargetTime;
                numberOfRaces++;
            }
            if (numberOfRaces > 0)
            {
                vm.targetTime = RaceListService.Models.EventRaceTimesVM.formatResult(total / numberOfRaces);
                vm.listOfRaces = listOfRacesVM.OrderBy(r => r.RaceDate).ToList();
                vm.totalTime = total;
            }
            else
            {
                vm.listOfRaces = null;
                vm.targetTime = "currently not available";
                vm.totalTime = 0;
            }

            var list = buildSelectList(elist);
            ViewBag.elistValue = new SelectList(list, "Value", "Text");
            if (elistValue == null)
            {
                elistValue = elist.FirstOrDefault().EFKey.ToString(); 
            }
            var thisRace = db.RaceEvents.Find(Convert.ToInt32(elistValue));
            // DateTime.Parse(thisRace.Date.ToShortDateString(), new CultureInfo("en-GB", false));
            vm.lastRaceDate = thisRace.Date;
            vm.lastRaceDistance = db.distances.SingleOrDefault(d => d.Code == thisRace.Event.DistanceCode).Name;
            vm.lastRaceTitle = thisRace.Event.Title;
            var now = DateTime.Today;
            vm.totalTime = (vm.lastRaceDate.DayOfYear - now.Date.DayOfYear) / 7;
            // ViewBag.elistValue = list;
            var distnceInMiles = distance * 0.00062137119;
            double timeInMin = (total / numberOfRaces);
            var pace = timeInMin / distnceInMiles;
            
            var timeSpan = TimeSpan.FromSeconds(pace);
            int hh = timeSpan.Hours;
            int mm = timeSpan.Minutes;
            int ss = timeSpan.Seconds;

            string min = mm.ToString();
            string sec = ss.ToString();
            string sPace = min + " min " + sec + " sec";
            vm.pace = sPace;
            return View(vm);
 
        }

        private List<SelectListItem> buildSelectList(IEnumerable<RaceEvent> elist)
        {
           
            List<SelectListItem> slist = new List<SelectListItem>();

            foreach (var item in elist)
            {
                var option = new SelectListItem()
                {
                    Text = item.Event.Title,
                    Value = item.EFKey.ToString()
                };
                slist.Add(option);
            }
            return slist;

        }


        private List<RaceListService.Models.EventRaceTimesVM> BuildEventRaceList(runner thisRunner, int newDistance)
        {
            List<RaceListService.Models.EventRaceTimesVM> listOfRacesVM = new List<RaceListService.Models.EventRaceTimesVM>();
            var listOfRace = db.EventRunnerTimes.Where(r => r.RunnerId == thisRunner.EFKey && r.Actual != null  && r.Actual > 0);
            var lastThreeRaces = listOfRace.OrderByDescending(r => r.Date).Take(3);
            double totalTime = 0;
            int numberOfRaces = 0;
            foreach (var race in lastThreeRaces)
            {

                

                RaceListService.Models.EventRaceTimesVM ert = new RaceListService.Models.EventRaceTimesVM();
                ert.RaceId = race.EFKey;
                ert.RaceDistance = db.distances.SingleOrDefault(d => d.Code == race.Event.DistanceCode).Name;
                var distance = db.distances.SingleOrDefault(d => d.Code == race.Event.DistanceCode).Value;
                ert.RaceTitle = race.Event.Title;
                
                int x = race.Actual ?? 0;
                int y = (int)CalculatePredicion(newDistance, distance, x);
                numberOfRaces++;

                ert.RaceActualTime = RaceListService.Models.EventRaceTimesVM.formatResult(x);
                ert.TargetTime = y;
                ert.RaceTargetTime = RaceListService.Models.EventRaceTimesVM.formatResult(y);
                int td = 0;
                if (x != 0)
                {
                    td = y - x;
                }
                if (td > 0)
                { ert.TimeDifference = (y - x).ToString(); }
                else
                { ert.TimeDifference = 0.ToString(); }


                if (race.Date != null)
                {
                    ert.RaceDate = (DateTime)race.Date;
                    listOfRacesVM.Add(ert);
                }

            }

            return listOfRacesVM;
        }


        private static double CalculatePredicion(double distances, double oldDistance, int oldtime)
        {
            return (RaceCalc.calcPredictedTime(oldDistance, Convert.ToDouble(distances), oldtime) + RaceCalc.cameron(oldDistance, Convert.ToDouble(distances), oldtime)) / 2;
        }

        private void BuildmemberRaceDetailVM(runner thisRunner, RaceListService.Models.memberRaceDetailVM vm)
        {
            vm.RunnerId = thisRunner.EFKey;
            vm.Name = thisRunner.firstname + " " + thisRunner.secondname;
            vm.ukaNumber = thisRunner.ukan;
        }
    }
}