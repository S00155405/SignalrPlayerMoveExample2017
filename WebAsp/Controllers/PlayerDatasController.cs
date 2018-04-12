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
using CommonDataItems;
using WebAsp.Models;

namespace WebAsp.Controllers
{
    public class PlayerDatasController : ApiController
    {
        private WebAspContext db = new WebAspContext();

        // GET: api/PlayerDatas
        public IQueryable<PlayerData> GetPlayerDatas()
        {

            return db.PlayerDatas;
        }

        // GET: api/PlayerDatas/5
        [ResponseType(typeof(PlayerData))]
        public IHttpActionResult GetPlayerData(string name)
        {
            return Ok( db.PlayerDatas.First());
            //if (db.PlayerDatas.Any(pd => pd.PlayerName == name))
            //{
            //    PlayerData playerData = db.PlayerDatas.Where(pd => pd.PlayerName == name).First();
               
            //    if (playerData == null)
            //    {
            //        return NotFound();
            //    }

            //    return Ok(playerData);
            //}
            //else
            //{
            //    return NotFound();
            //}
        }

        // PUT: api/PlayerDatas/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPlayerData(string id, PlayerData playerData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != playerData.ID)
            {
                return BadRequest();
            }

            db.Entry(playerData).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlayerDataExists(id))
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

        // POST: api/PlayerDatas
        [ResponseType(typeof(PlayerData))]
        public IHttpActionResult PostPlayerData(PlayerData playerData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PlayerDatas.Add(playerData);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (PlayerDataExists(playerData.ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = playerData.ID }, playerData);
        }

        // DELETE: api/PlayerDatas/5
        [ResponseType(typeof(PlayerData))]
        public IHttpActionResult DeletePlayerData(string id)
        {
            PlayerData playerData = db.PlayerDatas.Find(id);
            if (playerData == null)
            {
                return NotFound();
            }

            db.PlayerDatas.Remove(playerData);
            db.SaveChanges();

            return Ok(playerData);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PlayerDataExists(string id)
        {
            return db.PlayerDatas.Count(e => e.ID == id) > 0;
        }
    }
}