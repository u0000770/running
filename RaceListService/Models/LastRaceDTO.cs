using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using RunningModel;

namespace RaceListService.Models
{
    public class RaceItemDTO
    {
        public string Title { get; set; } 
        public double Distance { get; set; }
    }


    public class RunnerLastRaceDTO
    {
        public int RaceTime { get; set; }
        public double Distance { get; set; }
    }
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
        public string RunnerUKAN { get; set; }

        public string RunnerName { get; set; }

        internal static IEnumerable<RunnerDTO> BuildListofRunners(List<runner> allRunners)
        {

            var dto = allRunners.Select(r => new RunnerDTO
            {
                RunnerUKAN = r.ukan,
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