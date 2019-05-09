using RunningModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RaceListService.Controllers
{
    public class TrophyController : Controller
    {
        private RunningModelEntities db = new RunningModelEntities();
        private bool isLongRace(EventRunnerTime race)
        {
            var distance = db.distances.SingleOrDefault(d => d.Code == race.Event.DistanceCode).Value;
            if (distance > 10000 && race.Actual != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        // GET: Trophy
        public ActionResult Index()
        {
            //if (IsAdmin())
            //{
            ViewBag.Admin = true;
            var all = db.runners.Where(r => r.Active == true && r.EventRunnerTimes.Count > 0);
            List<RaceListService.Models.jrListItemVM> jrlist = new List<RaceListService.Models.jrListItemVM>();
            List<RaceListService.Models.jrListItemVM> rblist = new List<RaceListService.Models.jrListItemVM>();
            foreach (var runner in all)
            {
                RaceListService.Models.jrListItemVM vmrunner = new RaceListService.Models.jrListItemVM();
                vmrunner.RunnerId = runner.EFKey;
                vmrunner.Name = runner.firstname + " " + runner.secondname;
                var listOfRace = db.EventRunnerTimes.Where(r => r.RunnerId == runner.EFKey);
              //  var valid
                var top5Races = GetTopFive(listOfRace);
                vmrunner.timeDiff = 0;
                vmrunner.jrPoints = 0;
                vmrunner.races = 0;
                vmrunner.JR = false;
                foreach (var race in listOfRace)
                {
                    bool longrace = isLongRace(race);
                    if (longrace) { vmrunner.JR = true; }
                    int actual = race.Actual ?? 0;
                    int target = race.Target ?? 0;
                    if (actual != 0)
                    {
                        // increment number of races completed.
                        vmrunner.races = vmrunner.races + 1;
                        // if your beat your target
                        if ((target - actual) > 0)
                        {
                            //// You get 10 Points for this race !
                            vmrunner.jrPoints = vmrunner.jrPoints + 10;
                            // cap the time difference to 2 min
                            if (target - actual > 120)
                            {
                                vmrunner.timeDiff = vmrunner.timeDiff + 120;

                            }
                            else
                            {
                                vmrunner.timeDiff = vmrunner.timeDiff + (target - actual);
                            }

                        }
                        else // you completed the race but did not beat your target
                        {
                            vmrunner.jrPoints = vmrunner.jrPoints + 6;
                        }
                    }



                }




                if (vmrunner.jrPoints >= 50)
                {
                    vmrunner.jrPoints = 50;
                }

                jrlist.Add(vmrunner);

            }

            var vm = jrlist.OrderByDescending(s => s.timeDiff).OrderByDescending(r => r.jrPoints);
            return View(vm);
            //}
            //else
            //{
            //    return RedirectToAction("Index", "Default");
            //}
        }

        private IEnumerable<EventRunnerTime> GetTopFive(IQueryable<EventRunnerTime> listOfRace)
        {
            
             //   int actual = r.Actual ?? 0;
             //   int target = r.Target ?? 0;
                List<EventRunnerTime> list = listOfRace.Where(r => r.Actual != null && r.Target != null)
                                     .OrderByDescending(x => x.Target - x.Actual)
                                     .ToList();

            return list.Take(5);
        }

        private IEnumerable<EventRunnerTime> WithResult(IQueryable<EventRunnerTime> listOfRace)
        {

            //   int actual = r.Actual ?? 0;
            //   int target = r.Target ?? 0;
            List<EventRunnerTime> list = listOfRace.Where(r => r.Actual != null && r.Target != null).ToList();
                       

            return list;
        }
    }
}