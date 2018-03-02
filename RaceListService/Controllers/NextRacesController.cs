using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RaceListService.Models;
using RunningModel;



namespace RaceListService.Controllers
{
    public class NextRacesController : Controller
    {
        private RunningModelEntities db = new RunningModelEntities();

        #region Display List of Last Races
        /// <summary>
        /// Display a list of Last Races from LastRace collection for each runner
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            // get all events
            var eventList = db.Events;
            // build a list of events to select one from
            ViewBag.distanceList = buildSelectList(eventList);
            // get all runners
            var allRunners = db.runners.OrderBy(n => n.secondname);
            List<nextRaceVM> vm = new List<nextRaceVM>();
            // construct the viewmodel - list of Last Races 
            BuildListofLastRaces(allRunners, vm);
            return View(vm);
        }

        private void BuildListofLastRaces(IOrderedQueryable<runner> allRunners, List<nextRaceVM> vm)
        {
            foreach (var m in allRunners)
            {
                var nr = new nextRaceVM();
                var last = db.LastRaces.SingleOrDefault(l => l.RunnerId == m.EFKey);
                if (last != null)
                {
                    nr.RunnerId = m.EFKey;
                    nr.LastDistance = db.distances.SingleOrDefault(d => d.Value == last.Distance).Name;
                    nr.LastTime = RaceCalc.formatTime(last.Time);
                    nr.RunnerName = m.firstname + " " + m.secondname;
                    nr.Time = RaceCalc.formatTime(last.Time);
                    vm.Add(nr);
                }
            }
        }


        #endregion


        #region Update NextRace and EventRunnerTimes with new prediction

        /// <summary>
        /// Updates NextRace and EventRunnerTimes with predicted time when called by Index View Update Button 
        /// </summary>
        /// <param name="raceDate"></param>
        /// <param name="distanceList"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult NewTarget(DateTime? raceDate, string distanceList)
        {

            var thisEvent = db.Events.Find(Convert.ToInt32(distanceList));
            var dCode = thisEvent.DistanceCode;
            var distances = db.distances.SingleOrDefault(d => d.Code == dCode).Value;
            CleanNextRaces();
            var allRunners = db.runners;
            // foreach runner update next race predicted time and either add to EventRunnerTimes or update an existing EventRunnerTimes
            UpdateRaceTimes(raceDate, thisEvent, distances, allRunners);
            db.SaveChanges();

            // Build VM for next Race List
            var allNextRaces = db.NextRaces;
            List<nextRaceVM> vm = new List<nextRaceVM>();
            BuildNextRaceVM(allNextRaces, vm);
            var ds = Convert.ToDouble(distances);
            ViewBag.NewDistance = db.distances.SingleOrDefault(d => d.Value == ds).Name;
            return View(vm);
        }

        /// <summary>
        /// Build Next Race VM
        /// </summary>
        /// <param name="allNextRaces"></param>
        /// <param name="vm"></param>
        private void BuildNextRaceVM(DbSet<NextRace> allNextRaces, List<nextRaceVM> vm)
        {
            foreach (var m in allNextRaces)
            {
                var nr = new nextRaceVM();
                var last = db.LastRaces.SingleOrDefault(l => l.RunnerId == m.RunnerId);
                nr.LastDistance = db.distances.SingleOrDefault(d => d.Value == last.Distance).Name;
                nr.LastTime = RaceCalc.formatTime(last.Time);
                nr.RunnerName = m.runner.firstname + " " + m.runner.secondname;
                nr.Time = RaceCalc.formatTime(m.Time);
                vm.Add(nr);
            }
        }

