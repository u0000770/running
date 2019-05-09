using RaceListService.Models;
using RunningModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RaceListService.Controllers
{
    public class NewNextRaceController : Controller
    {
        private RunningModelEntities db = new RunningModelEntities();

        /// <summary>
        /// Build a List of Race events to select next race from
        /// </summary>
        /// <param name="elist"></param>
        /// <returns></returns>
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


        /// <summary>
        /// Calculate Prediction
        /// </summary>
        /// <param name="distances"></param>
        /// <param name="oldDistance"></param>
        /// <param name="oldtime"></param>
        /// <returns></returns>
        private static double CalculatePredicion(double distances, double oldDistance, int oldtime)
        {
            return (RaceCalc.calcPredictedTime(oldDistance, Convert.ToDouble(distances), oldtime) + RaceCalc.cameron(oldDistance, Convert.ToDouble(distances), oldtime)) / 2;
        }


        // Empty next Race Table 
        private void CleanNextRaces()
        {
            db.NextRaces.RemoveRange(db.NextRaces);
            db.SaveChanges();
        }

        private List<EventRaceTimesVM> BuildEventRaceList(runner thisRunner)
        {
            List<EventRaceTimesVM> listOfRacesVM = new List<EventRaceTimesVM>();
            var listOfRace = db.EventRunnerTimes.Where(r => r.RunnerId == thisRunner.EFKey);
            foreach (var race in listOfRace)
            {
                EventRaceTimesVM ert = new EventRaceTimesVM();
                ert.RaceId = race.EFKey;
                ert.RaceDistance = db.distances.SingleOrDefault(d => d.Code == race.Event.DistanceCode).Name;
                ert.RaceTitle = race.Event.Title;

                int x = race.Actual ?? 0;
                ert.RaceActualTime = EventRaceTimesVM.formatResult(x);
                int y = race.Target ?? 0;
                ert.RaceTargetTime = EventRaceTimesVM.formatResult(y);
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


        private List<newNextRaceVM> BuildNextRaceVM(DbSet<NextRace> allNextRaces)
        { 

            List<newNextRaceVM> outList = new List<newNextRaceVM>();
            foreach (var m in allNextRaces)
            {

                var nr = new newNextRaceVM();
                nr.PredictedTime = RaceCalc.formatTime(m.Time);
                nr.RunnerName = m.runner.secondname + " " + m.runner.firstname;
                nr.RunnerId = m.RunnerId;
                nr.UKAN = m.runner.ukan;
                outList.Add(nr);
            }

            return outList;
        }

        private bool IsAdmin()
        {
            try
            {
                var admin = (bool)Session["admin"];
                if (admin)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch
            {
                return false;
            }
        }


        // GET: Display List of Races to Select from
        public ActionResult Index()
        {
            var admin = IsAdmin();

            if (admin)
            {
                // var elist = db.RaceEvents.Where(d => d.Date >= DateTime.Now).OrderBy(r => r.Date).Take(4);
                var elist = db.RaceEvents.Where(d => d.Date >= DateTime.Now).Take(5);
                var list = buildSelectList(elist);
                ViewBag.elistValue = new SelectList(list, "Value", "Text");
                ViewBag.Admin = true;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Default");
            }
        }


        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return RedirectToAction("Index", "Default");
        //    }

        //    runner thisRunner = db.runners.Find(id);
        //    if (thisRunner == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    var vm = new memberRaceDetailVM();
        //    BuildmemberRaceDetailVM(id, vm);

        //    List<EventRaceTimesVM> listOfRacesVM = BuildEventRaceList(thisRunner);
        //    var total = 0;
        //    foreach (EventRaceTimesVM ev in listOfRacesVM)
        //    {
        //        total = total + Convert.ToInt32(ev.TimeDifference);
        //    }
        //    vm.listOfRaces = listOfRacesVM.OrderBy(r => r.RaceDate).ToList();

        //    var admin = IsAdmin();

        //    if (admin)
        //    {
        //        ViewBag.Admin = true;
        //        vm.admin = true;
        //    }
        //    else
        //    {
        //        ViewBag.Admin = false;
        //        vm.admin = false;
        //    }
        //    vm.totalTime = total;
        //    return View(vm);
        //}



        /// <summary>
        /// Updates NextRace and EventRunnerTimes with predicted time when called by Index View Update Button 
        /// </summary>
        /// <param name="raceDate"></param>
        /// <param name="distanceList"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult NewTarget(string elistValue)
        {
            
            // get the selected Race details
            var nextRaceEvent = db.RaceEvents.Find(Convert.ToInt32(elistValue));
            var nextDCode = nextRaceEvent.Event.DistanceCode;
            var newdistance = db.distances.SingleOrDefault(d => d.Code == nextDCode).Value;

            // Clean Next Race Table
            CleanNextRaces();

            // Get the valid set of runners must be active and have some races
            var yearAgo = DateTime.Now.AddYears(-1);
            var allRunners = db.runners.Where(r => (r.Active == true) && (r.EventRunnerTimes.Where(d => d.Actual != null && d.Date > yearAgo).Count()) > 0 );
            List<runner> all = allRunners.ToList();
            // foreach runner update next race predicted time and either add to EventRunnerTimes or update an existing EventRunnerTimes
            UpdateRaceTimes(nextRaceEvent,allRunners, newdistance);
            db.SaveChanges();

            // Build VM for next Race List
            var allNextRaces = db.NextRaces;
            List<newNextRaceVM> vm = BuildNextRaceVM(allNextRaces);
            var elist = db.RaceEvents.Where(d => d.Date > DateTime.Now).OrderBy(r => r.Date).Take(4);
            var list = buildSelectList(elist);
            ViewBag.elistValue = new SelectList(list, "Value", "Text");
            ViewBag.Admin = true;
            return View("Index",vm.OrderBy(r => r.PredictedTime));
        }

        /// <summary>
        /// foreach runner update next race predicted time and either add to EventRunnerTimes or update an existing EventRunnerTimes
        /// </summary>
        /// <param name="nextEvent"></param>
        /// <param name="newDistances"></param>
        /// <param name="allRunners"></param>
        private void UpdateRaceTimes(RaceEvent nextEvent,IEnumerable<runner> allRunners, double newDistance)
        {
            var allDistances = db.distances;
            foreach (var r in allRunners)
            {
                /// get the last 3 races for this runner
                var listOfRace = db.EventRunnerTimes.Where(x => x.RunnerId == r.EFKey && x.Actual != null && x.Actual > 0);
                var lastThreeRaces = listOfRace.OrderByDescending(y => y.Date).Take(3);

                // for each race

                double totalTime = 0;
                int numberOfRaces = 0;
                foreach (var race in lastThreeRaces)
                {
                    var oldTime = (int)race.Actual;
                    var oldDistance = allDistances.SingleOrDefault(d => d.Code == race.Event.DistanceCode).Value;
                    int y = (int)CalculatePredicion(newDistance,oldDistance,oldTime);
                    numberOfRaces++;
                    totalTime = totalTime + y;
                }
                var newPreditedTime = totalTime / numberOfRaces;

                var thisEvent = nextEvent.Event;
                var raceDate = nextEvent.Date;

                    /// need to check if this is an existing or new entry
                var ert = db.EventRunnerTimes.SingleOrDefault(rt => rt.EventId == thisEvent.EFKey && rt.RunnerId == r.EFKey && rt.Date == raceDate);
                    if (ert == null)
                    {
                        var nextRaceEvent = new EventRunnerTime();
                        nextRaceEvent.EventId = thisEvent.EFKey;
                        nextRaceEvent.RunnerId = r.EFKey;
                        nextRaceEvent.Date = raceDate;
                        nextRaceEvent.Target = Convert.ToInt32(newPreditedTime);
                        db.EventRunnerTimes.Add(nextRaceEvent);
                    }
                    else
                    {
                        ert.Target = Convert.ToInt32(newPreditedTime);
                        db.Entry(ert).State = EntityState.Modified;
                    }



                    RunningModel.NextRace nextrace = new NextRace();
                    nextrace.RunnerId = r.EFKey;
                    nextrace.Distance = Convert.ToDouble(newDistance);
                    nextrace.Time = Convert.ToInt32(newPreditedTime);
                    nextrace.Active = true;
                    db.NextRaces.Add(nextrace);

                }
            }
        }

     
       



}