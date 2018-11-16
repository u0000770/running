using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using System.IO;
using ClassLibrary1;
using RaceListService.Models;
using System.Data.Entity;
using RunningModel;

namespace RaceListService.Controllers
{
    public class HomeController : Controller
    {

        private Model1 db = new Model1();
       // private RunningModelEntities newdb = new RunningModelEntities();

        public ActionResult NewTarget(string racedetails)
        {
            try
            {
                var valid = (bool)Session["login"];
                if (!valid)
                {
                    return RedirectToAction("Index", "Default");
                }
            }
            catch
            {
                return RedirectToAction("Index", "Default");
            }
            TempData["targetdistance"] = racedetails;
            
            return RedirectToAction("ListRunners");
        }


        /// present the upload file form
        public ActionResult Index()
        {
            try
            {
                var valid = (bool)Session["login"];
                if (!valid)
                {
                    return RedirectToAction("Index", "Default");
                }
            }
            catch
            {
                return RedirectToAction("Index", "Default");
            }
            ViewBag.Title = "Home Page";

            return View("UpLoad");
        }


        /// <summary>
        /// Get the file from the form and save it to the appdata folder
        /// </summary>
        /// <param name="file"></param>
        /// <returns>Call to UpLoad Method using tempdata path as a pointer to the file</returns>
        public ActionResult GetFile(HttpPostedFileBase file)
        {
            try
            {
                var valid = (bool)Session["login"];
                if (!valid)
                {
                    return RedirectToAction("Index", "Default");
                }
            }
            catch
            {
                return RedirectToAction("Index", "Default");
            }

            if (file != null && file.ContentLength > 0)
            {
                // extract only the filename
                var fileName = Path.GetFileName(file.FileName);
                string name = System.IO.Path.GetFileName(fileName);
                // store the file inside ~/App_Data/uploads folder
                var path = Path.Combine(Server.MapPath("~/App_Data/"), name);
                file.SaveAs(path);
                TempData["path"] = name;
            }

            ViewBag.Title = "Home Page";

            return RedirectToAction("UpLoad");
        }


        /// <summary>
        /// Displays the list of runners showing the current target time.
        /// It also sets the dropdown list of all potential distances
        /// </summary>
        /// <returns></returns>
        public ActionResult ListRunners()
        {

            //try
            //{
            //    var valid = (bool)Session["login"];
            //    if (!valid)
            //    {
            //        return RedirectToAction("Index", "Default");
            //    }
            //}
            //catch
            //{
            //    return RedirectToAction("Index", "Default");
            //}

            // get all runners with result time
            var AllRunners = db.memberLists;
            var targetdistanceMeters = Convert.ToDouble(TempData["targetdistance"]);
            var targetdistanceName = RaceDetails.GetByRaceNameByMeters(targetdistanceMeters);
            ViewBag.targetdistance = targetdistanceName;
            ViewBag.Title = "Home Page";
            RaceDetails racedetails = new RaceDetails();
            ViewBag.racedetails = racedetails.buildSelectList(targetdistanceMeters);
            IEnumerable<ClubMemberListItemVM> vm = ClubMemberListItemVM.buildVM(AllRunners);
            TempData["targetdistance"] = targetdistanceMeters;
            return View(vm);
        }

        //public ActionResult Details(int id, RaceListService.Models.memberDetailVM rxvm)
        //{
        //    //try
        //    //{
        //    //    var valid = (bool)Session["login"];
        //    //    if (!valid)
        //    //    {
        //    //        return RedirectToAction("Index", "Default");
        //    //    }
        //    //}
        //    //catch
        //    //{
        //    //    return RedirectToAction("Index", "Default");
        //    //}
        //    // get runner
        //    var ThisRunner = db.memberLists.Find(id);

