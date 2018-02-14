using ClassLibrary1;
using RaceListService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RaceListService.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Default
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetIn()
        {
            var un = Request.Form["un"];
            var pw = Request.Form["pw"];
            if (un == "503141" && pw == "827191")
            {
                Session["login"] = true;
                TempData["targetdistance"] = 10000;
                return RedirectToAction("Index", "NextRaces");
            }
            else
            {
                Session["login"] = false;
                return RedirectToAction("Index", "Default");
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