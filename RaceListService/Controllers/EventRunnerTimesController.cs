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
    public class EventRunnerTimesController : Controller
    {
        private RunningModelEntities db = new RunningModelEntities();

        // GET: EventRunnerTimes
        public ActionResult Index()
        {
            var eventRunnerTimes = db.EventRunnerTimes.Include(e => e.Event).Include(e => e.runner);
            return View(eventRunnerTimes.ToList());
        }

        // GET: EventRunnerTimes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventRunnerTime eventRunnerTime = db.EventRunnerTimes.Find(id);
            if (eventRunnerTime == null)
            {
                return HttpNotFound();
            }
            return View(eventRunnerTime);
        }

        // GET: EventRunnerTimes/Create
        public ActionResult Create()
        {
            ViewBag.EventId = new SelectList(db.Events, "EFKey", "Title");
            ViewBag.RunnerId = new SelectList(db.runners, "EFKey", "secondname");
            return View();
        }

        // POST: EventRunnerTimes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EFKey,RunnerId,EventId,Target,Actual,Date,Active")] EventRunnerTime eventRunnerTime)
        {
            if (ModelState.IsValid)
            {
                db.EventRunnerTimes.Add(eventRunnerTime);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EventId = new SelectList(db.Events, "EFKey", "Title", eventRunnerTime.EventId);
            ViewBag.RunnerId = new SelectList(db.runners, "EFKey", "firstname", eventRunnerTime.RunnerId);
            return View(eventRunnerTime);
        }

        // TODO: Form created post form update to complete
        // GET: EventRunnerTimes/Edit/5
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventRunnerTime eventRunnerTime = db.EventRunnerTimes.Find(id);
            EditEventRunnerTimeVM vm = new EditEventRunnerTimeVM();
            if (eventRunnerTime == null)
            {
                return HttpNotFound();
            }
            var runner = db.runners.Find(eventRunnerTime.RunnerId);
            vm.RaceDate = (DateTime)eventRunnerTime.Date;
            vm.RaceTargetTime = EventRaceTimesVM.formatResult(Convert.ToInt32(eventRunnerTime.Target));
            vm.RaceTitle = eventRunnerTime.Event.Title;
            vm.RaceId = eventRunnerTime.EFKey;
            vm.RaceActualTime = eventRunnerTime.Actual;
            vm.RunnerName = runner.firstname + " " + runner.secondname;
            vm.RunnerId = eventRunnerTime.RunnerId;
           // vm.RaceActualTime = eventRunnerTime.Actual;
            return View(vm);
        }

        // POST: EventRunnerTimes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditEventRunnerTimeVM vm)
        {
            if (ModelState.IsValid)
            {
                var ert = db.EventRunnerTimes.Find(vm.RaceId);
                ert.Actual = vm.RaceActualTime;
                db.Entry(ert).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "runners", new { id = vm.RunnerId});
            }
           
            return View(vm);
        }


        ////[HttpPost, ActionName("BatchDelete")]
        ////[ValidateAntiForgeryToken]
        //public ActionResult BatchDelete()
        //{
        //    var killdate = DateTime.Now.AddYears(-1);
        //    var deadrecords = db.EventRunnerTimes.Where(r => r.Date < killdate);
        //    if (deadrecords.Count() > 0)
        //    {
        //        foreach (var objR in deadrecords)
        //        {
        //            deadrecords.
        //        }
        //    }
        //    objBS.SaveChanges();
        //    return View(deadrecords);
        //}

        // GET: EventRunnerTimes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventRunnerTime eventRunnerTime = db.EventRunnerTimes.Find(id);
            if (eventRunnerTime == null)
            {
                return HttpNotFound();
            }
            return View(eventRunnerTime);
        }

        // POST: EventRunnerTimes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EventRunnerTime eventRunnerTime = db.EventRunnerTimes.Find(id);
            db.EventRunnerTimes.Remove(eventRunnerTime);
            db.SaveChanges();
            return RedirectToAction("Index", "NextRaces");
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
