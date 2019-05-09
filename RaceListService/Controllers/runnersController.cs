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
        private bool IsAdmin()
        {
            try
            {
                var admin = (bool)Session["admin"];
                if (admin)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch
            {
                return false;
            }
        }
        // GET: runners
        public ActionResult Index()
        {
            if (IsAdmin())
            {
                ViewBag.Admin = true;
                var all = db.runners;
                //List<RunningModel.runner> qual = getActiveRunners(all);
                List<RunningModel.runner> qual = all.ToList();
                runnerListVM vm = new runnerListVM();
                vm.listOfRunners = runnerListVM.buildVM(qual).ToList();
                vm.SelectedRunnerIDs = runnerListVM.buildSelectedList(vm.listOfRunners);
                return View(vm);
            }
            else
            {
                return RedirectToAction("Index", "Default");
            }
        }

        private List<RunningModel.runner> getActiveRunners(DbSet<RunningModel.runner> all)
        {
            List<RunningModel.runner> outlist = new List<RunningModel.runner>();
            var yearAgo = DateTime.Now.AddYears(-1);
            foreach (var r in all)
            {
                var id = r.EFKey;
                var runs = db.EventRunnerTimes.Where(x => x.RunnerId == id && x.Actual != null && x.Date >= yearAgo);
                if (runs.Count() > 0)
                {
                    outlist.Add(r);
                }
            }

            return outlist;
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


        private void BuildmemberRaceDetailVM(RunningModel.runner thisRunner, RaceListService.Models.memberRaceDetailVM vm)
        {
            vm.RunnerId = thisRunner.EFKey;
            vm.Name = thisRunner.firstname + " " + thisRunner.secondname;
            vm.ukaNumber = thisRunner.ukan;
        }

       

        // GET: runners/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RunningModel.runner thisRunner = db.runners.Find(id);
            // get list of races for runner 
            if (thisRunner == null)
            {
                return HttpNotFound();
            }

            var vm = new RaceListService.Models.memberRaceDetailVM();
            BuildmemberRaceDetailVM(thisRunner, vm);
            List<RaceListService.Models.EventRaceTimesVM> listOfRacesVM = BuildEventRaceList(thisRunner);
            ViewBag.Runner = vm;
            return View(listOfRacesVM);
        }

        private List<EventRaceTimesVM> BuildEventRaceList(RunningModel.runner thisRunner)
        {

            List<EventRaceTimesVM> VM = new List<EventRaceTimesVM>();
            var listOfRaces = db.EventRunnerTimes.Where(r => r.RunnerId == thisRunner.EFKey);
            DateTime startOfYear = new DateTime(2018, 12, 30);
            var thisYearsRaces = listOfRaces.Where(r => r.Date >= startOfYear);
            foreach(var race in thisYearsRaces)
            {
                RaceListService.Models.EventRaceTimesVM ert = new RaceListService.Models.EventRaceTimesVM();
                ert.RaceId = race.EFKey;
                ert.RaceDistance = db.distances.SingleOrDefault(d => d.Code == race.Event.DistanceCode).Name;
                var distance = db.distances.SingleOrDefault(d => d.Code == race.Event.DistanceCode).Value;
                ert.RaceTitle = race.Event.Title;

                int x = race.Actual ?? 0;
                int y = race.Target ?? 0;

                ert.RaceActualTime = RaceListService.Models.EventRaceTimesVM.formatResult(x);
                ert.TargetTime = y;
                ert.RaceTargetTime = RaceListService.Models.EventRaceTimesVM.formatResult(y);
                int td = 0;
                if (x != 0)
                {
                    td = y - x;
                }
                if (td > 0)
                { ert.TimeDifference = (y - x).ToString(); }
                else
                { ert.TimeDifference = 0.ToString(); }


                if (race.Date != null)
                {
                    ert.RaceDate = (DateTime)race.Date;
                    VM.Add(ert);
                }

            }

            return VM;
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
