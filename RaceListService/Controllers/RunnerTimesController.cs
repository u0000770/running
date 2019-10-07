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
    public class RunnerTimesController : Controller
    {
        private RunningModelEntities db = new RunningModelEntities();

        // GET: RunnerTimes
        public ActionResult Index(int id)
        {
            DateTime yearAgo = new DateTime(2019, 1, 1);
            // var all = db.EventRunnerTimes.Where(r => r.RunnerId == id);
            var all = db.EventRunnerTimes.Where(r => r.RunnerId == id && r.Date > yearAgo && (r.Actual != null || r.Actual > 0) ).OrderByDescending(r => r.Date);
            // var handicapRaces = filterHandicap(all);
            List<EventRunnerTime> handicapRaces = new List<EventRunnerTime>();
            foreach(var r in all)
            {
                if ( r.Active == null)
                {
                    r.Active = true;
                }
                handicapRaces.Add(r);
            }

            runnerTimeListVM vm = new runnerTimeListVM();
            vm.runnerId = id;
            vm.listOfRaces = runnerTimeListVM.buildVM(handicapRaces,db).ToList();

           

            vm.SelectedRaceIDs = runnerTimeListVM.buildSelectedList(vm.listOfRaces);

            ViewBag.median = runnerTimeItemVM.formatResult((int)GetMedian(vm));
            ViewBag.average = runnerTimeItemVM.formatResult((int)GetAverage(vm));
            ViewBag.last = runnerTimeItemVM.formatResult((int)GetLast(vm));
            var runner = db.runners.Find(id);
            ViewBag.name = runner.secondname + " " + runner.firstname;
            vm.listOfRaces.OrderBy(r => r.RaceDate).OrderBy(r => r.PredictedTimeValue);
            return View(vm);
        }

        private IEnumerable<EventRunnerTime> filterHandicap(IQueryable<EventRunnerTime> all)
        {
            List<EventRunnerTime> outList = new List<EventRunnerTime>();
            foreach(var r in all)
            {
                if (isHandiCapRace(r))
                {
                    outList.Add(r);
                }
            }
            return outList;
        }

        // GET: RunnerTimes/Details/5
        public ActionResult UpdateSelection(runnerTimeListVM vm)
        {
            var all = db.EventRunnerTimes.Where(r => r.RunnerId == vm.runnerId);
            foreach (var r in all)
            {
                var race = db.EventRunnerTimes.Find(r.EFKey);
                race.Active = false;
                db.Entry(race).State = EntityState.Modified;

            }
            db.SaveChanges();
            foreach (var r in vm.SelectedRaceIDs)
            {
                var race = db.EventRunnerTimes.Find(r);
                race.Active = true;
                db.Entry(race).State = EntityState.Modified;

            }
            db.SaveChanges();
            return RedirectToAction("Index", new { id = vm.runnerId });




        }


        public decimal GetLast(runnerTimeListVM vm)
        {
            var sorted = vm.listOfRaces.Where(r => r.Active).OrderBy(r => r.RaceDate);
            return sorted.Last().PredictedTimeValue;
        }


        public decimal GetAverage(runnerTimeListVM  vm)
        {
            List<int> tempArray = new List<int>();
            decimal total = 0;
            foreach (var m in vm.listOfRaces.Where(r => r.Active))
            {
                tempArray.Add(m.PredictedTimeValue);
                total = m.PredictedTimeValue + total;
            }

            int count = tempArray.Count;

            return total / count;

        }


        public decimal GetMedian(runnerTimeListVM vm)
        {

            List<int> tempArray = new List<int>();

            foreach (var m in vm.listOfRaces.Where(r => r.Active))
            {
                tempArray.Add(m.PredictedTimeValue);
            }


            int count = tempArray.Count;

            tempArray.Sort((x, y) => y.CompareTo(x));

            decimal medianValue = 0;
            if  (count == 3)
            {
                var sum = tempArray.Sum();
                medianValue = sum / 3;
                return medianValue;
            }
            if (count > 0 && count % 2 == 0)
            {
                // count is even, need to get the middle two elements, add them together, then divide by 2
                int middleElement1 = tempArray[(count / 2) - 1];
                int middleElement2 = tempArray[(count / 2)];
                // new line 
                int middleElement3 = tempArray[(count / 2)] + 1;
                medianValue = (middleElement1 + middleElement2 + middleElement3) / 3;
            }
            else
            {
                // count is odd, simply get the middle element.
                int middleElement1 = tempArray[(count / 2) - 1];
                int middleElement2 = tempArray[(count / 2)];
                // new line 
                int middleElement3 = tempArray[(count / 2)] + 1;
                medianValue = (middleElement1 + middleElement2 + middleElement3) / 3;
            }

            return medianValue;
        }


        private bool isHandiCapRace(EventRunnerTime race)
        {
            var distance = db.distances.SingleOrDefault(d => d.Code == race.Event.DistanceCode).Value;
            if (distance <= 10000 && race.Actual != null)
            {
                return true;
            }
            else
            {
                return false;
            }
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
