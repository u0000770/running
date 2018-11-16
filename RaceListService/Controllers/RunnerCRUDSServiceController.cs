using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using RunningModel;

namespace RaceListService.Controllers
{
    public class RunnerCRUDSServiceController : ApiController
    {
        private RunningModelEntities db = new RunningModelEntities();

        // GET: api/RunnerCRUDSService
        public IQueryable<runner> Getrunners()
        {
            return db.runners;
        }

        // GET: api/RunnerCRUDSService/5
        [ResponseType(typeof(runner))]
        public IHttpActionResult Getrunner(int id)
        {
            runner runner = db.runners.Find(id);
            if (runner == null)
            {
                return NotFound();
            }

            return Ok(runner);
        }

        // PUT: api/RunnerCRUDSService/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putrunner(int id, runner runner)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != runner.EFKey)
            {
                return BadRequest();
            }

            db.Entry(runner).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!runnerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/RunnerCRUDSService
        [ResponseType(typeof(runner))]
        public IHttpActionResult Postrunner(runner runner)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.runners.Add(runner);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = runner.EFKey }, runner);
        }

        // DELETE: api/RunnerCRUDSService/5
        [ResponseType(typeof(runner))]
        public IHttpActionResult Deleterunner(int id)
        {
            runner runner = db.runners.Find(id);
            if (runner == null)
            {
                return NotFound();
            }

            db.runners.Remove(runner);
            db.SaveChanges();

            return Ok(runner);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool runnerExists(int id)
        {
            return db.runners.Count(e => e.EFKey == id) > 0;
        }
    }
}