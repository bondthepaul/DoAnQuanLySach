using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnQuanLySach.Models;

namespace DoAnQuanLySach.Controllers
{
    public class NhapHangController : Controller
    {
        List<Cartitem> giohang = new List<Cartitem>();
        BookManagerDbEntities db = new BookManagerDbEntities();

        public ActionResult Index()
        {
            ViewBag.BookId = db.Books;
            // Tạo đối tượng Session để truyeefnduwx liệu cho view
            List<Cartitem> giohang = Session["nhaphang"] as List<Cartitem>;
            return View(giohang); // Truyền Session với tên (key): giohang
        }
        public RedirectToRouteResult AddToCart(int BookId)
        {
            System.Diagnostics.Debug.WriteLine(BookId);
            // Nếu giỏ hàng chưa được khởi tạo
            if (Session["nhaphang"] == null)
            {
                // Khởi tạo Session["giohang"] là 1 List<BookItem>
                Session["nhaphang"] = new List<Cartitem>();
            }
            // Gán qua biến giohang cho dễ code
            giohang = Session["nhaphang"] as List<Cartitem>;
            // Kiểm tra xem sách khách đang chọn đã có trong giỏ hàng chưa ?
            // Nếu chưa có trong giỏ hàng
            if (giohang.FirstOrDefault(m => m.BookId == BookId) == null)
            {
                // Tìm sách theo id
                Book book = db.Books.Find(BookId);
                // Tạo ra 1 sách chọn (BookItem) mới
                Cartitem newItem = new Cartitem(BookId, book.Title, 1, (double)book.Price*0.2 , book.CoverPage);
                // Thêm BookItem vào giỏ
                giohang.Add(newItem);
            }
            // Nếu sách đã có trong giỏ hàng
            else
            {
                // Không thêm vào giỏ nữa mà tăng số lượng lên.
                Cartitem bookItem = giohang.FirstOrDefault(m => m.BookId == BookId);
                bookItem.Quantity++;
            }
            // Action này sẽ chuyển hướng về trang danh mục để khách chọn tiếp
            // có thể thay bằng lệnh return Redirect(Request.UrlReferrer.ToString());
            return RedirectToAction("Index", "NhapHang");
        }
        public RedirectToRouteResult UpdateQuantity(int? BookId, int newQuantity)
        {
            // gán Session cho biến giohang cho dễ code
            List<Cartitem> giohang = Session["nhaphang"] as List<Cartitem>;
            //tìm Bookitem muốn sửa và gọi là itemUpdate
            Cartitem itemUpdate = giohang.FirstOrDefault(m => m.BookId == BookId);
            // Nếu itemUpdate không null
            if (itemUpdate != null)
            {
                itemUpdate.Quantity = newQuantity; //gán số lượng mới
            }
            // Quay về trang danh mục chọn sách
            return RedirectToAction("Index", "NhapHang");
        }
        public RedirectToRouteResult RemoveCart(int id)
        {
            List<Cartitem> giohang = Session["nhaphang"] as List<Cartitem>;
            // Tìm sách có BookId = id và gọi là itemDelete
            Cartitem itemDelete = giohang.FirstOrDefault(m => m.BookId == id);
            if (itemDelete != null)
            {
                giohang.Remove(itemDelete);
            }
            // Quay về trang danh mục chọn sách
            return RedirectToAction("Index", "NhapHang");
        }
        public ActionResult ToPay(double sumtotal = 0)
        {	// Nhận giohang từ View truyền sang
            List<Cartitem> giohang = Session["nhaphang"] as List<Cartitem>;
            Cart myCart = new Cart();
            myCart.Total = sumtotal;
            Session["sumtotal"] = sumtotal;
            return View(giohang);
        }
        [HttpPost]
        public ActionResult ToPays(int CartId = 0, String UserName = "", String Total = "1")
        {
            // Nhận giohang từ View truyền sang
            List<Cartitem> giohang = Session["nhaphang"] as List<Cartitem>;
            Import import= new Import();
            var list = db.Imports;
            int tmp = 0;
            for (int i = 1; i <= list.ToList().Count + 1; i++)
            {
                Cart user = db.Carts.Find(i);
                if (user == null) tmp = i;
            }
            if (tmp == 0) tmp = list.ToList().Count + 1;
            import.ImportId = tmp; import.Date = DateTime.Now;
            import.ExportName = UserName; import.Total = Double.Parse(Total);

            if (1 == 1)
            {   // Ghi vào Cart
                db.Imports.Add(import);
                db.SaveChanges();

                // Ghi chi tiết vào CartDetail                
                if (giohang.Count > 0)
                {
                    foreach (var item in giohang)
                    {
                        ImportDetail idetail = new ImportDetail();
                        var listt = db.ImportDetails;
                        int tmpp = 0;
                        for (int i = 1; i <= listt.ToList().Count+1; i++)
                        {
                            CartDetail user = db.CartDetails.Find(i);
                            if (user == null) tmp = i;
                        }
                        if (tmpp == 0) tmpp = listt.ToList().Count + 1;
                        idetail.ImportDetailId = tmpp;
                        idetail.ImportId = import.ImportId; idetail.BookId = item.BookId;
                        idetail.Quantity = item.Quantity; idetail.Price = item.Price;
                        db.ImportDetails.Add(idetail);
                        db.SaveChanges();
                    }
                    // Xóa giỏ hàng
                    giohang.Clear();
                }
            }
            return RedirectToAction("Index", "Imports");
        }
    }
}