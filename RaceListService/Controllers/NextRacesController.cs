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
    public class NextRacesController : Controller
    {
        private RunningModelEntities db = new RunningModelEntities();


       
            // GET: NextRaces
            public ActionResult Index()
             {
                var distances = db.distances;
                ViewBag.distances = buildSelectList(distances);
                var nextRaces = db.NextRaces.Include(n => n.runner);
                return View(nextRaces.ToList());
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
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NextRace nextRace = db.NextRaces.Find(id);
            if (nextRace == null)
            {
                return HttpNotFound();
            }
            ViewBag.RunnerId = new SelectList(db.runners, "EFKey", "firstname", nextRace.RunnerId);
            return View(nextRace);
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
