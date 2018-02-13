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
    public class RaceRunnersController : Controller
    {
        private RunningModel.RunningModelEntities db = new RunningModel.RunningModelEntities();

        // GET: RaceRunners
        public ActionResult Index()
        {
            var all = db.runners;
            runnerListVM vm = new runnerListVM();
            vm.listOfRunners = runnerListVM.buildVM(all).ToList();
            vm.SelectedRunnerIDs = runnerListVM.buildSelectedList(vm.listOfRunners);
            return View(vm);
        }

        // GET: RaceRunners/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RunningModel.runner runner = db.runners.Find(id);
            if (runner == null)
            {
                return HttpNotFound();
            }
            return View(runner);
        }

        // GET: RaceRunners/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RaceRunners/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EFKey,firstname,secondname,ukan,dob,email,Active,ageGradeCode")] runner runner)
        {
            if (ModelState.IsValid)
            {
                db.runners.Add(runner);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(runner);
        }

        // GET: RaceRunners/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RunningModel.runner runner = db.runners.Find(id);
            if (runner == null)
            {
                return HttpNotFound();
            }
            return View(runner);
        }

        // POST: RaceRunners/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EFKey,firstname,secondname,ukan,dob,email,Active,ageGradeCode")] runner runner)
        {
            if (ModelState.IsValid)
            {
                db.Entry(runner).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(runner);
        }

        // GET: RaceRunners/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RunningModel.runner runner = db.runners.Find(id);
            if (runner == null)
            {
                return HttpNotFound();
            }
            return View(runner);
        }

        // POST: RaceRunners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RunningModel.runner runner = db.runners.Find(id);
            db.runners.Remove(runner);
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
