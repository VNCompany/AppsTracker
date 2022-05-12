using DataLayer;

var appBuilder = WebApplication.CreateBuilder(args);
appBuilder.Configuration.AddJsonFile("./dbsettings.json");
appBuilder.Services.AddControllersWithViews();

var app = appBuilder.Build();
app.UseStaticFiles();
app.UseRouting();
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

Database.Configure(
    host: app.Configuration["postgresql:host"],
    user: app.Configuration["postgresql:user"],
    password: app.Configuration["postgresql:password"],
    dbname: app.Configuration["postgresql:dbname"]);

app.Run();