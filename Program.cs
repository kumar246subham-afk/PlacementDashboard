using PlacementDashboard.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register ADO.NET PlacementRepository
builder.Services.AddScoped<PlacementRepository>();

// Bind to port 5000 and 5226
builder.WebHost.UseUrls("http://localhost:5000", "http://localhost:5226");

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

app.Run();
