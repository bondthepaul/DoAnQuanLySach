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
    public class CartsController : Controller
    {
        private BookManagerDbEntities db = new BookManagerDbEntities();

        // GET: Carts
        public ActionResult Index()
        {
            var carts = db.Carts.Include(c => c.Userr);
            return View(carts.ToList());
        }
        public ActionResult BaoCao(int Thang = 0, int Quy = 0, int Nam = 0)
        {
            var carts = db.Carts.Include(c => c.Userr);
            Session["Thang"] = Thang;
            Session["Quy"] = Quy;
            Session["Nam"] = Nam;
            Session["Tongtien"] = 0;
            if (Thang != 0)
            {
                carts = carts.Where(s => s.Datebuy.Value.Month == Thang);
            }
            if (Quy != 0)
            {
                if (Quy == 1) carts = carts.Where(s => s.Datebuy.Value.Month >= 0 && s.Datebuy.Value.Month <= 3);
                else if (Quy == 2) carts = carts.Where(s => s.Datebuy.Value.Month >= 4 && s.Datebuy.Value.Month <= 6);
                else if (Quy == 3) carts = carts.Where(s => s.Datebuy.Value.Month >= 7 && s.Datebuy.Value.Month <= 9);
                else if (Quy == 4) carts = carts.Where(s => s.Datebuy.Value.Month >= 10 && s.Datebuy.Value.Month <= 12);
            }
            if (Nam != 0)
            {
                carts = carts.Where(s => s.Datebuy.Value.Year == Nam);
            }
            foreach (var item in carts)
            {
                Session["Tongtien"] = double.Parse(Session["Tongtien"].ToString()) + item.Total;
            }
            return View(carts.ToList());

        }
        // GET: Carts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }

        // GET: Carts/Create
        public ActionResult Create()
        {
            ViewBag.UserID = new SelectList(db.Userrs, "UserId", "Username");
            return View();
        }

        // POST: Carts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CartId,Datebuy,UserID,Total")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                db.Carts.Add(cart);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserID = new SelectList(db.Userrs, "UserId", "Username", cart.UserID);
            return View(cart);
        }

        // GET: Carts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(db.Userrs, "UserId", "Username", cart.UserID);
            return View(cart);
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CartId,Datebuy,UserID,Total")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cart).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.Userrs, "UserId", "Username", cart.UserID);
            return View(cart);
        }

        // GET: Carts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cart cart = db.Carts.Find(id);
            db.Carts.Remove(cart);
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
