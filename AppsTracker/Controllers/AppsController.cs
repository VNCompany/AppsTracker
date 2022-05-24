using Microsoft.AspNetCore.Mvc;

using DataLayer;
using DataLayer.Models;
using DataLayer.ViewModels;
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
        
        public IActionResult Statistics(int id)
        {
            if (!auth.Authenticate(Request.Cookies))
                return StatusCode(403);

            var period = DataLayer.Repositories.AppStatisticsPeriod.Day;
            if (Request.Query.TryGetValue("period", out var sPeriod)
                && int.TryParse(sPeriod, out int iPeriod)
                && iPeriod >= 1 && iPeriod <= 3)
                period = (DataLayer.Repositories.AppStatisticsPeriod)iPeriod;

            AppEventsViewModel? vm = db.Apps.GetStatistics(auth.User!.Id, id, period);

            if (vm != null)
            {
                ViewData["Period"] = period;
                ViewData["Title"] = db.Apps[id]!.Name;
                return View(vm);
            }
            else return StatusCode(404);
        }
    }
}
