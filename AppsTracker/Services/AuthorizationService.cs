using Microsoft.AspNetCore.CookiePolicy;

using DataLayer;
using DataLayer.Models;

namespace AppsTracker.Services
{
    public class AuthorizationService : IDisposable
    {
        User? user;
        readonly DatabaseService db;

        public User? User => user;

        public bool IsAuthenticated => user != null;

        public AuthorizationService(DatabaseService databaseService)
        {
            db = databaseService;
            Console.WriteLine("AuthorizationService is created");
        }

        public bool Authenticate(IRequestCookieCollection cookies)
        {
            if (IsAuthenticated) return true;

            if (cookies.ContainsKey("token") && cookies.ContainsKey("email"))
            {
                string email = cookies["email"]!.ToString();
                User? user = null;

                if (!string.IsNullOrWhiteSpace(email)
                    && (user = db.Users.Get(DatabaseService.AntiSqlInjection(email))) != null
                    && user.Password == cookies["token"]!.ToString())
                {
                    this.user = user;
                    return true;
                }
            }

            return false;
        }

        public bool Login(string email, string password, IResponseCookies cookies)
        {
            User? user = db.Users.Get(DatabaseService.AntiSqlInjection(email));
            if (user != null && DatabaseService.HashPassword(password) == user.Password)
            {
                var cookieOptions = new CookieOptions()
                {
                    Expires = DateTime.Now.AddDays(20),
                    Path = "/"
                };

                cookies.Append("email", email, cookieOptions);
                cookies.Append("token", DatabaseService.HashPassword(password), cookieOptions);
                return true;
            }

            return false;
        }

        public bool Register(string email, string password)
        {
            if (!string.IsNullOrEmpty(email)
                && !string.IsNullOrEmpty(password)
                && db.Users.Create(DatabaseService.AntiSqlInjection(email), DatabaseService.HashPassword(password)))
                return true;

            return false;
        }

        public void Dispose()
        {
            Console.WriteLine("AuthorizationService is disposed");
        }
    }
}