        /// <summary>
        /// foreach runner update next race predicted time and either add to EventRunnerTimes or update an existing EventRunnerTimes
        /// </summary>
        /// <param name="raceDate"></param>
        /// <param name="thisEvent"></param>
        /// <param name="distances"></param>
        /// <param name="allRunners"></param>
        private void UpdateRaceTimes(DateTime? raceDate, Event thisEvent, double distances, DbSet<runner> allRunners)
        {
            foreach (var r in allRunners)
            {
                /// get the last race for this runner
                var lastRace = r.LastRaces.FirstOrDefault(s => s.RunnerId == r.EFKey);
                if (lastRace != null)
                {
                    /// use the last race details to calculate time for next race
                    var oldDistance = lastRace.Distance;
                    var oldtime = lastRace.Time;
                    double predictedTime = CalculatePredicion(distances, oldDistance, oldtime);
                    /// need to check if this is an existing or new entry
                    var ert = db.EventRunnerTimes.SingleOrDefault(rt => rt.EventId == thisEvent.EFKey && rt.RunnerId == r.EFKey && rt.Date == raceDate);
                    if (ert == null)
                    {
                        var nextRaceEvent = new EventRunnerTime();
                        nextRaceEvent.EventId = thisEvent.EFKey;
                        nextRaceEvent.RunnerId = r.EFKey;
                        nextRaceEvent.Date = raceDate;
                        nextRaceEvent.Target = Convert.ToInt32(predictedTime);
                        db.EventRunnerTimes.Add(nextRaceEvent);
                    }
                    else
                    {
                        ert.Target = Convert.ToInt32(predictedTime);
                        db.Entry(ert).State = EntityState.Modified;
                    }
                


                    RunningModel.NextRace nextrace = new NextRace();
                    nextrace.RunnerId = r.EFKey;
                    nextrace.Distance = Convert.ToDouble(distances);
                    nextrace.Time = Convert.ToInt32(predictedTime);
                    nextrace.Active = true;
                    db.NextRaces.Add(nextrace);

                }
            }
        }

        /// <summary>
        /// Cleans Next Race Table so new times can be enttered
        /// </summary>
        private void CleanNextRaces()
        {
            db.NextRaces.RemoveRange(db.NextRaces);
            db.SaveChanges();
        }

        #endregion

        private static double CalculatePredicion(double distances, double oldDistance, int oldtime)
        {
            return (RaceCalc.calcPredictedTime(oldDistance, Convert.ToDouble(distances), oldtime) + RaceCalc.cameron(oldDistance, Convert.ToDouble(distances), oldtime)) / 2;
        }

        private List<SelectListItem> buildSelectList(IEnumerable<Event> inList)
        {
            List<SelectListItem> slist = new List<SelectListItem>();

            foreach (var item in inList)
            {
                //if (item.Value == targetdistance)
                //{
                //    var option = new SelectListItem()
                //    {
                //        Text = item.Code,
                //        Value = item.Value.ToString(),
                //        Selected = true
                //    };
                //    slist.Add(option);
                //}
                //else
                //{
                var option = new SelectListItem()
                {
                    Text = item.Title,
                    Value = item.EFKey.ToString(),
                };
                slist.Add(option);
            }
            return slist;

        }

        private List<SelectListItem> buildSelectList(IEnumerable<Event> inList, string currentEvent)
        {
            List<SelectListItem> slist = new List<SelectListItem>();

            foreach (var item in inList)
            {
                if (item.EFKey == Convert.ToInt32(currentEvent))
                {
                    var option = new SelectListItem()
                    {
                        Text = item.Title,
                        Value = item.EFKey.ToString(),
                        Selected = true
                    };
                    slist.Add(option);
                }
                else
                {
                    var option = new SelectListItem()
                    {
                        Text = item.Title,
                        Value = item.EFKey.ToString(),
                    };
                    slist.Add(option);
                }
                

            }
            return slist;
        }

        private List<SelectListItem> buildSelectList(IEnumerable<distance> inList)
        {
            List<SelectListItem> slist = new List<SelectListItem>();

            foreach (var item in inList)
            {
                //if (item.Value == targetdistance)
                //{
                //    var option = new SelectListItem()
                //    {
                //        Text = item.Code,
                //        Value = item.Value.ToString(),
                //        Selected = true
                //    };
                //    slist.Add(option);
                //}
                //else
                //{
                var option = new SelectListItem()
                {
                    Text = item.Code,
                    Value = item.Value.ToString(),
                };
                slist.Add(option);
            }
            return slist;

        }

        private List<SelectListItem> buildSelectList(IEnumerable<distance> inList, double targetdistance)
        {
            List<SelectListItem> slist = new List<SelectListItem>();

            foreach (var item in inList)
            {
                if (item.Value == targetdistance)
                {
                    var option = new SelectListItem()
                    {
                        Text = item.Code,
                        Value = item.Value.ToString(),
                        Selected = true
                    };
                    slist.Add(option);
                }
                else
                {
                    var option = new SelectListItem()
                    {
                        Text = item.Code,
                        Value = item.Value.ToString(),
                    };
                    slist.Add(option);
                }


            }
            return slist;
        }


        #region Display Last Race and list of EventRaceTimes for specific runner

