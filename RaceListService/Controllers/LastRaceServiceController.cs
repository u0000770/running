using RaceListService.Models;
using RunningModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace RaceListService.Controllers
{

    [EnableCors("*", "*", "*")]
    public class LastRaceServiceController : ApiController
    {

        private RunningModelEntities db = new RunningModelEntities();
        // GET: api/LastRaceService
        public IEnumerable<LastRaceDTO> Get()
        {

            
            var allRunners = db.runners.Where(r => r.Active == true).OrderBy(n => n.secondname);
            List<LastRaceDTO> dto = new List<LastRaceDTO>();
            // construct the viewmodel - list of Last Races 
            BuildListofLastRaces(allRunners, dto);

            return dto;
        }

        private void BuildListofLastRaces(IOrderedQueryable<runner> allRunners, List<LastRaceDTO> dto)
        {
           
                foreach (var m in allRunners)
                {
                    var nr = new LastRaceDTO();
                    var last = db.LastRaces.SingleOrDefault(l => l.RunnerId == m.EFKey);
                    if (last != null)
                    {
                        nr.RunnerId = m.EFKey;
                        nr.LastDistance = db.distances.SingleOrDefault(d => d.Value == last.Distance).Name;
                        nr.LastTime = RaceCalc.formatTime(last.Time);
                        nr.RunnerName = m.secondname + " " + m.firstname;
                        //nr.Time = RaceCalc.formatTime(last.Time);
                        nr.date = last.Date.ToShortDateString();
                        dto.Add(nr);
                    }
                }
            }


        // GET: api/LastRaceService/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/LastRaceService
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/LastRaceService/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/LastRaceService/5
        public void Delete(int id)
        {
        }
    }
}
