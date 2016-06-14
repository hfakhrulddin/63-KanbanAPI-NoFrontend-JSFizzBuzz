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
using Kanban_API;
using Kanban_API.Models;
using AutoMapper;

namespace Kanban_API.Controllers
{
    public class ListsController : ApiController
    {
        private FilmsEntities db = new FilmsEntities();
        // GET: api/Lists
//Below is the default code.
        ////public IQueryable<List> GetLists()
        ////{
        ////    return db.Lists;
        ////}
        ///public IQueryable<ListModel> GetLists()  
        // BElow code is has been chaged to use the auto mapper.
        public IEnumerable<ListModel> GetLists()
        {
            return Mapper.Map<IEnumerable<ListModel>>(db.Lists);

//Below is the way to map manually.
            //return db.Lists.Select(l => new ListModel
            //{
            //    ListId = l.ListId,
            //    Name = l.Name,
            //    CreatedDate = l.CreatedDate,
            //    UserId = l.UserId
            //});
        }
        // GET: api/Lists/5
        [ResponseType(typeof(ListModel))]
        public IHttpActionResult GetList(int id)
        {
            List list = db.Lists.Find(id);
            if (list == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<ListModel>(list));
        }

        //GET: api/List/5/Card
        [Route("api/Lists/{listID}/Cards")]
        public IEnumerable<CardModel> GetCardsforList(int listID)
        {
            var cards = db.Cards.Where(c => c.ListID == listID);
            return Mapper.Map<IEnumerable<CardModel>>(cards);
        }
        // PUT: api/Lists/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutList(int id, ListModel list)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != list.ListId)
            {
                return BadRequest();
            }
            #region this need to be chagne

            //////////////////Below is the update part
            var dblist = db.Lists.Find(id);
            dblist.Update(list);
            db.Entry(dblist).State = EntityState.Modified;
            ////////////////////////////////////////////////////////////////
            //db.Entry(list).State = EntityState.Modified; ///this was the original code not a DTOs 
            #endregion
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ListExists(id))
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

        // POST: api/Lists
        [ResponseType(typeof(ListModel))]
        public IHttpActionResult PostList(ListModel list)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //////////the New code
            var dblist = new List(list);

           list.CreatedDate =  dblist.CreatedDate; //those tow lines to show the user the old values with updated dates.
           list.ListId = dblist.ListId;

            db.Lists.Add(dblist);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = list.ListId }, list);
        }
        // DELETE: api/Lists/5
        [ResponseType(typeof(ListModel))]
        public IHttpActionResult DeleteList(int id)
        {
            List list = db.Lists.Find(id);
            if (list == null)
            {
                return NotFound();
            }

            db.Lists.Remove(list);
            db.SaveChanges();

            return Ok(Mapper.Map<ListModel>(list));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ListExists(int id)
        {
            return db.Lists.Count(e => e.ListId == id) > 0;
        }
    }
}