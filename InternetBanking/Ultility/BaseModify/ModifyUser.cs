
using BookingApp.Ultility.BaseControllers;
using System;
using System.Linq;
using System.Web;

namespace BookingApp.Ultility.BaseModify
{
    public class ModifyUser<T> : BaseController
        where T : class
    {
        public static T ModifySet(T t, string m = null)
        {
            var method = !string.IsNullOrEmpty(m) ? m : HttpContext.Current.Request.HttpMethod;
            if (method == "POST")
            {
                t.GetType().GetProperty("CreatedBy").SetValue(t, MaTaiKhoan);
                t.GetType().GetProperty("CreatedDate").SetValue(t, DateTime.Now);
            }
            else if ((new string[] { "PUT", "PATCH" }).Contains(method))
            {
                t.GetType().GetProperty("UpdatedBy").SetValue(t, MaTaiKhoan);
                t.GetType().GetProperty("UpdatedDate").SetValue(t, DateTime.Now);
            }

            return t;
        }
    }
}