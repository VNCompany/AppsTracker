using Microsoft.AspNetCore.Mvc;

using DataLayer;
using DataLayer.Models;
using AppsTracker.Services;

namespace AppsTracker.Controllers
{
    public class AppsController : Controller
    {
        DatabaseService db;
        AuthorizationService auth;
        public AppsController(DatabaseService databaseService, AuthorizationService authorizationService)
        {
            db = databaseService;
            auth = authorizationService;
        }

        public IActionResult New()
        {
            if (!auth.Authenticate(Request.Cookies))
                return StatusCode(403);

            return View();
        }

        [HttpPost]
        public IActionResult New(string appname)
        {
            if (!auth.Authenticate(Request.Cookies))
                return StatusCode(403);

            if (string.IsNullOrWhiteSpace(appname))
            {
                ViewData["Error"] = "Название приложения не может быть пустым.";
                return View();
            }

            if (!db.Apps.Create(auth.User!.Id, appname))
            {
                ViewData["Error"] = "Приложение с таким именем уже существует.";
                return View();
            }
            else return Redirect("/Apps");
        }

        public IActionResult Index()
        {
            if (!auth.Authenticate(Request.Cookies))
                return StatusCode(403);

            return View(db.Apps.GetList(auth.User!.Id));
        }
    }
}
