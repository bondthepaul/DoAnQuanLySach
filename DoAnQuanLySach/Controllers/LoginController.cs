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
    public class LoginController : Controller
    {
        private BookManagerDbEntities db = new BookManagerDbEntities();
        // GET: Login
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            var userr = from s in db.Userrs select s;
            userr = userr.Where(s => s.Username.CompareTo(username) == 0);
            foreach (var item in userr)
            {
                if (item.Password.CompareTo(password)==0)
                {
                    var categories = from c in db.Categories select c;
                    ViewBag.categoryID = new SelectList(categories, "CategoryID", "CategoryName");
                    Session["username"] = item.Username;
                    Session["accountname"] = item.Name;
                    Session["role"] = item.TypeID;
                    Session["userid"] = item.UserId;
                    ViewBag.error = "Good";
                    if(item.TypeID==1)
                    return View("~/Views/Home/Admin.cshtml");
                    else if (item.TypeID == 2)
                        return View("~/Views/Home/NhanVien.cshtml");
                    else return View("~/Views/Home/Index.cshtml");
                }    
            }
            Session["username"] = "";
            ViewBag.error = "Tên đăng nhập hoặc mật khẩu không hợp lệ";
            return View("Index");
        }
        [HttpGet]
        public ActionResult Logout()
        {
            Session.Remove("username"); Session.Remove("role"); Session.Remove("userid"); Session.Remove("accountname");
            return RedirectToAction("Index");
        }
    }
}