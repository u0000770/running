using RaceListService.Models;
using RunningModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RaceListService.Controllers
{
    public class HandicapController : Controller
    {
        private RunningModelEntities db = new RunningModelEntities();

        private bool isShortRace(EventRunnerTime race)
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

        /// <summary>
        /// Builds list of races from EventRunnerTimes for this runner
        /// </summary>
        /// <param name="thisRunner"></param>
        /// <returns>List<EventRaceTimesVM></returns>
        private List<EventRaceTimesVM> BuildEventRaceList(runner thisRunner)
        {
            List<EventRaceTimesVM> listOfRacesVM = new List<EventRaceTimesVM>();
            var listOfRace = db.EventRunnerTimes.Where(r => r.RunnerId == thisRunner.EFKey);
            foreach (var race in listOfRace)
            {
                if (isShortRace(race))
                { 
                    EventRaceTimesVM ert = new EventRaceTimesVM();
                    ert.RaceId = race.EFKey;
                    ert.RaceDistance = db.distances.SingleOrDefault(d => d.Code == race.Event.DistanceCode).Name;
                    ert.RaceTitle = race.Event.Title;

                    int x = race.Actual ?? 0;
                    ert.RaceActualTime = EventRaceTimesVM.formatResult(x);
                    int y = race.Target ?? 0;
                    ert.RaceTargetTime = EventRaceTimesVM.formatResult(y);
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
                        listOfRacesVM.Add(ert);
                    }
                }

            }

            return listOfRacesVM;
        }


        private void BuildmemberRaceDetailVM(int? id, memberRaceDetailVM vm)
        {
            vm.RunnerId = Convert.ToInt32(id);
            var lastRace = db.LastRaces.SingleOrDefault(r => r.RunnerId == vm.RunnerId);
            if (lastRace != null)
            {
                vm.lastRaceId = lastRace.RunnerId;
                vm.lastRaceTime = EventRaceTimesVM.formatResult(lastRace.Time);
                vm.lastRaceDistance = db.distances.Single(d => d.Value == lastRace.Distance).Name;
                vm.lastRaceDate = lastRace.Date.Date;
            }
        }

        public ActionResult Details(int id)
        {
            runner thisRunner = db.runners.Find(id);
            if (thisRunner == null)
            {
                return HttpNotFound();
            }
            var vm = new memberRaceDetailVM();
            BuildmemberRaceDetailVM(id, vm);

            List<EventRaceTimesVM> listOfRacesVM = BuildEventRaceList(thisRunner);
            var total = 0;
            foreach (EventRaceTimesVM ev in listOfRacesVM)
            {
                total = total + Convert.ToInt32(ev.TimeDifference);
            }
            vm.listOfRaces = listOfRacesVM.OrderBy(r => r.RaceDate).ToList();



            return View(vm);
        }

        
        // GET: Handicap
        public ActionResult Index()
        {

            List<RaceListService.Models.HCListItemVM> vm = new List<Models.HCListItemVM>();
            var all = db.runners.Where(r => r.Active == true && r.EventRunnerTimes.Count > 0);
           
            foreach (var runner in all)
            {
                RaceListService.Models.HCListItemVM vmrunner = new RaceListService.Models.HCListItemVM();
                vmrunner.RunnerId = runner.EFKey;
                vmrunner.RunnerName = runner.firstname + " " + runner.secondname;
                var listOfRace = db.EventRunnerTimes.Where(r => r.RunnerId == runner.EFKey);
                List<EventRunnerTime> shortRaces = new List<EventRunnerTime>();
                foreach (var race in listOfRace)
                {
                    if (isShortRace(race))
                    {
                        shortRaces.Add(race);
                    }
                }
                int shortRaceCount = shortRaces.Count();
                if (shortRaceCount >= 3)
                {
                    vmrunner.RunnerId = runner.EFKey;
                    vmrunner.RunnerName = runner.secondname + " " + runner.firstname;
                    vmrunner.numberofRaces = shortRaceCount;
                    vm.Add(vmrunner);
                }

                

            }

            var nvm = vm.OrderByDescending(x => x.numberofRaces);
            return View(nvm);

        }
    }
}