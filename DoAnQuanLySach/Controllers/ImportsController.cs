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
    public class ImportsController : Controller
    {
        private BookManagerDbEntities db = new BookManagerDbEntities();

        // GET: Imports
        public ActionResult Index(int Thang=0, int Quy=0, int Nam=0)
        {
            Session["Thang"] = Thang;
            Session["Quy"] = Quy;
            Session["Nam"] = Nam;
            Session["Tongtien"] = 0;
            var imports = from s in db.Imports select s;
            if (Thang != 0)
            {
                imports = imports.Where(s => s.Date.Value.Month==Thang);
            }
            if (Quy != 0) 
            {
                if(Quy==1) imports = imports.Where(s => s.Date.Value.Month >=0 && s.Date.Value.Month <= 3);
                else if (Quy == 2) imports = imports.Where(s => s.Date.Value.Month >= 4 && s.Date.Value.Month <= 6);
                else if (Quy == 3) imports = imports.Where(s => s.Date.Value.Month >= 7 && s.Date.Value.Month <= 9);
                else if (Quy == 4) imports = imports.Where(s => s.Date.Value.Month >= 10 && s.Date.Value.Month <= 12);
            }
            if (Nam != 0) 
            {
                imports = imports.Where(s => s.Date.Value.Year == Nam);
            }
            foreach(var item in imports)
            {
                Session["Tongtien"] = double.Parse(Session["Tongtien"].ToString()) + item.Total;
            }
            return View(imports.ToList());
        }

        // GET: Imports/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Import import = db.Imports.Find(id);
            if (import == null)
            {
                return HttpNotFound();
            }
            return View(import);
        }

        // GET: Imports/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Imports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ImportId,Date,ExportName,Total")] Import import)
        {
            if (ModelState.IsValid)
            {
                var list = db.Imports;
                int tmp = 0;
                for (int i = 1; i <= list.ToList().Count; i++)
                {
                    Import user = db.Imports.Find(i);
                    if (user == null) tmp = i;
                }
                import.ImportId = tmp;
                db.Imports.Add(import);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(import);
        }

        // GET: Imports/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Import import = db.Imports.Find(id);
            if (import == null)
            {
                return HttpNotFound();
            }
            return View(import);
        }

        // POST: Imports/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ImportId,Date,ExportName,Total")] Import import)
        {
            if (ModelState.IsValid)
            {
                db.Entry(import).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(import);
        }

        // GET: Imports/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Import import = db.Imports.Find(id);
            if (import == null)
            {
                return HttpNotFound();
            }
            return View(import);
        }

        // POST: Imports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Import import = db.Imports.Find(id);
            db.Imports.Remove(import);
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
