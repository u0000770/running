using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace TestWCFClient.Controllers
{

    public class HomeController : Controller
    {
        

        private string BASE_URL = "http://localhost:51993/Service1.svc/";
        public ActionResult All()
        {
                var webclient = new WebClient();
                var json = webclient.DownloadString(BASE_URL + "runners");
                var js = new JavaScriptSerializer();
                var dto =  js.Deserialize<List<Models.RunnerDTO>>(json);
                return View(dto);
        }
            


            public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}