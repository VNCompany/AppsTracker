using Microsoft.AspNetCore.Mvc;

using DataLayer;
using DataLayer.Models;

namespace AppsTracker.Api
{
    [Route("api/[controller]")]
    public class NewEventController : Controller
    {
        DatabaseService db;
        public NewEventController(DatabaseService databaseService)
        {
            db = databaseService;
        }

        public string Post([FromBody]AppEvent appEvent)
        {
            try
            {
                if (appEvent == null)
                    return "invalid json";
                if (appEvent.AppId == null)
                    return "appId is empty";
                if (appEvent.Name == null || string.IsNullOrEmpty(appEvent.Name))
                    return "name is empty";
                if (appEvent.Description == null)
                    appEvent.Description = "";

                appEvent.Date = DateTime.Now;

                return db.Apps.NewEvent(appEvent) ? "ok" : "invalid appId";
            } 
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}