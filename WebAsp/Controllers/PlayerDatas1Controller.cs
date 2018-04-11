using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CommonDataItems;
using WebAsp.Models;

namespace WebAsp.Controllers
{
    public class PlayerDatas1Controller : Controller
    {
        private WebAspContext db = new WebAspContext();

        // GET: PlayerDatas1
        public ActionResult Index()
        {
            return View(db.PlayerDatas.ToList());
        }

        // GET: PlayerDatas1/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlayerData playerData = db.PlayerDatas.Find(id);
            if (playerData == null)
            {
                return HttpNotFound();
            }
            return View(playerData);
        }

        // GET: PlayerDatas1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PlayerDatas1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,PlayerName,XP,Password")] PlayerData playerData)
        {
            if (ModelState.IsValid)
            {
                db.PlayerDatas.Add(playerData);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(playerData);
        }

        // GET: PlayerDatas1/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlayerData playerData = db.PlayerDatas.Find(id);
            if (playerData == null)
            {
                return HttpNotFound();
            }
            return View(playerData);
        }

        // POST: PlayerDatas1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,PlayerName,XP,Password")] PlayerData playerData)
        {
            if (ModelState.IsValid)
            {
                db.Entry(playerData).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(playerData);
        }

        // GET: PlayerDatas1/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlayerData playerData = db.PlayerDatas.Find(id);
            if (playerData == null)
            {
                return HttpNotFound();
            }
            return View(playerData);
        }

        // POST: PlayerDatas1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            PlayerData playerData = db.PlayerDatas.Find(id);
            db.PlayerDatas.Remove(playerData);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
