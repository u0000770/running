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
    public class DisciplinesController : Controller
    {
        private RunningModel.RunningModelEntities db = new RunningModel.RunningModelEntities();

        // GET: Disciplines
        public ActionResult Index()
        {
            return View(db.Disciplines.ToList());
        }

        // GET: Disciplines/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RunningModel.Discipline discipline = db.Disciplines.Find(id);
            if (discipline == null)
            {
                return HttpNotFound();
            }
            return View(discipline);
        }

        // GET: Disciplines/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Disciplines/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,Code,Name,Active")] RunningModel.Discipline discipline)
        {
            if (ModelState.IsValid)
            {
                db.Disciplines.Add(discipline);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(discipline);
        }

        // GET: Disciplines/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RunningModel.Discipline discipline = db.Disciplines.Find(id);
            if (discipline == null)
            {
                return HttpNotFound();
            }
            return View(discipline);
        }

        // POST: Disciplines/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Code,Name,Active")] RunningModel.Discipline discipline)
        {
            if (ModelState.IsValid)
            {
                db.Entry(discipline).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(discipline);
        }

        // GET: Disciplines/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RunningModel.Discipline discipline = db.Disciplines.Find(id);
            if (discipline == null)
            {
                return HttpNotFound();
            }
            return View(discipline);
        }

        // POST: Disciplines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RunningModel.Discipline discipline = db.Disciplines.Find(id);
            db.Disciplines.Remove(discipline);
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
