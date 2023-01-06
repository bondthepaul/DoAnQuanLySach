using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DoAnQuanLySach.Models;

namespace DoAnQuanLySach.Controllers
{
    public class BooksController : Controller
    {
        private BookManagerDbEntities db = new BookManagerDbEntities();

        // GET: Books
        public ActionResult Index()
        {
            var categories = from c in db.Categories select c;
            ViewBag.categoryID = new SelectList(categories, "CategoryID", "CategoryName");
            var books = db.Books.Include(b => b.Category);
            return View(books.ToList());
        }
        public ActionResult Listbooks(string SearchString,string Publisherr, int categoryID = 0)
        {
            var books = from s in db.Books select s;
            var categories = from c in db.Categories select c;
            ViewBag.categoryID = new SelectList(categories, "CategoryId", "CategoryName");
            ViewBag.category = categories.ToList();
            Session["Timkiem"] = "-1";
            if (!String.IsNullOrEmpty(SearchString))
            {
                books = books.Where(s => s.Title.Contains(SearchString));
                Session["Timkiem"] = "1";
            }
            if (!String.IsNullOrEmpty(Publisherr))
            {
                books = books.Where(s => s.PublisherName.Contains(Publisherr));
                Session["Timkiem"] = "1";
            }
            if (categoryID != 0)
            {
                books = books.Where(x => x.CategoryId == categoryID);
                Session["Timkiem"] = "1";
            }
            books = books.OrderByDescending(s => s.Price);
            return View(books.ToList());
        }
        // GET: Books/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // GET: Books/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BookId,Title,AuthorName,Price,Amount,Year,CoverPage,CategoryId,PublisherName")] Book book, HttpPostedFileBase file)
        {
            System.Diagnostics.Debug.WriteLine(file);
            ViewBag.error = "";
            if (file != null) ViewBag.error = file.FileName; else 
            if (file == null ) ViewBag.error = "ASSSSSSSSSSSSSS";
            else 
            if (ModelState.IsValid && book.Title != null && book.AuthorName != null && book.Price != null && book.Year != null && book.PublisherName != null && file != null)
            {
                if(book.Price<0) ViewBag.error = "Giá tiền phải là số dương";
                else if (book.Year < 0) ViewBag.error = "Năm phải là số dương";
                else if (book.Amount < 0) ViewBag.error = "Số lượng phải là số dương";
                else
                {
                    if (file != null)
                    {
                        //file.SaveAs(HttpContext.Server.MapPath("~/Images/")+ file.FileName);
                        book.CoverPage = "/Content/images/" + file.FileName;

                    }
                    var list = db.Books;
                    int tmp = 0;
                    for (int i = 1; i <= list.ToList().Count; i++)
                    {
                        Book user = db.Books.Find(i);
                        if (user == null) tmp = i;
                    }
                    book.BookId = tmp;
                    db.Books.Add(book);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else ViewBag.error = "Nhập đữ liệu còn trống";
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", book.CategoryId);
            return View(book);
        }

        // GET: Books/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", book.CategoryId);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BookId,Title,AuthorName,Price,Amount,Year,CoverPage,CategoryId,PublisherName")] Book book, HttpPostedFileBase file)
        {
            System.Diagnostics.Debug.WriteLine(file);

            ViewBag.error = "";
            if (ModelState.IsValid && book.Title != null && book.AuthorName != null && book.Price != null && book.Year != null && book.PublisherName != null && file != null)
            {
                if (book.Price < 0) ViewBag.error = "Giá tiền phải là số dương";
                else if (book.Year < 0) ViewBag.error = "Năm phải là số dương";
                else if (book.Amount < 0) ViewBag.error = "Số lượng phải là số dương";
                else
                {
                    if (file != null)
                    {
                        //file.SaveAs(HttpContext.Server.MapPath("~/Images/")+ file.FileName);
                        book.CoverPage = "/Content/images/" + file.FileName;

                    }
                    if (book.Price < 0) ViewBag.error = "Giá tiền phải là số dương";
                    else if (book.Year < 0) ViewBag.error = "Năm phải là số dương";
                    else
                    {
                        if (file != null)
                        {
                            //file.SaveAs(HttpContext.Server.MapPath("~/Images/")+ file.FileName);
                            book.CoverPage = "/Content/images/" + file.FileName;

                        }
                        db.Entry(book).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }


            }
            else ViewBag.error = "Nhập đữ liệu còn trống";

            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", book.CategoryId);
            return View(book);
        }
        // GET: Books/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = db.Books.Find(id);
            db.Books.Remove(book);
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
