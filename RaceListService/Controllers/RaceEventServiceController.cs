using RaceListService.Models;
using RunningModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;

namespace RaceListService.Controllers
{

    [EnableCors("*", "*", "*")]
    public class RaceEventServiceController : ApiController
    {
        private RunningModelEntities db = new RunningModelEntities();
        // GET: api/LastRaceService
        [RequireHttps]
        public IEnumerable<RaceItemDTO> Get()
        {


            var allRaces = db.RaceEvents;
            List<RaceItemDTO> dto = new List<RaceItemDTO>();
            // construct the viewmodel - list of Last Races 
            dto = BuildListofRaces(allRaces);

            return dto;
        }

        [RequireHttps]
        public Models.RunnerLastRaceDTO GetLastRace(string ukan)
        {
            Models.RunnerLastRaceDTO dto = new Models.RunnerLastRaceDTO();
            var distance = db.distances;
            var thisRunner = db.runners.SingleOrDefault(r => r.ukan == ukan);
            if (thisRunner != null)
            {
                var listOfRace = db.EventRunnerTimes.Where(r => r.RunnerId == thisRunner.EFKey && r.Actual != null);
                var lastRace = listOfRace.OrderByDescending(r => r.Date).First();
                dto.RaceTime = (int)lastRace.Actual;
                dto.Distance = distance.SingleOrDefault(s => s.Code == lastRace.Event.DistanceCode).Value;

                return dto;

            }



            return null;
        }


       
        //[RequireHttps]

        //public RaceItemDTO GetLast()
        //{
        //    var lastrace = db.RaceEvents.OrderBy(r => r.Date).First();
        //    RaceItemDTO dto = new RaceItemDTO();
        //    // construct the viewmodel - list of Last Races 
        //    dto.Distance = db.distances.SingleOrDefault(d => d.Code == lastrace.Event.DistanceCode).Value;
        //    dto.Title = lastrace.Event.Title;

        //    return dto;
        //}

        private List<RaceItemDTO> BuildListofRaces(DbSet<RaceEvent> allRaces)
        {
            List<RaceItemDTO> dto = new List<RaceItemDTO>();
            var distance = db.distances;
            var now = DateTime.Now;
            foreach (var r in allRaces)
            {
                if (r.Date >= now)
                {
                    RaceItemDTO thisRace = new RaceItemDTO();
                    thisRace.Title = r.Event.Title;
                    thisRace.Distance = distance.SingleOrDefault(d => d.Code == r.Event.DistanceCode).Value;
                    dto.Add(thisRace);
                }
            }
            return dto;

        }
    }
}
