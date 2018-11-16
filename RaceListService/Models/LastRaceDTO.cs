using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using RunningModel;

namespace RaceListService.Models
{
    public class LastRaceDTO
    {
        public int RunnerId { get; set; }

        public string RunnerName { get; set; }

       // public string Time { get; set; }

        public string LastDistance { get; set; }

        public string LastTime { get; set; }
        public string date { get; set; }

    }


    public class RunnerDTO
    {
        public int RunnerId { get; set; }

        public string RunnerName { get; set; }

        internal static IEnumerable<RunnerDTO> BuildListofRunners(IOrderedQueryable<runner> allRunners)
        {

            var dto = allRunners.Select(r => new RunnerDTO
            {
                 RunnerId = r.EFKey,
                 RunnerName = r.secondname + " " + r.firstname,
            }).AsEnumerable();

            return dto;

        }
    }

    //public static Expression<Func<RunningModel.runner, RunnerDTO>> AsRunnerDTO =
    //        p => new AsRunnerDTO
    //        {
    //             p.firstname =  
    //        };


}