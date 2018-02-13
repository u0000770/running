using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RaceListService.Controllers
{
    public class BatchUploadController : Controller
    {

        private RunningModel.RunningModelEntities db = new RunningModel.RunningModelEntities();
        // GET: BatchUpload
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult UpLoad()
        {
         

            var fred = TempData["path"].ToString();
            //var filesData = Directory.GetFiles(@fred);
            string path = Server.MapPath("~/App_Data/" + fred);
            //string path = Server.MapPath(fred.ToString());
            var package = new OfficeOpenXml.ExcelPackage(new FileInfo(path));

            ExcelWorksheet workSheet = package.Workbook.Worksheets[1];

            var allrunners = db.runners.Where(r => r.Active == true).ToList();

            for (int row = workSheet.Dimension.Start.Row;
                     row <= workSheet.Dimension.End.Row;
                     row++)
            {
                AddRunnerToDb(workSheet, row, allrunners);
               // ReadRowData(workSheet, row, runnderID);
            }

            ViewBag.Title = "Home Page";

            return View();
        }

        private static string GetFirstName(ExcelWorksheet workSheet, int row)
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

        private static string GetSecondName(ExcelWorksheet workSheet, int row)
        {
            if (workSheet.Cells[row, workSheet.Dimension.Start.Column + 1].Value != null)
            {
                return workSheet.Cells[row, workSheet.Dimension.Start.Column + 1].Value.ToString();
            }
            else
            {
                return null;
            }
        }

        //private static string GetDistance(ExcelWorksheet workSheet, int row)
        //{
        //    if (workSheet.Cells[row, workSheet.Dimension.Start.Column + 2].Value != null)
        //    {
        //        return workSheet.Cells[row, workSheet.Dimension.Start.Column + 2].Value.ToString();
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        private static double GetDistance(ExcelWorksheet workSheet, int row)
        {
            if (workSheet.Cells[row, workSheet.Dimension.Start.Column + 2].Value != null)
            {
                return  Convert.ToDouble(workSheet.Cells[row, workSheet.Dimension.Start.Column + 2].Value);
            }
            else
            {
                return 0; 
            }
        }

        private static int GetTime(ExcelWorksheet workSheet, int row)
        {
            if (workSheet.Cells[row, workSheet.Dimension.Start.Column + 3].Value != null)
            {
                var fraction = Convert.ToDouble(workSheet.Cells[row, workSheet.Dimension.Start.Column + 3].Value);
                var seconds = 86400 * fraction;
                return Convert.ToInt32(seconds);
            }
            else
            {
                return 0;
            }
        }


        public ActionResult GetFile(HttpPostedFileBase file)
        {
           

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

        private void AddRunnerToDb(ExcelWorksheet workSheet, int row, IEnumerable<RunningModel.runner> Allrunners)
        {
            var fname = GetFirstName(workSheet, row);
            var sname = GetSecondName(workSheet, row);
            var distance = GetDistance(workSheet, row);
            var racetime = GetTime(workSheet, row);

            var result = from x in Allrunners
                         where x.firstname.Contains(fname) && x.secondname.Contains(sname)
                         select x;

            if ( result.Count() == 0)
            {
                RunningModel.runner thisRunner = AddRunner(fname, sname);
                if (distance > 0)
                { 
                  AddRace(distance, racetime, thisRunner);
                }

            }
            else
            {
                var ourRunner = result.First();
                var lastRace = db.runners.Find(ourRunner.EFKey).LastRaces.OrderBy(r => r.Date).FirstOrDefault();
                if (lastRace != null)
                { 
                    if (distance > 0)
                    {
                        EditLastRace(distance, racetime, lastRace);
                    }
                }
                else
                {
                    if (distance > 0)
                    {
                        AddRace(distance, racetime, ourRunner);
                    }
                }
            }

        }

        private void EditLastRace(double distance, int racetime, RunningModel.LastRace lastRace)
        {
            lastRace.Date = DateTime.Now;
            lastRace.Distance = distance;
            lastRace.Time = racetime;
            db.Entry(lastRace).State = EntityState.Modified;
            db.SaveChanges();
        }

        private RunningModel.runner AddRunner(string fname, string sname)
        {
            RunningModel.runner thisRunner = new RunningModel.runner();
            thisRunner.firstname = fname;
            thisRunner.secondname = sname;
            thisRunner.Active = true;
            db.runners.Add(thisRunner);
            db.SaveChanges();
            return thisRunner;
        }

        private void AddRace(double distance, int racetime, RunningModel.runner thisRunner)
        {
            RunningModel.LastRace newRace = new RunningModel.LastRace();
            newRace.Date = DateTime.Now;
            newRace.Time = racetime;
            newRace.Distance = distance;
            newRace.RunnerId = thisRunner.EFKey;
            db.LastRaces.Add(newRace);
            db.SaveChanges();
        }
    }



   


   

}