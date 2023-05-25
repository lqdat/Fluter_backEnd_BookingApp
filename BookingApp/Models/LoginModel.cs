using System;
using System.Collections.Generic;

namespace BookingApp.Models
{
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponse
    {
        public string Token { get; set; }
        public LoginModel TaiKhoan { get; set; }
    }

    public class LoginModel
    {
        public Guid Id { get; set; }
        public string MaTaiKhoan { get; set; }
        public string TenHienThi { get; set; }
        public bool? Status { get; set; }
        public string Email { get; set; }
        public string SDT { get; set; }

    }
    public class RegisterModel
    {
        public string TenDangNhap { get; set; }
        public string MatKhau { get; set; }
        public string HoTen { get; set; }
        public string Email { get; set; }
        public string SDT { get; set; }
    }





    public class CurrentUserModel
    {
        public string ma_don_vi { get; set; }
        public string ma_tai_khoan { get; set; }
        public string id_don_vi { get; set; }
        public string ten_don_vi { get; set; }
    }

}