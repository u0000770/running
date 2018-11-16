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
    public class RunnerServiceController : ApiController
    {
        /// <summary>
        /// db is a Model of the underlying data
        /// </summary>
        private RunningModelEntities db = new RunningModelEntities();

        /// <summary>
        /// when http://rundistance.azurewebsites.net/api/RunnerService is called
        /// </summary>
        /// <returns>A List of JSON objects each with the RunnerDTO shape</returns>
        public IEnumerable<Models.RunnerDTO> Get()
        {

            var allRunners = db.runners.Where(r => r.Active == true).OrderBy(n => n.secondname);
            
            IEnumerable<Models.RunnerDTO> dto = Models.RunnerDTO.BuildListofRunners(allRunners);

            return dto;
        }



        /// <summary>
        /// when http://rundistance.azurewebsites.net/api/RunnerService/id is called
        /// </summary>
        /// <param name="id">Key value for a specific Runner</param>
        /// <returns>A single DTO that describes the last race for that runner</returns>
        public Models.LastRaceDTO GetLastRace(int id)
        { 
            var thisRace = db.LastRaces.FirstOrDefault(r => r.RunnerId == id);
            Models.LastRaceDTO dto = new Models.LastRaceDTO();
            dto.RunnerId = thisRace.RunnerId;
            dto.RunnerName = thisRace.runner.secondname + " " + thisRace.runner.firstname;
            dto.LastTime = formatResult(thisRace.Time);
            dto.LastDistance = db.distances.Single(d => d.Value == thisRace.Distance).Name;
            dto.date = thisRace.Date.ToShortDateString();
            return dto;
        }

        public string formatResult(int result)
        {
            string resultString = "No Result";
            if (result > 0)
            {
                TimeSpan t = TimeSpan.FromSeconds(result);
                resultString = string.Format("{0:D2}h:{1:D2}m:{2:D2}s",
                                t.Hours,
                                t.Minutes,
                                t.Seconds);
            }
            return resultString;
        }
    }
}
