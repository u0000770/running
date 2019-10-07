using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RaceListService.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public Models.FactorObject Get()
        {
            var result = calc_fwd();
            return result;
        }


        private Models.FactorObject calc_fwd() // lookup factor and calculate age-grading etc.
        {
            TimeSpan raceTime = new TimeSpan(0, 25, 58);
            //alert("calc_fwd");
            // event_list is global in scope, but is only set by the list callbacks
            //  if (event_list==null) {alert("No event specified!");return;} // lookup_factor now does this
            var calc = new Models.Wava();
            Models.FactorObject factorObject = new Models.FactorObject();
            factorObject =  calc.lookup_factor(true, "5kmRoad", 59);
            if (factorObject.factor == 0.0) return null; // failure returns fac=0.0

           double result = raceTime.TotalSeconds; // convert from hh:mm:ss
           factorObject.age_graded_result = result * factorObject.factor; // age-graded result
         
            if (factorObject.age_graded_standard > 0)
            {

                    factorObject.age_percentage = 100 * factorObject.age_graded_standard / result; // time events
              
            }
            return factorObject;
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
