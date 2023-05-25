
using BookingApp.DataProvider.EF;
using Microsoft.Data.Edm;
using System.Web.Http;
using System.Web.Http.OData.Builder;

namespace BookingApp.WebAPI
{
    public static class ODataEdmBuilder
    {
        public static IEdmModel GetEdmModelV3(this HttpConfiguration config)
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder(config);
           
            builder.EntitySet<TaiKhoan>("TaiKhoans");
            builder.EntitySet<DM_Xe>("DM_Xe");
            builder.EntitySet<BookHistory>("BookHistories");
            builder.EntitySet<DM_MaKhuyenMai>("DM_MaKhuyenMai");
            builder.EntitySet<ThongBao>("ThongBaos");
            builder.EntitySet<DM_Voucher>("DM_Voucher");

            
           
            return builder.GetEdmModel();
        }
    }
}
