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
                var all = db.runners.Where(r => r.Active == true && r.EventRunnerTimes.Count > 0);
                List<RaceListService.Models.jrListItemVM> jrlist = new List<RaceListService.Models.jrListItemVM>();
                foreach (var runner in all)
                {
                    RaceListService.Models.jrListItemVM vmrunner = new RaceListService.Models.jrListItemVM();
                    vmrunner.RunnerId = runner.EFKey;
                    vmrunner.Name = runner.firstname + " " + runner.secondname;
                    var listOfRace = db.EventRunnerTimes.Where(r => r.RunnerId == runner.EFKey);
                    vmrunner.timeDiff = 0;
                    vmrunner.jrPoints = 0;
                    vmrunner.races = 0;
                    foreach (var race in listOfRace)
                    {

                        int actual = race.Actual ?? 0;
                        int target = race.Target ?? 0;
                        if (actual != 0)
                        {
                            vmrunner.races = vmrunner.races + 1;
                            if ((target - actual) > 0)
                            {
                                vmrunner.timeDiff = vmrunner.timeDiff + (target - actual);
                                vmrunner.jrPoints = vmrunner.jrPoints + 10;
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

                var vm = jrlist.OrderByDescending(s => s.timeDiff).OrderByDescending(r => r.jrPoints);
                return View(vm);
            //}
            //else
            //{
            //    return RedirectToAction("Index", "Default");
            //}
        }
    }

  
}