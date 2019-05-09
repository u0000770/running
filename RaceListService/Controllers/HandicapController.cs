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
    public class HandicapController : Controller
    {
        private RunningModelEntities db = new RunningModelEntities();

      

       
        //public class HCmemberRaceDetailVM
        //{

        //    public bool admin { get; set; }
        //    public int RunnerId { get; set; }
        //    [Display(Name = "Name")]
        //    public string Name { get; set; }
        //    [Display(Name = "Median")]
        //    public string medianTime { get; set; }

        //    [Display(Name = "Average")]
        //    public string averageTime { get; set; }

        //    [Display(Name = "Last Race Prediction")]
        //    public string lastRaceTime { get; set; }

        //    [Display(Name = "Recommended Predicted")]
        //    public string recTime { get; set; }

        //    public List<HandicapRaceTimesVM> selectedRaces { get; set; }
        //    public List<HandicapRaceTimesVM> listOfRaces { get; set; }
        //    public List<int> SelectedRaceIDs { get; set; }

        //    public static List<int> buildSelectedList(IEnumerable<HandicapRaceTimesVM> listofraces)
        //    {
        //        List<int> list = new List<int>();
        //        foreach (var r in listofraces)
        //            if (r.Active)
        //            {
        //                list.Add(r.RaceId);
        //            }
        //        return list;

        //    }

        //}

        //public class HandicapRaceTimesVM
        //{
        //    public int RunnerId { get; set; }
        //    public int RaceId { get; set; }
        //    [Display(Name = "Event Race Distance")]
        //    public string RaceDistance { get; set; }
        //    [Display(Name = "Event Race Title")]
        //    public string RaceTitle { get; set; }
        //    [Display(Name = "Event Race Target")]
        //    public string RaceTargetTime { get; set; }
        //    [Display(Name = "Event Race Actual Time")]
        //    public string RaceActualTime { get; set; }
        //    [Display(Name = "Event Race Date")]
        //    [DataType(DataType.Date)]
        //    public DateTime RaceDate { get; set; }
        //    [Display(Name = "Time Difference")]
        //    public string TimeDifference { get; set; }
        //    [Display(Name = "HC Prediction")]
        //    public string Predicted { get; set; }
        //    public int PredictedTimeValue { get; set; }
        //    public bool Active { get; set; }
        //    public bool IsSelected { get; set; }


        //    public static string formatResult(int result)
        //    {
        //        string resultString = "No Result";
        //        if (result > 0)
        //        {

        //            TimeSpan t = TimeSpan.FromSeconds(result);

        //            resultString = string.Format("{0:D2}h:{1:D2}m:{2:D2}s",
        //                            t.Hours,
        //                            t.Minutes,
        //                            t.Seconds);

        //        }

        //        return resultString;
        //    }

        //}

        ///// <summary>
        ///// Builds list of races from EventRunnerTimes for this runner
        ///// </summary>
        ///// <param name="thisRunner"></param>
        ///// <returns>List<EventRaceTimesVM></returns>
        //private List<HandicapRaceTimesVM> BuildEventRaceList(runner thisRunner)
        //{
        //    List<HandicapRaceTimesVM> listOfRacesVM = new List<HandicapRaceTimesVM>();
        //    var listOfRace = db.EventRunnerTimes.Where(r => r.RunnerId == thisRunner.EFKey);
        //    foreach (var race in listOfRace)
        //    {
        //        if (isHandiCapRace(race))
        //        {
                    
        //            HandicapRaceTimesVM ert = new HandicapRaceTimesVM();
        //            ert.RunnerId = thisRunner.EFKey;
        //            if (race.Active == null)
        //            {
        //                ert.Active = false;
        //            }
        //            if(race.Active == true)
        //            {
        //                ert.Active = true;
        //            }
        //            if (race.Active == false)
        //            {
        //                ert.Active = false;
        //            }
                    
        //            ert.RaceId = race.EFKey;
        //            ert.RaceDistance = db.distances.SingleOrDefault(d => d.Code == race.Event.DistanceCode).Name;
        //            ert.RaceTitle = race.Event.Title;

        //            int x = race.Actual ?? 0;
        //            ert.RaceActualTime = HandicapRaceTimesVM.formatResult(x);
        //            int y = race.Target ?? 0;
        //            ert.RaceTargetTime = HandicapRaceTimesVM.formatResult(y);
        //            string code = race.Event.DistanceCode;
        //            double distance = db.distances.SingleOrDefault(d => d.Code == code).Value;
        //            double predicted = CalculateHCPredicion(distance, x);
        //            ert.PredictedTimeValue = (int)predicted;
        //            ert.Predicted = HandicapRaceTimesVM.formatResult((int)predicted);
        //            int td = 0;
        //            if (x != 0)
        //            {
        //                td = y - x;
        //            }
        //            if (td > 0)
        //            { ert.TimeDifference = (y - x).ToString(); }
        //            else
        //            { ert.TimeDifference = 0.ToString(); }


        //            if (race.Date != null)
        //            {
        //                ert.RaceDate = (DateTime)race.Date;
        //                listOfRacesVM.Add(ert);
        //            }
        //        }

        //    }

        //    return listOfRacesVM;
        //}


        //private void BuildmemberRaceDetailVM(int? id, HCmemberRaceDetailVM vm)
        //{
        //    vm.RunnerId = Convert.ToInt32(id);
        //    //var lastRace = db.LastRaces.SingleOrDefault(r => r.RunnerId == vm.RunnerId);
        //    //if (lastRace != null)
        //    //{
        //    //    vm.lastRaceId = lastRace.RunnerId;
        //    //    vm.lastRaceTime = HandicapRaceTimesVM.formatResult(lastRace.Time);
        //    //    vm.lastRaceDistance = db.distances.Single(d => d.Value == lastRace.Distance).Name;
        //    //    vm.lastRaceDate = lastRace.Date.Date;
        //    //}
        //}


        //public static List<int> buildSelectedList(IEnumerable<HandicapRaceTimesVM> listofraces)
        //{
        //    List<int> list = new List<int>();
        //    foreach (var r in listofraces)
        //        if (r.Active)
        //        {
        //            list.Add(r.RaceId);
        //        }
        //    return list;

        //}

        //public ActionResult Details(int id)
        //{
        //    runner thisRunner = db.runners.Find(id);
           
        //    if (thisRunner == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    var vm = new HCmemberRaceDetailVM();

        //    BuildmemberRaceDetailVM(id, vm);

        //    List<HandicapRaceTimesVM> listOfRacesVM = BuildEventRaceList(thisRunner);


        //    HCmemberRaceDetailVM.buildSelectedList(listOfRacesVM);

        //    List<int> selectedidlist = buildSelectedList(listOfRacesVM);
        //    vm.SelectedRaceIDs = selectedidlist;

        //    vm.selectedRaces = BuildSelectedRaceList(listOfRacesVM);

        //    var total = 0;
        //    foreach (HandicapRaceTimesVM ev in listOfRacesVM)
        //    {
        //        total = total + Convert.ToInt32(ev.TimeDifference);
        //    }
        //    vm.listOfRaces = listOfRacesVM.OrderBy(r => r.Predicted).ToList();

        //    var average = GetAverage(listOfRacesVM);

        //    var median = GetMedian(listOfRacesVM);

        //    var last = GetLast(listOfRacesVM);

        //    vm.lastRaceTime = HandicapRaceTimesVM.formatResult((int)last);

        //    vm.averageTime = HandicapRaceTimesVM.formatResult((int)average);

        //    vm.medianTime = HandicapRaceTimesVM.formatResult((int)median);

        //    var recTime = (median + last) / 2;

        //    vm.recTime = HandicapRaceTimesVM.formatResult((int)recTime);

        //    vm.Name = thisRunner.secondname + " " + thisRunner.firstname;


        //    foreach (var x in vm.selectedRaces.Where(a => vm.SelectedRaceIDs.Contains(a.RaceId)))
        //    {
        //        x.IsSelected = true;
        //    }


        //    return View(listOfRacesVM);
        //}

        //private List<HandicapRaceTimesVM> BuildSelectedRaceList(List<HandicapRaceTimesVM> listOfRacesVM)
        //{
        //    List<HandicapRaceTimesVM> outlist = new List<HandicapRaceTimesVM>();
        //    foreach (var r in listOfRacesVM)
        //    {
        //        if (r.Active)
        //        {
        //            outlist.Add(r);
        //        }
        //    }
        //    return outlist;
        //}

        //[HttpPost]
        //public ActionResult UpdateSelection(IEnumerable<RaceListService.Controllers.HandicapController.HandicapRaceTimesVM> vm)
        //{

        //    var all = db.EventRunnerTimes.Where(r => r.RunnerId == vm.First().RunnerId);
        //    foreach (var r in all)
        //    {
        //        var race = db.EventRunnerTimes.Find(r.EFKey);
        //        race.Active = false;
        //        db.Entry(race).State = EntityState.Modified;

        //    }
        //    db.SaveChanges();
        //    foreach (var r in vm)
        //    {
        //        if (r.IsSelected)
        //        { 
        //            var race = db.EventRunnerTimes.Find(r.RaceId);
                
        //            race.Active = true;
        //            db.Entry(race).State = EntityState.Modified;
        //        }

        //    }
        //    db.SaveChanges();
        //    return RedirectToAction("Details", new { id = vm.First().RunnerId} );


        //}

        //public decimal GetLast(List<HandicapRaceTimesVM> vm)
        //{
        //    var sorted = vm.OrderBy(r => r.RaceDate);
        //    return sorted.Last().PredictedTimeValue;

        //}


        //public decimal GetAverage(List<HandicapRaceTimesVM> vm)
        //{
        //    List<int> tempArray = new List<int>();
        //    decimal total = 0;
        //    foreach (var m in vm)
        //    {
        //        tempArray.Add(m.PredictedTimeValue);
        //        total = m.PredictedTimeValue + total;
        //    }

        //    int count = tempArray.Count;

        //    return total / count;

        //}


        //public decimal GetMedian(List<HandicapRaceTimesVM> vm)
        //{

        //    List<int> tempArray = new List<int>();

        //    foreach (var m in vm)
        //    {
        //        tempArray.Add(m.PredictedTimeValue);
        //    }

            
        //    int count = tempArray.Count;

        //    tempArray.Sort((x, y) => y.CompareTo(x));

        //    decimal medianValue = 0;

        //    if (count % 2 == 0)
        //    {
        //        // count is even, need to get the middle two elements, add them together, then divide by 2
        //        int middleElement1 = tempArray[(count / 2) - 1];
        //        int middleElement2 = tempArray[(count / 2)];
        //        medianValue = (middleElement1 + middleElement2) / 2;
        //    }
        //    else
        //    {
        //        // count is odd, simply get the middle element.
        //        medianValue = tempArray[(count / 2)];
        //    }

        //    return medianValue;
        //}


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
                    if (isHandiCapRace(race))
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