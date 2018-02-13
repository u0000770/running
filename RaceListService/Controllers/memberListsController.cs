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
using raceListModel;

namespace RaceListService.Controllers
{
    public class memberListsController : ApiController
    {
        private raceListModel.ModelRaceList db = new raceListModel.ModelRaceList();

        // GET: api/memberLists
        public IQueryable<raceListModel.memberList> GetmemberLists()
        {
            return db.memberLists;
        }

        // GET: api/memberLists/5
        [ResponseType(typeof(raceListModel.memberList))]
        public IHttpActionResult GetmemberList(int id)
        {
            raceListModel.memberList memberList = db.memberLists.Find(id);
            if (memberList == null)
            {
                return NotFound();
            }

            return Ok(memberList);
        }

        // PUT: api/memberLists/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutmemberList(int id, raceListModel.memberList memberList)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != memberList.Id)
            {
                return BadRequest();
            }

            db.Entry(memberList).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!memberListExists(id))
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

        // POST: api/memberLists
        [ResponseType(typeof(raceListModel.memberList))]
        public IHttpActionResult PostmemberList(raceListModel.memberList memberList)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.memberLists.Add(memberList);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = memberList.Id }, memberList);
        }

        // DELETE: api/memberLists/5
        [ResponseType(typeof(raceListModel.memberList))]
        public IHttpActionResult DeletememberList(int id)
        {
            raceListModel.memberList memberList = db.memberLists.Find(id);
            if (memberList == null)
            {
                return NotFound();
            }

            db.memberLists.Remove(memberList);
            db.SaveChanges();

            return Ok(memberList);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool memberListExists(int id)
        {
            return db.memberLists.Count(e => e.Id == id) > 0;
        }
    }
}