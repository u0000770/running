using ClassLibrary1;
using RaceListService.Models;
using RunningModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RaceListService.Controllers
{
    public class DefaultController : Controller
    {

        private RunningModelEntities db = new RunningModelEntities();
        // GET: Default
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetIn()
        {
            var un = Request.Form["un"];
            var pw = Request.Form["pw"];
            var ukan = Request.Form["ukan"];
            if (un == "503141" && pw == "827191")
            {
                Session["admin"] = true;
                TempData["targetdistance"] = 10000;
                return RedirectToAction("Index", "NextRaces");
            }
            else
            {
                if (ukan != null)
                {
                    var runner = db.runners.SingleOrDefault(r => r.ukan == ukan);
                    if (runner != null)
                    {
                        Session["admin"] = false;
                        return RedirectToAction("Details", "NextRaces", new { id = runner.EFKey });
                    }
                    else
                    {
                        return RedirectToAction("Index", "Default");
                    }
                    // /NextRaces/Details/192
                }
                else
                {
                    Session["admin"] = false;
                    return RedirectToAction("Index", "Default");
                }
            }
            
           



        }

        public ActionResult ListTargets()
        {
            // 
            ClassLibrary1.Model1 db = new ClassLibrary1.Model1();
            var AllRunners = db.memberLists;
            var targetdistanceMeters = Convert.ToDouble(TempData["targetdistance"]);
            var targetdistanceName = RaceDetails.GetByRaceNameByMeters(targetdistanceMeters);
            IEnumerable<ClubMemberListItemVM> vm = ClubMemberListItemVM.buildVM(AllRunners);
            return View(vm);
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Default");

        }
    }
}