using RunningModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace RunnersWcfSL
{
    public class Service1 : IRunnerService
    {

        private RunningModelEntities db = new RunningModelEntities();

        internal  IEnumerable<RunnerDTO> BuildListofRunners(List<runner> allRunners)
        {

            var dto = allRunners.Select(r => new RunnerDTO
            {
                RunnerId = r.EFKey,
                RunnerName = r.secondname + " " + r.firstname,
            }).AsEnumerable();

            return dto;

        }

        public List<RunnerDTO> GetAll()
        {
            var all = db.runners.Where(r => r.Active == true).ToList();
            var dto = BuildListofRunners(all);
            return dto.ToList();
        }

       
        public RunnerDTO GetById(int id)
        {
            
            RunningModel.runner runner = db.runners.Find(id);
            if (runner == null || runner.Active == false)
            {
                return null;
            }
            else
            {
                RunnerDTO dto = new RunnerDTO();
                dto.RunnerId = runner.EFKey;
                dto.RunnerName = runner.secondname + " " + runner.firstname;
                return dto;
            }
        }

       
        public bool Add(string firstname,string secondname)
        {
            if (string.IsNullOrEmpty(firstname) || string.IsNullOrEmpty(secondname))
            {
                return false;
            }

            try
            {
                var newRunner = db.runners.Create();

                newRunner.firstname = firstname;
                newRunner.secondname = secondname;
                newRunner.ukan = "";
                newRunner.ageGradeCode = "";
                newRunner.dob = null;
                newRunner.email = "";
                newRunner.Active = true;
                db.runners.Add(newRunner);
                db.SaveChanges();

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

       
        public bool Delete(int id)
        {
            runner runner = db.runners.Find(id);
            if (runner == null)
            {
                return false;
            }
            else
            {
                try
                {
                    runner.Active = false;
                    db.Entry(runner).State = EntityState.Modified;
                    db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

        }

      
        public bool Update(RunnerDTO runner)
        {
            runner upRunner = db.runners.SingleOrDefault(r => r.EFKey == runner.RunnerId);
            if (upRunner == null)
            {
                return false;
            }
            else
            {
                try
                {
                    string[] names = runner.RunnerName.Split(' ');
                    upRunner.firstname = names[1];
                    upRunner.secondname = names[0];
                    db.Entry(upRunner).State = EntityState.Modified;
                    db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
    }
}
