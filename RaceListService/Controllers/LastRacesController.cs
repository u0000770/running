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
    public class LastRacesController : Controller
    {
        private RunningModelEntities db = new RunningModelEntities();

        // GET: LastRaces
        public ActionResult Index()
        {
            var lastRaces = db.LastRaces.Include(l => l.runner);
            return View(lastRaces.ToList());
        }

        // GET: LastRaces/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LastRace lastRace = db.LastRaces.Find(id);
            if (lastRace == null)
            {
                return HttpNotFound();
            }
            return View(lastRace);
        }

        // GET: LastRaces/Create
        public ActionResult Create(int? id)
        {

            var thisRunner = db.runners.Find(id);
            ViewBag.ThisRunnerId = thisRunner.EFKey;
            ViewBag.RunnerName = thisRunner.firstname + " " + thisRunner.secondname;
            var eventList = db.Events;
            ViewBag.distanceList = buildSelectList(eventList);
            var lastRace = db.LastRaces.SingleOrDefault(r => r.RunnerId == thisRunner.EFKey);
            if (lastRace != null)
            {
                var vm = new EditLastRaceVM(lastRace);
                return View(vm);
            }
            else
            {
                return View();
            }

            
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

        // POST: LastRaces/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string distanceList, EditLastRaceVM vm, DateTime? raceDate, int? ThisRunnerId)
        {
            // get the runner
            var runner = db.runners.SingleOrDefault(r => r.EFKey == ThisRunnerId);
            // get the event
            var thisEvent = db.Events.Find(Convert.ToInt32(distanceList));
            // get the raw distance value for this event
            var selectedDistance = db.distances.SingleOrDefault(d => d.Code == thisEvent.DistanceCode).Value;

            // create a new EventRunnerTime 
            var nextRace = new EventRunnerTime();
            nextRace.EventId = thisEvent.EFKey;
            nextRace.RunnerId = runner.EFKey;
            nextRace.Actual = Convert.ToInt32(vm.RaceTimeSpan.TotalSeconds);
            nextRace.Date = (DateTime)raceDate;

            // try grab hold of this runners last race and update it
            var lastRace = db.LastRaces.SingleOrDefault(l => l.RunnerId == runner.EFKey);
            if (lastRace != null)
            { 
                lastRace.Distance = selectedDistance;
                lastRace.Time = Convert.ToInt32(vm.RaceTimeSpan.TotalSeconds);
                lastRace.Date = (DateTime)raceDate;
                db.Entry(lastRace).State = EntityState.Modified;
            }
            else // they dont have a last race !!
            {
                var newLastRace = new RunningModel.LastRace();
                newLastRace.Distance = selectedDistance;
                newLastRace.Time = Convert.ToInt32(vm.RaceTimeSpan.TotalSeconds);
                newLastRace.Date = (DateTime)raceDate;
                newLastRace.RunnerId = runner.EFKey;
                db.LastRaces.Add(newLastRace);
            }
            // update last race and event runner times 
            try
            {
                // get an existing race if it exisits
                var ert = db.EventRunnerTimes.SingleOrDefault(rt => rt.EventId == nextRace.EventId && rt.RunnerId == nextRace.RunnerId && rt.Date == (DateTime)raceDate);
                if (ert == null) // if not add it
                {
                    db.EventRunnerTimes.Add(nextRace);
                }
                else            // if it does exist update it
                {
                    ert.Actual = nextRace.Actual;
                    db.Entry(lastRace).State = EntityState.Modified;
                }
                
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

        // GET: LastRaces/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LastRace lastRace = db.LastRaces.Find(id);
            if (lastRace == null)
            {
                return HttpNotFound();
            }
            ViewBag.RunnerId = new SelectList(db.runners, "EFKey", "firstname", lastRace.RunnerId);
            return View(lastRace);
        }

        // POST: LastRaces/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EFKey,RunnerId,Distance,Time,Date")] LastRace lastRace)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lastRace).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RunnerId = new SelectList(db.runners, "EFKey", "firstname", lastRace.RunnerId);
            return View(lastRace);
        }

        // GET: LastRaces/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LastRace lastRace = db.LastRaces.Find(id);
            if (lastRace == null)
            {
                return HttpNotFound();
            }
            return View(lastRace);
        }

        // POST: LastRaces/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LastRace lastRace = db.LastRaces.Find(id);
            db.LastRaces.Remove(lastRace);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

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
