using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DoAnQuanLySach.Models;

namespace DoAnQuanLySach.Controllers
{
    public class TypeUsersController : Controller
    {
        private BookManagerDbEntities db = new BookManagerDbEntities();

        // GET: TypeUsers
        public ActionResult Index()
        {
            return View(db.TypeUsers.ToList());
        }

        // GET: TypeUsers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeUser typeUser = db.TypeUsers.Find(id);
            if (typeUser == null)
            {
                return HttpNotFound();
            }
            return View(typeUser);
        }

        // GET: TypeUsers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TypeUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TypeId,TypeName")] TypeUser typeUser)
        {
            if (ModelState.IsValid)
            {
                var list = db.TypeUsers;
                int tmp = 0;
                for (int i = 1; i <= list.ToList().Count; i++)
                {
                    TypeUser user = db.TypeUsers.Find(i);
                    if (user == null) tmp = i;
                }
                typeUser.TypeId = tmp;
                db.TypeUsers.Add(typeUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(typeUser);
        }

        // GET: TypeUsers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeUser typeUser = db.TypeUsers.Find(id);
            if (typeUser == null)
            {
                return HttpNotFound();
            }
            return View(typeUser);
        }

        // POST: TypeUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TypeId,TypeName")] TypeUser typeUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(typeUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(typeUser);
        }

        // GET: TypeUsers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeUser typeUser = db.TypeUsers.Find(id);
            if (typeUser == null)
            {
                return HttpNotFound();
            }
            return View(typeUser);
        }

        // POST: TypeUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TypeUser typeUser = db.TypeUsers.Find(id);
            db.TypeUsers.Remove(typeUser);
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
