//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DoAnQuanLySach.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Book
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Book()
        {
            this.Chapters = new HashSet<Chapter>();
            this.CartDetails = new HashSet<CartDetail>();
            this.ImportDetails = new HashSet<ImportDetail>();
        }
    
        public int BookId { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<int> Amount { get; set; }
        public Nullable<int> Year { get; set; }
        public string CoverPage { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public string PublisherName { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Chapter> Chapters { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CartDetail> CartDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ImportDetail> ImportDetails { get; set; }
        public virtual Category Category { get; set; }
    }
}
