using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAnQuanLySach.Models
{
    public class Cartitem
    {
        int bookId;
        string title;        // Tiêu đề
        int quantity;        // số lượng
        double price;        // Giá bán
        string coverPage;    // Hình bìa        
        double total;        // Thành tiền
        public int BookId { get => bookId; set => bookId = value; }
        public string Title { get => title; set => title = value; }
        public int Quantity { get => quantity; set => quantity = value; }
        public double Price { get => price; set => price = value; }
        public string CoverPage { get => coverPage; set => coverPage = value; }
        public double Total { get => price * Quantity; }
        public Cartitem() { }
        public Cartitem(int id, string title, int quality, double price, string img)
        {
            this.bookId = id;
            this.title = title;
            this.quantity = quality;
            this.price = price;
            this.coverPage = img;
            this.total = quality * price;
        }
    }
}