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
    
    public partial class ThongBao
    {
        public System.Guid Id { get; set; }
        public Nullable<System.Guid> TaiKhoan_Id { get; set; }
        public string NoiDung { get; set; }
        public string TieuDe { get; set; }
        public Nullable<System.DateTime> Ngay { get; set; }
    
        public virtual TaiKhoan TaiKhoan { get; set; }
    }
}