        // GET: NextRaces/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            runner thisRunner = db.runners.Find(id);
            if (thisRunner == null)
            {
                return HttpNotFound();
            }
            var vm = new memberRaceDetailVM();
            BuildmemberRaceDetailVM(id, vm);

            List<EventRaceTimesVM> listOfRacesVM = BuildEventRaceList(thisRunner);
            vm.listOfRaces = listOfRacesVM.OrderBy(r => r.RaceDate).ToList();
            return View(vm);
        }

        /// <summary>
        /// Builds the Core VM for MemberRaceDetailVM
        /// </summary>
        /// <param name="id"></param>
        /// <param name="vm"></param>
        private void BuildmemberRaceDetailVM(int? id, memberRaceDetailVM vm)
        {
            vm.RunnerId = Convert.ToInt32(id);
            var lastRace = db.LastRaces.SingleOrDefault(r => r.RunnerId == vm.RunnerId);
            if (lastRace != null)
            {
                vm.lastRaceId = lastRace.RunnerId;
                vm.lastRaceTime = EventRaceTimesVM.formatResult(lastRace.Time);
                vm.lastRaceDistance = db.distances.Single(d => d.Value == lastRace.Distance).Name;
                vm.lastRaceDate = lastRace.Date.Date;
            }
        }

        /// <summary>
        /// Builds list of races from EventRunnerTimes for this runner
        /// </summary>
        /// <param name="thisRunner"></param>
        /// <returns>List<EventRaceTimesVM></returns>
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
                ert.RaceDate = (DateTime)race.Date;
                int x = race.Actual ?? 0;
                ert.RaceActualTime = EventRaceTimesVM.formatResult(x);
                int y = race.Target ?? 0;
                ert.RaceTargetTime = EventRaceTimesVM.formatResult(y);
                listOfRacesVM.Add(ert);
            }

            return listOfRacesVM;
        }

        #endregion

        #region Edit Last Race for a specific runner
        // GET: NextRaces/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "NextRaces");
            }
            LastRace Race = db.LastRaces.SingleOrDefault(r => r.RunnerId == id);
            if (Race == null)
            {
                return RedirectToAction("Index", "NextRaces");
            }
            var eventList = db.Events;
            ViewBag.distanceList = buildSelectList(eventList);
            var vm = new EditLastRaceVM(Race);
            return View(vm);
        }

        // TO DO upate the UI and method so we can use a specified date
        /// <summary>
        /// 
        /// </summary>
        /// <param name="distanceList">This is An Event ID </param>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(string distanceList, EditLastRaceVM vm)
        {
            // get the runner
            var runner = db.runners.SingleOrDefault(r => r.EFKey == vm.RunnerId);
            // get the event
            var thisEvent = db.Events.Find(Convert.ToInt32(distanceList));
            // get the raw distance value for this event
            var selectedDistance = db.distances.SingleOrDefault(d => d.Code == thisEvent.DistanceCode).Value;

            // create a new EventRunnerTime 
            var nextRace = new EventRunnerTime();
            nextRace.EventId = thisEvent.EFKey;
            nextRace.RunnerId = runner.EFKey;
            nextRace.Actual = Convert.ToInt32(vm.RaceTimeSpan.TotalSeconds);
            nextRace.Date = DateTime.Now;

            // grab hold of this runners last race and update it
            var lastRace = db.LastRaces.SingleOrDefault(l => l.RunnerId == runner.EFKey);
            lastRace.Distance = selectedDistance;
            lastRace.Time = Convert.ToInt32(vm.RaceTimeSpan.TotalSeconds);
            lastRace.Date = DateTime.Now;
            
            // update last race and event runner times 
            try {
                // get an existing race if it exisits
                var ert = db.EventRunnerTimes.SingleOrDefault(rt => rt.EventId == nextRace.EventId && rt.RunnerId == nextRace.RunnerId);
                if (ert == null) // if not add it
                {
                    db.EventRunnerTimes.Add(nextRace);
                }
                else            // if it does exist update it
                {
                    ert.Actual = nextRace.Actual;
                    db.Entry(lastRace).State = EntityState.Modified;
                }
                db.Entry(lastRace).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "NextRaces");
            }
            catch
            {
                LastRace Race = db.LastRaces.SingleOrDefault(r => r.RunnerId == vm.RunnerId);
                if (Race == null)
                {
                    return RedirectToAction("Index", "NextRaces");
                }
                var eventList = db.Events;
                ViewBag.distanceList = buildSelectList(eventList);
                var nvm = new EditLastRaceVM(Race);
                return View(nvm);
            }
        }

#endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

    
