//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BookingApp.DataProvider.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class DM_MaKhuyenMai
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DM_MaKhuyenMai()
        {
            this.BookHistories = new HashSet<BookHistory>();
        }
    
        public System.Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public Nullable<int> Prepayment { get; set; }
        public Nullable<System.DateTime> ExpretionDate { get; set; }
        public Nullable<System.Guid> TaiKhoan_Id { get; set; }
        public Nullable<int> Status { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BookHistory> BookHistories { get; set; }
        public virtual TaiKhoan TaiKhoan { get; set; }
    }
}
