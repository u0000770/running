using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RunningModel;

namespace RaceListService.Controllers
{
    public class RealRaceEventsController : Controller
    {
        private RunningModelEntities db = new RunningModelEntities();

        // GET: RealRaceEvents
        public ActionResult Index()
        {
            var raceEvents = db.RaceEvents.Include(r => r.Event);
            return View(raceEvents.ToList());
        }

        // GET: RealRaceEvents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RaceEvent raceEvent = db.RaceEvents.Find(id);
            if (raceEvent == null)
            {
                return HttpNotFound();
            }
            return View(raceEvent);
        }

        // GET: RealRaceEvents/Create
        public ActionResult Create()
        {
            ViewBag.EFKey = new SelectList(db.Events, "EFKey", "Title");
            return View();
        }

        // POST: RealRaceEvents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RaceEvent raceEvent, string EFKey)
        {
            int RaceId = Convert.ToInt32(EFKey);
            if (ModelState.IsValid)
            {
                raceEvent.EventId = RaceId;
                db.RaceEvents.Add(raceEvent);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EFKey = new SelectList(db.Events, "EFKey", "Title", raceEvent.EFKey);
            return View(raceEvent);
        }

        // GET: RealRaceEvents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RaceEvent raceEvent = db.RaceEvents.Find(id);
            if (raceEvent == null)
            {
                return HttpNotFound();
            }
            ViewBag.EFKey = new SelectList(db.Events, "EFKey", "Title", raceEvent.EFKey);
            return View(raceEvent);
        }

        // POST: RealRaceEvents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EFKey,EventId,Date,Active")] RaceEvent raceEvent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(raceEvent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EFKey = new SelectList(db.Events, "EFKey", "Title", raceEvent.EFKey);
            return View(raceEvent);
        }

        // GET: RealRaceEvents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RaceEvent raceEvent = db.RaceEvents.Find(id);
            if (raceEvent == null)
            {
                return HttpNotFound();
            }
            return View(raceEvent);
        }

        // POST: RealRaceEvents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RaceEvent raceEvent = db.RaceEvents.Find(id);
            db.RaceEvents.Remove(raceEvent);
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
