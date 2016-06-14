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
    public class CardsController : ApiController
{
    private FilmsEntities db = new FilmsEntities();
    // GET: api/Cards
    //public IQueryable<Card> GetCards()
    //{
    //    return db.Cards;
    //}
    public IEnumerable<CardModel> GetCards()   
    {
        return Mapper.Map<IEnumerable<CardModel>>(db.Cards);
    }
    // GET: api/Cards/5
    [ResponseType(typeof(CardModel))]
    public IHttpActionResult GetCard(int id)
    {
        Card card = db.Cards.Find(id);
        if (card == null)
        {
            return NotFound();
        }

        return Ok(Mapper.Map<CardModel>(card));
    }
    // PUT: api/Cards/5
    [ResponseType(typeof(void))]
    public IHttpActionResult PutCard(int id, CardModel card)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (id != card.CardId)
        {
            return BadRequest();
        }
        #region this need to be chagne

        //////////////////Below is the update part
        var dbcard = db.Cards.Find(id);
        dbcard.Update(card);
        db.Entry(dbcard).State = EntityState.Modified;
        #endregion

        //db.Entry(card).State = EntityState.Modified; Original Code.

        try
        {
            db.SaveChanges();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CardExists(id))
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
        // POST: api/Cards
        [ResponseType(typeof(CardModel))]
        public IHttpActionResult PostCard(CardModel card)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //////////the New code
            var dbcard = new Card(card);

            card.CreatDate = dbcard.CreatDate;
            card.ListID = dbcard.ListID;

            db.Cards.Add(dbcard);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = card.CardId }, card);
        }
        // DELETE: api/Cards/5
        [ResponseType(typeof(CardModel))]
    public IHttpActionResult DeleteCard(int id)
    {
        Card card = db.Cards.Find(id);
        if (card == null)
        {
            return NotFound();
        }

        db.Cards.Remove(card);
        db.SaveChanges();

        return Ok(Mapper.Map<CardModel>(card));
    }
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            db.Dispose();
        }
        base.Dispose(disposing);
    }
    private bool CardExists(int id)
    {
        return db.Cards.Count(e => e.CardId == id) > 0;
    }
}
}