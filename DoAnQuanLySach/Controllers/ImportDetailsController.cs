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
    public class ImportDetailsController : Controller
    {
        private BookManagerDbEntities db = new BookManagerDbEntities();

        // GET: ImportDetails
        public ActionResult Index()
        {
            var importDetails = db.ImportDetails.Include(i => i.Book).Include(i => i.Import);
            return View(importDetails.ToList());
        }

        // GET: ImportDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ImportDetail importDetail = db.ImportDetails.Find(id);
            if (importDetail == null)
            {
                return HttpNotFound();
            }
            return View(importDetail);
        }

        // GET: ImportDetails/Create
        public ActionResult Create()
        {
            ViewBag.BookId = new SelectList(db.Books, "BookId", "Title");
            ViewBag.ImportId = new SelectList(db.Imports, "ImportId", "ExportName");
            return View();
        }

        // POST: ImportDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ImportDetailId,ImportId,BookId,Quantity,Price")] ImportDetail importDetail)
        {
            if (ModelState.IsValid)
            {
                var list = db.ImportDetails;
                int tmp = 0;
                for (int i = 1; i <= list.ToList().Count; i++)
                {
                    ImportDetail user = db.ImportDetails.Find(i);
                    if (user == null) tmp = i;
                }
                importDetail.ImportDetailId = tmp;
                db.ImportDetails.Add(importDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BookId = new SelectList(db.Books, "BookId", "Title", importDetail.BookId);
            ViewBag.ImportId = new SelectList(db.Imports, "ImportId", "ExportName", importDetail.ImportId);
            return View(importDetail);
        }

        // GET: ImportDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ImportDetail importDetail = db.ImportDetails.Find(id);
            if (importDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.BookId = new SelectList(db.Books, "BookId", "Title", importDetail.BookId);
            ViewBag.ImportId = new SelectList(db.Imports, "ImportId", "ExportName", importDetail.ImportId);
            return View(importDetail);
        }

        // POST: ImportDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ImportDetailId,ImportId,BookId,Quantity,Price")] ImportDetail importDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(importDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BookId = new SelectList(db.Books, "BookId", "Title", importDetail.BookId);
            ViewBag.ImportId = new SelectList(db.Imports, "ImportId", "ExportName", importDetail.ImportId);
            return View(importDetail);
        }

        // GET: ImportDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ImportDetail importDetail = db.ImportDetails.Find(id);
            if (importDetail == null)
            {
                return HttpNotFound();
            }
            return View(importDetail);
        }

        // POST: ImportDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ImportDetail importDetail = db.ImportDetails.Find(id);
            db.ImportDetails.Remove(importDetail);
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
