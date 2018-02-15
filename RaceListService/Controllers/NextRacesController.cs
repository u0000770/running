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


       
            // GET: NextRaces
            public ActionResult Index()
             {
                var distances = db.distances;
                var list = distances.ToList();
                ViewBag.distances = buildSelectList(distances);
                 var allRunners = db.runners.Include(n => n.LastRaces);
                 List<nextRaceVM> vm = new List<nextRaceVM>();
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
            return View(vm);
            }

        public ActionResult NewTarget(string distances)
        {
            db.NextRaces.RemoveRange(db.NextRaces);
            var allRunners = db.runners.Include(n => n.LastRaces);
            // foreach runner 
            foreach(var r in allRunners)
            {
                var lastRace = r.LastRaces.FirstOrDefault(s => s.RunnerId == r.EFKey);
                if (lastRace != null)
                {
                    var oldDistance = lastRace.Distance;
                    var oldtime = lastRace.Time;
                    var predictedTime = (RaceCalc.calcPredictedTime(oldDistance, Convert.ToDouble(distances), oldtime) + RaceCalc.cameron(oldDistance, Convert.ToDouble(distances), oldtime)) / 2;
                    RunningModel.NextRace nextrace = new NextRace();
                    nextrace.RunnerId = r.EFKey;
                    nextrace.Distance = Convert.ToDouble(distances);
                    nextrace.Time = Convert.ToInt32(predictedTime);
                    nextrace.Active = true;
                    db.NextRaces.Add(nextrace);
                    
                }
            }
            db.SaveChanges();

            // get last race time and distance
            // calculate next time for next time
            // update next date time and distance
            // contruct view model to display new times.
            var allNextRaces = db.NextRaces;
            List<nextRaceVM> vm = new List<nextRaceVM>();
            foreach(var m in allNextRaces)
            {
                var nr = new nextRaceVM();
                var last = db.LastRaces.SingleOrDefault(l => l.RunnerId == m.RunnerId);
                nr.LastDistance = db.distances.SingleOrDefault(d => d.Value == last.Distance).Name;
                nr.LastTime = RaceCalc.formatTime(last.Time);
                nr.RunnerName = m.runner.firstname + " " + m.runner.secondname;
                nr.Time = RaceCalc.formatTime(m.Time);
                vm.Add(nr);
            }
            var ds = Convert.ToDouble(distances);
            ViewBag.NewDistance = db.distances.SingleOrDefault(d => d.Value == ds).Name;
            return View(vm);
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



        // GET: NextRaces/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NextRace nextRace = db.NextRaces.Find(id);
            if (nextRace == null)
            {
                return HttpNotFound();
            }
            return View(nextRace);
        }

        // GET: NextRaces/Create
        public ActionResult Create()
        {
            ViewBag.RunnerId = new SelectList(db.runners, "EFKey", "firstname");
            return View();
        }

        // POST: NextRaces/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EFKey,RunnerId,Distance,Time,Active")] NextRace nextRace)
        {
            if (ModelState.IsValid)
            {
                db.NextRaces.Add(nextRace);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RunnerId = new SelectList(db.runners, "EFKey", "firstname", nextRace.RunnerId);
            return View(nextRace);
        }

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
            var distances = db.distances;
            ViewBag.distances = buildSelectList(distances, Race.Distance);
            return View(Race);
        }

        // POST: NextRaces/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EFKey,RunnerId,Distance,Time,Active")] NextRace nextRace)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nextRace).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RunnerId = new SelectList(db.runners, "EFKey", "firstname", nextRace.RunnerId);
            return View(nextRace);
        }

        // GET: NextRaces/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NextRace nextRace = db.NextRaces.Find(id);
            if (nextRace == null)
            {
                return HttpNotFound();
            }
            return View(nextRace);
        }

        // POST: NextRaces/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NextRace nextRace = db.NextRaces.Find(id);
            db.NextRaces.Remove(nextRace);
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
