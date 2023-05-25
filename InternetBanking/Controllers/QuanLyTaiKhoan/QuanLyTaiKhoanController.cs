using BookingApp.DataProvider.EF;
using BookingApp.Models;
using BookingApp.Ultility.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;

namespace BookingApp.Controllers
{
    [Authorize]
    public class QuanLyTaiKhoanController : ApiController
    {
        private Entities db = new Entities();

        [Route("QuanLyTaiKhoan/LayThongTinTaiKhoan")]
        [HttpGet]
        public HttpResponseMessage LayThongTinTaiKhoan()
        {
            var taiKhoan_Id = BaseController.GetCurrentUser().ma_tai_khoan;
            var taiKhoan = db.TaiKhoans.Where(f => f.Id.ToString() == taiKhoan_Id).Select(s => new LoginModel
            {
                Id = s.Id,
                TenHienThi = s.TenHienThi,
                SDT=s.SDT,
                Email = s.Email,
                Status = s.Status,
            }).FirstOrDefault();
          
            return Request.CreateResponse(HttpStatusCode.OK, taiKhoan);
        }


        [Route("QuanLyTaiKhoan/DangKy")]
        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage DangKy([FromBody] RegisterModel model)
        {
            try
            {
                var checkUserName = db.TaiKhoans.Any(a => a.UserName == model.TenDangNhap);
                if (checkUserName)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { value = "Tên đăng nhập đã được sử dụng!" });
                }

                var taiKhoan = new TaiKhoan()
                {
                    Id = Guid.NewGuid(),
                    CreateBy="admin",
                    CreateDate= DateTime.Now,
                    TenHienThi = model.HoTen,
                    UserName = model.TenDangNhap,
                    PassWord = Helper.MD5.CryptoPassword(model.MatKhau),
                    Email= model.Email,
                    SDT=model.SDT,

                };
                db.TaiKhoans.Add(taiKhoan);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK,  "Thành công!" );
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Lỗi!" );
            }
        }



        [Route("QuanLyTaiKhoan/DoiMatKhau")]
        [HttpPost]
        public HttpResponseMessage DoiMatKhau(string matKhauCu, string matKhauMoi)
        {
            var taiKhoan_Id = BaseController.GetCurrentUser().ma_tai_khoan;
            var taiKhoan = db.TaiKhoans.FirstOrDefault(f => f.Id.ToString() == taiKhoan_Id);

            var mKCu = Helper.MD5.CryptoPassword(matKhauCu);
            if (taiKhoan.PassWord == mKCu)
            {
                taiKhoan.PassWord = Helper.MD5.CryptoPassword(matKhauMoi);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    value = "Đổi Thành công!"
                });
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, new
            {
                value = "Mật khẩu cũ chưa đúng!"
            });
            
        }

        [Route("QuanLyTaiKhoan/UploadAnh")]
        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage UploadAnh()
        {
            Guid Id = Guid.Parse(HttpContext.Current.Request["Id"]);
            HttpPostedFile file = HttpContext.Current.Request.Files.Count > 0 ? HttpContext.Current.Request.Files[0] : null;

            var tk = db.TaiKhoans.SingleOrDefault(s => s.Id == Id);
            if (tk == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Không tồn tại thông tin tài khoản");
            }

            if (file != null)
            {
                luuHinhAnh(tk, file);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "Đã cập nhật ảnh thành công");
            }
            else return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Không thể cập nhật ảnh");
        }
        private void luuHinhAnh(TaiKhoan tk, HttpPostedFile file)
        {
            var path = Path.Combine(Helper.getFolder(tk.Id.ToString(), "HinhAnh"), tk.Id.ToString() + ".jpg");
            file.SaveAs(path);
            string base64String = Helper.getBase64(path);
            string path150 = Helper.SaveFileFromBase64(base64String, Helper.getFolder(tk.Id.ToString(), "HinhAnh"), tk.Id.ToString() + ".jpg", 100, 100, true);

            tk.URLImage = path150.Replace(HttpContext.Current.Server.MapPath("~/"), "/").Replace("\\", "/");

        }

    }
}
