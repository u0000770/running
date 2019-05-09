using RunningModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


namespace RaceListService.Controllers
{
    public class RunningEventsController : Controller
    {
        private RunningModelEntities db = new RunningModelEntities();

        // GET: RaceEvents
        public ActionResult Index()
        {
            return View(db.Events.ToList());
        }

        // GET: RaceEvents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // GET: RaceEvents/Create
        public ActionResult Create()
        {
            var distances = db.distances;
            var disciplines = db.Disciplines;
            List<SelectListItem> selectDistance = buildDistanceList(distances);
            List<SelectListItem> selectDisciplines = buildDisciplineList(disciplines);
            ViewBag.distances = new SelectList(selectDistance, "Value", "Text");
            ViewBag.disciplines = new SelectList(selectDisciplines, "Value", "Text");
            // ViewBag.BusinessUnitId = new SelectList(bu.All(), "BusinessUnitId", "Title");

            return View();
        }

        private List<SelectListItem> buildDisciplineList(DbSet<Discipline> list)
        {
            List<SelectListItem> slist = new List<SelectListItem>();
      
            foreach (var item in list)
            {
               
                    var option = new SelectListItem()
                    {
                        Text = item.Name,
                        Value = item.Code,
                        Selected = true
                    };
                    slist.Add(option);
               
                }
            return slist;
            }

        private List<SelectListItem> buildDistanceList(DbSet<distance> list)
        {
            List<SelectListItem> slist = new List<SelectListItem>();

            foreach (var item in list)
            {

                var option = new SelectListItem()
                {
                    Text = item.Name,
                    Value = item.Code,
                    Selected = true
                };
                slist.Add(option);

            }
            return slist;
        }

        // POST: RaceEvents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Event @event, string disciplines, string distances)
        {
            if (ModelState.IsValid)
            {
                @event.Discipline = disciplines;
                @event.DistanceCode = distances;
                @event.Active = true;
                db.Events.Add(@event);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(@event);
        }

        // GET: RaceEvents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: RaceEvents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EFKey,Title,Venue,Discipline,DistanceCode,Active")] Event @event)
        {
            if (ModelState.IsValid)
            {
                db.Entry(@event).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(@event);
        }

        // GET: RaceEvents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: RaceEvents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Event @event = db.Events.Find(id);
            db.Events.Remove(@event);
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
