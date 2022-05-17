using Microsoft.AspNetCore.Mvc;

using AppsTracker.Services;

namespace AppsTracker
{
    public class AccountController : Controller
    {
        AuthorizationService auth;
        public AccountController(AuthorizationService authorizationService)
        {
            auth = authorizationService;
        }

        public IActionResult Login()
        {
            if (auth.Authenticate(HttpContext.Request.Cookies))
            {
                return Redirect("/Home/Index");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            if (!string.IsNullOrEmpty(email) 
                && !string.IsNullOrEmpty(password) 
                && auth.Login(email, password, HttpContext.Response.Cookies)) 
                
                return Redirect("/Home/Index");
            else
            {
                ViewData["Error"] = "Неверный Email или Пароль";
                return View();
            }
        }

        public IActionResult Register()
        {
            if (auth.Authenticate(HttpContext.Request.Cookies))
            {
                return Redirect("/Home/Index");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Register(string email, string password, string repeat)
        {
            if (string.IsNullOrWhiteSpace(email)
                || string.IsNullOrWhiteSpace(password))
            {
                ViewData["Error"] = "Заполните пустые поля";
                return View();
            }
            else if (password != repeat)
            {
                ViewData["Error"] = "Пароли не совпадают";
                return View();
            }
            else
            {
                if (auth.Register(email, password))
                    return Redirect("/Account/Login");
                else
                {
                    ViewData["Error"] = "Пользователь с указанным Email уже существует";
                    return View();
                }
            }
        }

        public IActionResult Logout()
        {
            foreach (var cookie in Request.Cookies)
            {
                Response.Cookies.Delete(cookie.Key);
            }
            return Redirect("/Home/Index");
        }
    }
}