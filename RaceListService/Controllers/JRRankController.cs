using RaceListService.Models;
using RunningModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RaceListService.Controllers
{
    public class JRRankController : Controller
    {

        private RunningModelEntities db = new RunningModelEntities();
        // GET: JRRank

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


        public ActionResult Index()
        {
            //if (IsAdmin())
            //{
            ViewBag.Admin = true;
            DateTime startDate = new DateTime(2018, 12, 30);
            var all = db.runners.Where(r => r.Active == true && r.EventRunnerTimes.Count > 0);

            List<RaceListService.Models.jrListItemVM> jrlist = new List<RaceListService.Models.jrListItemVM>();
            List<RaceListService.Models.jrListItemVM> dblist = new List<RaceListService.Models.jrListItemVM>();
            List<runner> jrAllList = new List<runner>();
            List<runner> dbALList = new List<runner>();
            ///
            ///split all into db and jr lists
            SplitList(all, jrAllList, dbALList);

            BuildJRList(jrlist, jrAllList);
            BuildDBList(dblist, dbALList);


            var vmjr = jrlist.Where(r => r.jrPoints > 1).OrderByDescending(s => s.timeDiff).OrderByDescending(s => s.jrPoints);
            var vmdb = dblist.Where(r => r.jrPoints > 1).OrderByDescending(s => s.timeDiff).OrderByDescending(s => s.jrPoints);

            var vm = new TrophyVM();
            vm.dbTrophyList = vmdb.ToList();
            vm.jrTrophyList = vmjr.ToList();
            return View(vm);
            //}
            //else
            //{
            //    return RedirectToAction("Index", "Default");
            //}
        }

        private void SplitList(IQueryable<runner> all, List<runner> jrAllList, List<runner> dbALList)
        {
            DateTime start = new DateTime(2018, 12, 31);
            foreach (var runner in all)
            {
                var listOfRace = db.EventRunnerTimes.Where(r => r.RunnerId == runner.EFKey && r.Date > start);
                var isLong = false;
                foreach (var race in listOfRace)
                {
                    if (isLongRace(race))
                    {
                        isLong = true;
                        break;
                    }
                }
                if (isLong)
                {
                    jrAllList.Add(runner);
                }
                else
                {
                    dbALList.Add(runner);
                }

            }
        }

        private void BuildDBList(List<Models.jrListItemVM> dblist, List<runner> SplitList)
        {
            DateTime start = new DateTime(2018, 12, 31);
            foreach (var runner in SplitList)
            {

                RaceListService.Models.jrListItemVM vmrunner = new RaceListService.Models.jrListItemVM();
                vmrunner.RunnerId = runner.EFKey;
                vmrunner.Name = runner.firstname + " " + runner.secondname;
                var listOfRace = db.EventRunnerTimes.Where(r => r.RunnerId == runner.EFKey && r.Date > start);
                var top5Races = GetTopFive(listOfRace);
                vmrunner.timeDiff = 0;
                vmrunner.jrPoints = 0;
                vmrunner.races = 0;
                vmrunner.JR = false;
                foreach (var race in top5Races)
                {
                    // bool longrace = isLongRace(race);
                    // if (longrace) { vmrunner.JR = true; }
                    int actual = race.Actual ?? 0;
                    int target = race.Target ?? 0;
                    if (actual != 0)
                    {
                        vmrunner.races = vmrunner.races + 1;
                        if ((target - actual) > 0)
                        {
                            vmrunner.jrPoints = vmrunner.jrPoints + 10;
                            if (target - actual > 120)
                            {
                                vmrunner.timeDiff = vmrunner.timeDiff + 120;

                            }
                            else
                            {
                                vmrunner.timeDiff = vmrunner.timeDiff + (target - actual);
                            }

                        }
                        else
                        {
                            vmrunner.jrPoints = vmrunner.jrPoints + 6;
                        }
                    }
                }
                if (vmrunner.jrPoints >= 50)
                {
                    vmrunner.jrPoints = 50;
                }
                dblist.Add(vmrunner);
            }
        }


        private void BuildJRList(List<Models.jrListItemVM> jrlist, List<runner> SplitList)
        {
            DateTime start = new DateTime(2018, 12, 31);
            foreach (var runner in SplitList)
            {
                RaceListService.Models.jrListItemVM vmrunner = new RaceListService.Models.jrListItemVM();
                vmrunner.RunnerId = runner.EFKey;
                vmrunner.Name = runner.firstname + " " + runner.secondname;
                var listOfRace = db.EventRunnerTimes.Where(r => r.RunnerId == runner.EFKey && r.Date > start);
                var top5Races = GetTopFive(listOfRace);
                vmrunner.timeDiff = 0;
                vmrunner.jrPoints = 0;
                vmrunner.races = 0;
                vmrunner.JR = false;
                foreach (var race in top5Races)
                {
                   // bool longrace = isLongRace(race);
                   // if (longrace) { vmrunner.JR = true; }
                    int actual = race.Actual ?? 0;
                    int target = race.Target ?? 0;
                    if (actual != 0)
                    {
                        vmrunner.races = vmrunner.races + 1;
                        if ((target - actual) > 0)
                        {
                            vmrunner.jrPoints = vmrunner.jrPoints + 10;
                            if (target - actual > 120)
                            {
                                vmrunner.timeDiff = vmrunner.timeDiff + 120;

                            }
                            else
                            {
                                vmrunner.timeDiff = vmrunner.timeDiff + (target - actual);
                            }

                        }
                        else
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

        private bool isLongRace(EventRunnerTime race)
        {
            var distance = db.distances.SingleOrDefault(d => d.Code == race.Event.DistanceCode).Value;
            if (distance > 10000  && race.Actual != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

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
    }

  
}