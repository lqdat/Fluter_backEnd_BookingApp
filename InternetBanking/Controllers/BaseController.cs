using BookingApp.Models;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace BookingApp.Controllers
{
    public class BaseController : ApiController
    {
        public static CurrentUserModel GetCurrentUser()
        {
            var identity = HttpContext.Current.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var ma_tai_khoan = identity.FindFirst("ma_tai_khoan");

                if (ma_tai_khoan == null)
                {
                    return null;
                }
                else
                {
                    var user = new CurrentUserModel();
                    user.ma_tai_khoan = ma_tai_khoan.Value;
                    var id_don_vi = identity.FindFirst("id_don_vi");
                    if (id_don_vi != null)
                    {
                        user.id_don_vi = id_don_vi.Value;
                    }
                    var ma_don_vi = identity.FindFirst("ma_don_vi");
                    if (ma_don_vi != null)
                    {
                        user.ma_don_vi = ma_don_vi.Value;
                    }

                    return user;
                }
            }

            return null;
        }

        public static string IdDonVi
        {
            get
            {
                var user = GetCurrentUser();
                if (user != null)
                {
                    return user.id_don_vi;
                }
                return null;
            }
        }

        public static string MaDonVi
        {
            get
            {
                var user = GetCurrentUser();
                if (user != null)
                {
                    return user.ma_don_vi;
                }
                return null;
            }
        }

        public static string MaTaiKhoan
        {
            get
            {
                var user = GetCurrentUser();
                if (user != null)
                {
                    return user.ma_tai_khoan;
                }
                return null;
            }
        }
    }
}
