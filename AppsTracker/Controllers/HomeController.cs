using Microsoft.AspNetCore.Mvc;

using DataLayer;
using DataLayer.Models;
using AppsTracker.Services;

namespace AppsTracker
{
    public class HomeController : Controller
    {
        DatabaseService db;
        AuthorizationService auth;
        public HomeController(DatabaseService context, AuthorizationService authorizationService)
        {
            db = context;
            auth = authorizationService;
        }

        public IActionResult Index()
        {
            auth.Authenticate(HttpContext.Request.Cookies);
            return View();
        }
    }
}