        //    ViewBag.Title = "Cup Times";
        //    var targetDistance = TempData["targetdistance"];
        //    // create vm
        //    memberDetailVM vm = new memberDetailVM();
        //    // give vm runner name
        //    vm.Name = ThisRunner.Name;
        //    vm.currentTarget = RaceTimesVM.formatResult(ThisRunner.Time);
        //    // get ALL thier races
        //    IEnumerable<ClassLibrary1.race> listofraces = ThisRunner.races;
        //    // create an empty list to display and populate with details
        //    vm.listOfRaces = new List<RaceTimesVM>();
        //    vm.listOfRaces = memberDetailVM.buildList(listofraces, Convert.ToDouble(targetDistance));


        //    List<RaceTimesVM> copy = new List<RaceTimesVM>();
        //    copy.AddRange(vm.listOfRaces);
        //    vm.simpleAverage = memberDetailVM.CalcSimpleAve(vm.listOfRaces);
        //    vm.listOfRaces = copy.OrderBy(r => r.predictedTime).ToList();
        //    vm.SelectedRaceIDs = memberDetailVM.buildSelectedList(vm.listOfRaces);

        //    // non selected
        //    if (rxvm.SelectedRaceIDs != null && rxvm.SelectedRaceIDs.Count > 0)
        //    {

        //        List<RaceTimesVM> selectedRaceVM = new List<RaceTimesVM>();
        //        List<race> selectedRaces = new List<race>();
        //        foreach (var r in listofraces)
        //        {
        //            if (rxvm.SelectedRaceIDs.Any(s => s == r.Id))
        //            {
        //                selectedRaces.Add(r);
        //            }

        //        }
        //        vm.selectedRaces = memberDetailVM.buildList(selectedRaces, Convert.ToDouble(targetDistance));
        //        vm.SelectedRaceIDs = memberDetailVM.buildSelectedList(vm.selectedRaces);
        //        vm.topAndtailAverage = memberDetailVM.CalcSimpleAve(vm.selectedRaces);



        //        string newtime = vm.topAndtailAverage;

        //        int seconds = UpdateTime(newtime);

        //        ThisRunner.Time = seconds;
        //        ThisRunner.Distance = Convert.ToDouble(targetDistance);
        //        vm.SelectedRaceIDs = rxvm.SelectedRaceIDs;
        //        vm.currentTarget = RaceTimesVM.formatResult(seconds);

        //    }
        //    else
        //    {
        //        // none selected
        //        string newtime = vm.simpleAverage;
        //        int seconds = UpdateTime(newtime);
        //        ThisRunner.Time = seconds;

        //    }
            
        //    ViewBag.targetdistance = RaceDetails.GetByRaceNameByMeters(Convert.ToDouble(TempData["targetdistance"]));
        //    db.Entry(ThisRunner).State = EntityState.Modified;
        //    db.SaveChanges();
        //    TempData["targetdistance"] = targetDistance;
        //    return View(vm);
        //   // return RedirectToAction("ListRunners");

        //}

        private static int UpdateTime(string newtime)
        {
            string output = new string(newtime.Where(c => (Char.IsDigit(c) || c == '.' || c == ':')).ToArray());
            TimeSpan ts = TimeSpan.Parse(output);
            double totalSeconds = ts.TotalSeconds;
            int seconds = Convert.ToInt32(totalSeconds);
            return seconds;
        }

        /// <summary>
        /// Uploads the data in the spreadsheet into the database
        /// The file location is read from tempdata path
        /// </summary>
        /// <returns></returns>
        //public ActionResult UpLoad()
        //{
        //    try
        //    {
        //        var valid = (bool)Session["login"];
        //        if (!valid)
        //        {
        //            return RedirectToAction("Index", "Default");
        //        }
        //    }
        //    catch
        //    {
        //        return RedirectToAction("Index", "Default");
        //    }

        //    var fred = TempData["path"].ToString();
        //    //var filesData = Directory.GetFiles(@fred);
        //    string path = Server.MapPath("~/App_Data/" + fred);
        //    //string path = Server.MapPath(fred.ToString());
        //    var package = new OfficeOpenXml.ExcelPackage(new FileInfo(path));

        //    ExcelWorksheet workSheet = package.Workbook.Worksheets[1];
        //    CleanDataBase();

