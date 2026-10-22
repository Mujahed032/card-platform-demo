using CardPlatformDemo.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// In-memory card catalog + rule engine (no database).
builder.Services.AddSingleton<CardCatalog>();

// Render / cloud: bind to the port provided via the PORT env variable.
// Only applied when PORT is set (Render sets it). Locally this is skipped so
// Visual Studio uses launchSettings.json and opens the browser automatically.
var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrEmpty(port))
{
    builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

// Note: no HTTPS redirect — Render terminates TLS at its proxy and forwards HTTP.
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
