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
    public class UserrsController : Controller
    {
        private BookManagerDbEntities db = new BookManagerDbEntities();

        // GET: Userrs
        public ActionResult Index()
        {
            var userrs = db.Userrs.Include(u => u.TypeUser);
            return View(userrs.ToList());
        }

        // GET: Userrs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Userr userr = db.Userrs.Find(id);
            if (userr == null)
            {
                return HttpNotFound();
            }
            return View(userr);
        }
        //ass
        public ActionResult Signup()
        {
            ViewBag.TypeID = new SelectList(db.TypeUsers, "TypeId", "TypeName");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Signup([Bind(Include = "UserId,Username,Password,Name,Dateofbirth,TypeID")] Userr userr, string password)
        {
            ViewBag.TypeID = new SelectList(db.TypeUsers, "TypeId", "TypeName", userr.TypeID);
            var list = db.Userrs;
            int tmp = 0;
            for (int i = 1; i <= list.ToList().Count; i++)
            {
                Userr user = db.Userrs.Find(i);
                if (user == null) tmp = i;
            }
            if (tmp == 0) tmp = list.ToList().Count + 1;
            userr.UserId =tmp;
            userr.TypeID = 3;
            if (ModelState.IsValid)
            {
                foreach (var item in list)
                {
                    if (item.Username.CompareTo(userr.Username) == 0) { ViewBag.error = "Tên đăng nhập bị trùng"; return View(userr); }
                }
                if (userr.Username.Length <= 3) { ViewBag.error = "Tên đăng nhập phải nhiều hơn 3 kí tự"; return View(userr); }
                if (userr.Password.Length < 3) { ViewBag.error = "Mật khẩu phải ít nhất 3 kí tự"; return View(userr); }
                if (userr.Password.CompareTo(password)==0)
                {
                    db.Userrs.Add(userr);
                    db.SaveChanges();
                    ViewBag.error = "Đăng kí thành công";
                    return RedirectToAction("Index", "Login");
                }else ViewBag.error = "Nhập lại mật khẩu không đúng";
            }
            else ViewBag.error = "Đăng kí thất bại";
            return View(userr);
        }

        // GET: Userrs/Create
        public ActionResult Create()
        {
            ViewBag.TypeID = new SelectList(db.TypeUsers, "TypeId", "TypeName");
            return View();
        }

        // POST: Userrs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,Username,Password,Name,Dateofbirth,TypeID")] Userr userr)
        {
            if (ModelState.IsValid)
            {
                var list = db.Userrs;
                int tmp = 0;
                for (int i = 1; i <= list.ToList().Count; i++)
                {
                    Userr user = db.Userrs.Find(i);
                    if (user == null) tmp = i;
                }
                if (db.Userrs.Find(userr.Username)==null)
                {
                    userr.UserId = tmp;
                    db.Userrs.Add(userr);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }

            ViewBag.TypeID = new SelectList(db.TypeUsers, "TypeId", "TypeName", userr.TypeID);
            return View(userr);
        }

        // GET: Userrs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Userr userr = db.Userrs.Find(id);
            if (userr == null)
            {
                return HttpNotFound();
            }
            ViewBag.TypeID = new SelectList(db.TypeUsers, "TypeId", "TypeName", userr.TypeID);
            return View(userr);
        }

        // POST: Userrs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,Username,Password,Name,Dateofbirth,TypeID")] Userr userr)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userr).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TypeID = new SelectList(db.TypeUsers, "TypeId", "TypeName", userr.TypeID);
            return View(userr);
        }

        // GET: Userrs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Userr userr = db.Userrs.Find(id);
            if (userr == null)
            {
                return HttpNotFound();
            }
            return View(userr);
        }

        // POST: Userrs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Userr userr = db.Userrs.Find(id);
            db.Userrs.Remove(userr);
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
