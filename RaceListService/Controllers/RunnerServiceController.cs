using RunningModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;

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
        /// 
        [RequireHttps]
        public IEnumerable<Models.RunnerDTO> Get()
        {

            var allRunners = db.runners.Where(r => r.Active == true).OrderBy(n => n.secondname).ToList();
            
            IEnumerable<Models.RunnerDTO> dto = Models.RunnerDTO.BuildListofRunners(allRunners);

            return dto;
        }








        /// <summary>
        /// when http://rundistance.azurewebsites.net/api/RunnerService/id is called
        /// </summary>
        /// <param name="id">Key value for a specific Runner</param>
        /// <returns>A single DTO that describes the last race for that runner</returns>
        [RequireHttps]
        public Models.RunnerRaceDetailDTO GetLastRace(string ukan)
        {
            Models.RunnerRaceDetailDTO dto = new Models.RunnerRaceDetailDTO();
            var distance = db.distances;
            var thisRunner = db.runners.SingleOrDefault(r => r.ukan == ukan);
            if (thisRunner != null)
            {
                var listOfRace = db.EventRunnerTimes.Where(r => r.RunnerId == thisRunner.EFKey && r.Actual != null);
                var lastThreeRaces = listOfRace.OrderByDescending(r => r.Date);
                dto.ukaNumber = thisRunner.ukan;
                dto.Name = thisRunner.firstname + " " + thisRunner.secondname;
                
                foreach(var r in lastThreeRaces)
                {
                    Models.EventRaceTimesDTO ert = new Models.EventRaceTimesDTO();
                    ert.RaceId = r.EFKey;
                    ert.RaceActualTime = formatResult((int)r.Actual);
                    ert.RaceTitle = r.Event.Title;
                    ert.RaceDate = r.Date.Value.ToShortDateString();
                    ert.RaceDistance = distance.SingleOrDefault(d => d.Code == r.Event.DistanceCode).Name;
                    if (r.Target != null )
                    {
                        ert.RaceTargetTime = formatResult((int)r.Target);
                    }
                    else
                    {
                        ert.RaceTargetTime = "non set";
                    }
                    dto.listOfRaces.Add(ert);
                }
                
            }

             
            
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
