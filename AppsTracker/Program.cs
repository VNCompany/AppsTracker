var appBuilder = WebApplication.CreateBuilder(args);
appBuilder.Services.AddControllersWithViews();

var app = appBuilder.Build();

app.MapGet(pattern: "/", (context) =>
{
    context.Response.StatusCode = 404;
    context.Response.Headers.ContentType = "text/html; charset=utf-8";
    return context.Response.WriteAsync("<h1>Yep</h1>");
});

app.Run();