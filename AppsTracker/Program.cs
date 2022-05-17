using DataLayer;
using AppsTracker.Services;

var appBuilder = WebApplication.CreateBuilder(args);
appBuilder.Configuration.AddJsonFile("./dbsettings.json");

appBuilder.Services.AddScoped<DatabaseService>(provider => new DatabaseService(
    host: appBuilder.Configuration["postgresql:host"],
    user: appBuilder.Configuration["postgresql:user"],
    password: appBuilder.Configuration["postgresql:password"],
    dbname: appBuilder.Configuration["postgresql:dbname"]));

appBuilder.Services.AddScoped<AuthorizationService>();

appBuilder.Services.AddControllersWithViews();

var app = appBuilder.Build();
app.UseStaticFiles();
app.UseRouting();
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

app.Run();