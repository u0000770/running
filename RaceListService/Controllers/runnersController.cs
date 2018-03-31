using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RaceListService.Models;
using runners;
using RunningModel;

namespace RaceListService.Controllers
{
    public class runnersController : Controller
    {

        private RunningModelEntities db = new RunningModelEntities();
        //private RunningModel.RunningModelEntities db = new RunningModel.RunningModelEntities();
        //private runners.rrcmlistRunners db = new runners.rrcmlistRunners();

        // GET: runners
        public ActionResult Index()
        {
            var all = db.runners;
            runnerListVM vm = new runnerListVM();
            vm.listOfRunners = runnerListVM.buildVM(all).ToList();
            vm.SelectedRunnerIDs = runnerListVM.buildSelectedList(vm.listOfRunners);
            return View(vm);
        }

        public ActionResult JustLIst()
        {
            var all = db.runners;
            runnerListVM vm = new runnerListVM();
            vm.listOfRunners = runnerListVM.buildVM(all).ToList();
            vm.SelectedRunnerIDs = runnerListVM.buildSelectedList(vm.listOfRunners);
            return View(vm);
        }

        public ActionResult UpdateSelection(RaceListService.Models.runnerListVM vm)
        {

            var all = db.runners;
            foreach (var r in all)
            {
                var runner = db.runners.Find(r.EFKey);
                runner.Active = false;
                db.Entry(runner).State = EntityState.Modified;
                
            }
            db.SaveChanges();
            foreach (var r in vm.SelectedRunnerIDs)
            {
                var runner = db.runners.Find(r);
                runner.Active = true;
                db.Entry(runner).State = EntityState.Modified;
                
            }
            db.SaveChanges();
            return RedirectToAction("Index");


        }

        // GET: runners/Details/5
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

        // GET: runners/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: runners/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EFKey,firstname,secondname,ukan,dob,email,Active")] RunningModel.runner runner)
        {
            if (ModelState.IsValid)
            {
                db.runners.Add(runner);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(runner);
        }

        // GET: runners/Edit/5
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

        // POST: runners/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EFKey,firstname,secondname,ukan,dob,email,Active")] RunningModel.runner runner)
        {
            if (ModelState.IsValid)
            {
                db.Entry(runner).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(runner);
        }

        // GET: runners/Delete/5
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

        // POST: runners/Delete/5
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
