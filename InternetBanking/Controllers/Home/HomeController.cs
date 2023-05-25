using System.Web.Mvc;

namespace BookingApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Download()
        {
            return View();
        }
    }
}
