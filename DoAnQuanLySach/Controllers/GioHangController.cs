using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnQuanLySach.Models;
namespace DoAnQuanLySach.Controllers
{
    public class GioHangController : Controller
    {
        List<Cartitem> giohang = new List<Cartitem>();
        BookManagerDbEntities db = new BookManagerDbEntities();

        public ActionResult Index()
        {
            // Tạo đối tượng Session để truyeefnduwx liệu cho view
            List<Cartitem> giohang = Session["giohang"] as List<Cartitem>;
            return View(giohang); // Truyền Session với tên (key): giohang
        }
        public RedirectToRouteResult AddToCart(int id)
        {
            // Nếu giỏ hàng chưa được khởi tạo
            if (Session["giohang"] == null)
            {
                // Khởi tạo Session["giohang"] là 1 List<BookItem>
                Session["giohang"] = new List<Cartitem>();
            }
            // Gán qua biến giohang cho dễ code
            giohang = Session["giohang"] as List<Cartitem>;
            // Kiểm tra xem sách khách đang chọn đã có trong giỏ hàng chưa ?
            // Nếu chưa có trong giỏ hàng
            if (giohang.FirstOrDefault(m => m.BookId == id) == null)
            {
                // Tìm sách theo id
                Book book = db.Books.Find(id);
                // Tạo ra 1 sách chọn (BookItem) mới
                Cartitem newItem = new Cartitem(id, book.Title, 1, (double)book.Price, book.CoverPage);
                // Thêm BookItem vào giỏ
                giohang.Add(newItem);
            }
            // Nếu sách đã có trong giỏ hàng
            else
            {
                // Không thêm vào giỏ nữa mà tăng số lượng lên.
                Cartitem bookItem = giohang.FirstOrDefault(m => m.BookId == id);
                bookItem.Quantity++;
            }
            // Action này sẽ chuyển hướng về trang danh mục để khách chọn tiếp
            // có thể thay bằng lệnh return Redirect(Request.UrlReferrer.ToString());
            return RedirectToAction("ListBooks", "Books");
        }
        public RedirectToRouteResult UpdateQuantity(int? BookId, int newQuantity)
        {
            // gán Session cho biến giohang cho dễ code
            List<Cartitem> giohang = Session["giohang"] as List<Cartitem>;
            //tìm Bookitem muốn sửa và gọi là itemUpdate
            Cartitem itemUpdate = giohang.FirstOrDefault(m => m.BookId == BookId);
            // Nếu itemUpdate không null
            if (itemUpdate != null)
            {
                itemUpdate.Quantity = newQuantity; //gán số lượng mới
            }
            // Quay về trang danh mục chọn sách
            return RedirectToAction("Index", "giohang");
        }
        public RedirectToRouteResult RemoveCart(int id)
        {
            List<Cartitem> giohang = Session["giohang"] as List<Cartitem>;
            // Tìm sách có BookId = id và gọi là itemDelete
            Cartitem itemDelete = giohang.FirstOrDefault(m => m.BookId == id);
            if (itemDelete != null)
            {
                giohang.Remove(itemDelete);
            }
            // Quay về trang danh mục chọn sách
            return RedirectToAction("Index", "giohang");
        }
        public ActionResult ToPay(double sumtotal = 0)
        {	// Nhận giohang từ View truyền sang
            List<Cartitem> giohang = Session["giohang"] as List<Cartitem>;
            Cart myCart = new Cart();
            myCart.Total = sumtotal;
            Session["sumtotal"] = sumtotal;
            return View(giohang);
        }
        [HttpPost]
        public ActionResult ToPays(int CartId = 0, int UserId = 0, String Total = "1")
        {
            // Nhận giohang từ View truyền sang
            List<Cartitem> giohang = Session["giohang"] as List<Cartitem>;
            Cart cart = new Cart();
            var list = db.Carts;
            int tmp = 0;
            for (int i = 1; i <= list.ToList().Count+1; i++)
            {
                Cart user = db.Carts.Find(i);
                if (user == null) tmp = i;
            }
            if (tmp == 0) tmp = list.ToList().Count + 1;
            cart.CartId = tmp; cart.Datebuy = DateTime.Now;
            cart.UserID = UserId; cart.Total = Double.Parse(Total);

            if (1==1)
            {   // Ghi vào Cart
                db.Carts.Add(cart);
                db.SaveChanges();

                // Ghi chi tiết vào CartDetail                
                if (giohang.Count > 0)
                {
                    foreach (var item in giohang)
                    {
                        CartDetail cdetail = new CartDetail();
                        var listt = db.CartDetails;
                        int tmpp = 0;
                        for (int i = 1; i <= listt.ToList().Count; i++)
                        {
                            CartDetail user = db.CartDetails.Find(i);
                            if (user == null) tmp = i;
                        }
                        if (tmpp == 0) tmpp = listt.ToList().Count + 1;
                        cdetail.CartDetailId = tmpp;
                        cdetail.CartId = cart.CartId; cdetail.BookId = item.BookId;
                        cdetail.Quantity = item.Quantity; cdetail.Price = item.Price;
                        db.CartDetails.Add(cdetail);
                        db.SaveChanges();
                    }
                    // Xóa giỏ hàng
                    giohang.Clear();
                }
            }
            return RedirectToAction("ListBooks", "Books");
        }
    }
}