        //    for (int row = workSheet.Dimension.Start.Row;
        //             row <= workSheet.Dimension.End.Row;
        //             row++)
        //    {
        //        var runnderID = AddRunnerToDb(workSheet, row);
        //        ReadRowData(workSheet, row, runnderID);
        //    }

        //    ViewBag.Title = "Home Page";

        //    return View();
        //}

        private void CleanDataBase()
        {
            db.races.RemoveRange(db.races);
            db.memberLists.RemoveRange(db.memberLists);
        }

        //private int AddRunnerToDb(ExcelWorksheet workSheet, int row)
        //{
        //    memberList memberList = new memberList();
        //    memberList.Name = GetName(workSheet, row);
        //    memberList.Time = 0;
        //    memberList.Distance = 8046.72;
        //    db.memberLists.Add(memberList);
        //    db.SaveChanges();
        //    return memberList.Id;
        //}

        private double calcPredictedTime(double d1, double d2, int t1)
        {
            double con = 1.06;
            //t2 = t1 * (d2 / d1) ^ 1.06
            // d3 = (d2 / d1)
            double d3 = d2 / d1;
            // w = d3^ 1.06
            double w = Math.Pow(d3, con);
            // t2 = t1 * w
            double t2 = t1 * w;
            return t2;
        }

        private double cameron(double old_dist, double new_distance, double old_time)
        {
            //a = 13.49681 - (0.000030363 * old_dist) + (835.7114 / (old_dist ^ 0.7905))
            //b = 13.49681 - (0.000030363 * new_dist) + (835.7114 / (new_dist ^ 0.7905))
            //new_time = (old_time / old_dist) * (a / b) * new_dist

            // x = (0.000030363 * old_dist)
            double x = (0.000030363 * old_dist);
            // y = (old_dist ^ 0.7905)
            double y = Math.Pow(old_dist, 0.7905);
            // z = (835.7114 / y )
            double z = 835.7114 / y;
            double a = 13.49681 - x + z;

            double w = Math.Pow(new_distance, 0.7905);
            double v = 835.7114 / w;
            double t = 0.000030363 * new_distance;


            double b = 13.49681 - t + v;

            double new_time = (old_time / old_dist) * (a / b) * new_distance;
            return new_time;

        }

        private static string GetName(ExcelWorksheet workSheet, int row)
        {
            if (workSheet.Cells[row, workSheet.Dimension.Start.Column].Value != null)
            {
                return workSheet.Cells[row, workSheet.Dimension.Start.Column].Value.ToString();
            }
            else
            {
                return null;
            }
        }

        //private static void ReadRowData(ExcelWorksheet workSheet, int row, int runnerId)
        //{

        //    Model1 db = new Model1();
        //    for (int col = workSheet.Dimension.Start.Column + 1;
        //                             col <= workSheet.Dimension.End.Column;
        //                             col = col + 2){
        //        object cellValue = workSheet.Cells[row, col].Value;
        //        if (workSheet.Cells[row, col].Value != null)
        //        {
        //            AddResult(workSheet, row,col, runnerId, db);
        //        }
        //    }

        //}

        //private static void AddResult(ExcelWorksheet workSheet, int row,int col, int runnerId, Model1 db)
        //{
        //    // var d = RaceDetails.GetMetersByName(workSheet.Cells[row, col].Value.ToString());
        //    var d = RaceDetails.GetMetersByCode(workSheet.Cells[row, col].Value.ToString());
        //    var distance = workSheet.Cells[row, col].Value;
        //    var time = workSheet.Cells[row, col + 1].Value.ToString();

            

        //    string output = new string(time.Where(c => (Char.IsDigit(c) || c == '.' || c == ':')).ToArray());

        //    string SubString = output.Substring(output.Length - 8);
        //    TimeSpan ts = TimeSpan.Parse(SubString);
        //    double totalSeconds = ts.TotalSeconds;
        //    int seconds = Convert.ToInt32(totalSeconds);

        //    ClassLibrary1.race Arace = new race();
        //    Arace.runner = runnerId;
        //    Arace.distance = d;
        //    Arace.time = seconds;

        //    db.races.Add(Arace);
        //    db.SaveChanges();
        //}
    }
}

