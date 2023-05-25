using BookingApp.DataProvider.EF;
using BookingApp.Models;
using BookingApp.Ultility.Helpers;
using Microsoft.Ajax.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BookingApp.Controllers.Home
{
    public class LoginController : ApiController
    {
        private Entities db = new Entities();


        [Route("Login/Authenticate")]
        [HttpPost]
        public HttpResponseMessage Authenticate([FromBody] LoginRequest login)
        {
            var MatKhau = Helper.MD5.CryptoPassword(login.Password);
            var taikhoan = db.TaiKhoans.FirstOrDefault(item => item.UserName == login.Username);

            if (taikhoan != null)
            {
                if (taikhoan?.Status == true)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Tài khoản đã bị khóa! Vui lòng liên hệ quản trị hệ thống!");
                }

                if (taikhoan.PassWord == MatKhau)
                {


                    TokenValidationHandler tokenValidator = new TokenValidationHandler();
                    string token = tokenValidator.CreateToken(taikhoan);

                    LoginResponse loginResponse = new LoginResponse();
                    loginResponse.Token = token;
                    loginResponse.TaiKhoan = getTaiKhoan(taikhoan);

                    return Request.CreateResponse(HttpStatusCode.OK, loginResponse);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Mật khẩu không đúng");
                }
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Tài khoản không tồn tại");
            }
        }

        private LoginModel getTaiKhoan(TaiKhoan tk)
        {
            LoginModel taiKhoan = new LoginModel();
            taiKhoan.Id = tk.Id;
            taiKhoan.TenHienThi = tk.TenHienThi;
            taiKhoan.Status = tk.Status;
            taiKhoan.Email = tk.Email;
            taiKhoan.SDT = tk.SDT;
            return taiKhoan;
        }

        [Route("Login/LogOff")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage LogOff()
        {
            return Request.CreateErrorResponse(HttpStatusCode.OK, "Đăng xuất thành công");
        }
    }
}
