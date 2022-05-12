using Microsoft.AspNetCore.Mvc;
using DataLayer;

namespace AppsTracker
{
    public class HomeController : Controller
    {
        public HomeController()
        {

        }

        public IActionResult Index()
        {
            HttpContext.Response.Cookies.Append("", "", new CookieOptions() {  })
            return View();
        }
    }
}