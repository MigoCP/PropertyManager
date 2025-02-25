using Microsoft.EntityFrameworkCore;
using Property_Manager_WebApp.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add console logging
builder.Logging.AddConsole();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

// Add database context
builder.Services.AddDbContext<PropertyRentalManagementContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Add Session services
builder.Services.AddDistributedMemoryCache(); // Enables in-memory caching for session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session timeout (30 minutes)
    options.Cookie.HttpOnly = true; // Prevent JavaScript access to the session cookie
    options.Cookie.IsEssential = true; // Ensure the session cookie is created even if the user doesn't consent to non-essential cookies
});

// Configure localization
var supportedCultures = new[] { "en-US", "en-CA" }; // Adjust as needed
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Add localization middleware
app.UseRequestLocalization(localizationOptions);

app.UseRouting();

// Enable session middleware
app.UseAuthorization(); // Adjust middleware order if using authentication
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();