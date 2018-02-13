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
    public class racedistancesController : Controller
    {
        private RunningModelEntities db = new RunningModelEntities();

        // GET: racedistances
        public ActionResult Index()
        {
            return View(db.distances.ToList());
        }

        // GET: racedistances/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            distance distance = db.distances.Find(id);
            if (distance == null)
            {
                return HttpNotFound();
            }
            return View(distance);
        }

        // GET: racedistances/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: racedistances/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EFKey,Code,Name,Value")] distance distance)
        {
            if (ModelState.IsValid)
            {
                db.distances.Add(distance);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(distance);
        }

        // GET: racedistances/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            distance distance = db.distances.Find(id);
            if (distance == null)
            {
                return HttpNotFound();
            }
            return View(distance);
        }

        // POST: racedistances/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EFKey,Code,Name,Value")] distance distance)
        {
            if (ModelState.IsValid)
            {
                db.Entry(distance).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(distance);
        }

        // GET: racedistances/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            distance distance = db.distances.Find(id);
            if (distance == null)
            {
                return HttpNotFound();
            }
            return View(distance);
        }

        // POST: racedistances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            distance distance = db.distances.Find(id);
            db.distances.Remove(distance);
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
