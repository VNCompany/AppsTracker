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

            if (auth.IsAuthenticated)
            {
                ViewData["Message"] = $"Количество добавленных приложений: {db.Apps.Count(auth.User!.Id)}";
            }
            else
            {
                ViewData["Message"] = "Войдите или зарегистрируйтесь, чтобы посмотреть список приложений.";
            }

            return View();
        }
    }
}