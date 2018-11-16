using RunningModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WCFRest
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IRunner
    {
        public string HelloWorld()
        {
            return "Hello World";
        }


        private RunningModelEntities db = new RunningModelEntities();

        internal IEnumerable<Models.RunnerDTO> BuildListofRunners(List<runner> allRunners)
        {

            var dto = allRunners.Select(r => new Models.RunnerDTO
            {
                RunnerId = r.EFKey,
                RunnerName = r.secondname + " " + r.firstname,
            }).AsEnumerable();

            return dto;

        }


        public List<Models.RunnerDTO> GetAll()
        {
            var all = db.runners.Where(r => r.Active == true).ToList();
            var dto = BuildListofRunners(all);
            return dto.ToList();
        }


        public RunningModel.runner GetById(string id)
        {
            return null;
        }

        public bool create(RunningModel.runner runner)
        {
            return true;
        }



        public bool edit(RunningModel.runner runner)
        {
          return true;
        }

   public bool delete(string id)
    {
        return true;
    }



    }
}
