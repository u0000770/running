using RaceListService.Models;
using RunningModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RaceListService.Controllers
{
    public class RaceResultController : Controller
    {

        private RunningModelEntities db = new RunningModelEntities();

        private List<SelectListItem> buildSelectList(IEnumerable<RaceEvent> elist)
        {

            List<SelectListItem> slist = new List<SelectListItem>();

            foreach (var item in elist)
            {
                var option = new SelectListItem()
                {
                    Text = item.Event.Title,
                    Value = item.EventId.ToString()
                };
                slist.Add(option);
            }
            return slist;

        }


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

        public ActionResult ListResult(string elistValue)
        {
            var eventid = Convert.ToInt32(elistValue);
            var RaceEventList = db.RaceEvents.OrderBy(r => r.Date);
            RaceEvent thisEvent = RaceEventList.FirstOrDefault(r => r.EventId == eventid);
            var DCode = thisEvent.Event.DistanceCode;
            var distance = db.distances.SingleOrDefault(d => d.Code == DCode).Value;
            var allEventRunnerTimes = db.EventRunnerTimes.Where(r => r.EventId == thisEvent.EventId && thisEvent.Date == r.Date);
            List<RaceResultItemVM> VM = buildResultList(allEventRunnerTimes);
            var elist = db.RaceEvents.Where(d => d.Date < DateTime.Now);
            var list = buildSelectList(elist);
            ViewBag.elistValue = new SelectList(list, "Value", "Text");
            ViewBag.Admin = true;

            return View("Index",VM);
        }

        private List<RaceResultItemVM> buildResultList(IQueryable<EventRunnerTime> allEventRunnerTimes)
        {
            List<RaceResultItemVM> list = new List<RaceResultItemVM>();
            foreach (EventRunnerTime ert in allEventRunnerTimes)
            {
                RaceResultItemVM item = new RaceResultItemVM();
                if ((ert.Actual != null && ert.Actual > 0) && (ert.Target != null && ert.Target > 0))
                {
                    var runner = db.runners.Find(ert.RunnerId);
                    item.RunnerName = runner.firstname + " " + runner.secondname;
                    item.actualTime = RaceResultItemVM.formatResult((int)ert.Actual);
                    item.predictedTime = RaceResultItemVM.formatResult((int)ert.Target);
                    var trophyItem = TrophyData(ert,item);
                    item.differenceTime = trophyItem.differenceTime;
                    item.trophyPoints = trophyItem.trophyPoints;
                    item.trophyTime = trophyItem.trophyTime;
                    list.Add(item);
                }
            }
            return list.OrderByDescending(r => r.differenceTime).ToList();
        }

        private RaceResultItemVM TrophyData(EventRunnerTime ert, RaceResultItemVM item)
        {
            int actual = ert.Actual ?? 0;
            int target = ert.Target ?? 0;
            item.differenceTime = target - actual;
            if (actual != 0)
            {
                if ((item.differenceTime) > 0)
                {
                    item.trophyPoints = 10;
                    if (item.differenceTime > 120)
                    {
                        item.trophyTime = "120";

                    }
                    else
                    {
                        item.trophyTime = item.differenceTime.ToString();
                    }

                }
                else
                {
                    item.trophyPoints = 6;
                }


            }
            return item;
        }

            private int CalculatePoints(int differenceTime)
        {
                if (differenceTime > 0)
                {
                    return 10;
                }
                else
                {
                    return  6;
                }
            }

        // GET: RaceResult
        public ActionResult Index()
        {
            var admin = IsAdmin();

            if (true)
            {
                // var elist = db.RaceEvents.Where(d => d.Date >= DateTime.Now).OrderBy(r => r.Date).Take(4);
                var elist = db.RaceEvents.Where(d => d.Date < DateTime.Now);
                var list = buildSelectList(elist);
                ViewBag.elistValue = new SelectList(list, "Value", "Text");
                ViewBag.Admin = true;
                return View();
            }
            //else
            //{
            //    return RedirectToAction("Index", "Default");
            //}
        }
    }
}