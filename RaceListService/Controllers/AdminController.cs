using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RaceListService.Controllers
{
    public class AdminController : Controller
    {

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
        // GET: Admin
        public ActionResult Index()
        {
            if(IsAdmin())
            { 
               return View();
            }
            else
            {
                return RedirectToAction("Index", "Default");
            }
        }
    }
}