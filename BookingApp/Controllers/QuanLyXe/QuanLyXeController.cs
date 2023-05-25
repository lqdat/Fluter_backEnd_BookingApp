using BookingApp.Common;
using BookingApp.DataProvider.EF;
using BookingApp.Ultility.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace BookingApp.Controllers.QuanLyXe
{
    public class QuanLyXeController : ApiController
    {
        private Entities db = new Entities();

        [Route("QuanLyXe/UploadAnhXe")]
        [HttpPost]
        public HttpResponseMessage UploadAnhXe()
        {
            
            Guid ptd_Id = Guid.Parse(HttpContext.Current.Request["Id"]);
            HttpPostedFile file = HttpContext.Current.Request.Files.Count > 0 ? HttpContext.Current.Request.Files[0] : null;

            var dM_Xe = db.DM_Xe.SingleOrDefault(s => s.Id == ptd_Id);
            if (dM_Xe == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Không tồn tại thông tin xe");
            }

            if (file != null)
            {
                luuHinhAnh(dM_Xe, file);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "Đã cập nhật ảnh thành công");
            }
            else return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Không thể cập nhật ảnh");
        }
        private void luuHinhAnh(DM_Xe dM_Xe, HttpPostedFile file)
        {
            var path = Path.Combine(Helper.getFolder(dM_Xe.Id.ToString(), "HinhAnhXe"), dM_Xe.Id.ToString() + ".jpg");
            file.SaveAs(path);
            string base64String = Helper.getBase64(path);
            string path150 = Helper.SaveFileFromBase64(base64String, Helper.getFolder(dM_Xe.Id.ToString(), "HinhAnhXe"),dM_Xe.Id.ToString() + ".jpg", 100, 100, true);
           
            dM_Xe.URLImage = path150.Replace(HttpContext.Current.Server.MapPath("~/"), "/").Replace("\\", "/");
           
        }
    }
}
