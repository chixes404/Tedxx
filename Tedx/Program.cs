using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Tedx.Data;
using Tedx.Models;
using Tedx.Helper;
using Humanizer.Localisation;
using Tedx.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Configure Database Context
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Configure Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true; // Make cookie accessible only via HTTP (not JavaScript)
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // Cookie expiry time
    options.LoginPath = "/identity/Account/Login"; // Redirect path for login
    options.AccessDeniedPath = "/Account/AccessDenied"; // Redirect path for access denied
    options.SlidingExpiration = true; // Extend expiration on user activity
});

// Configure Razor Pages (for Identity UI)
builder.Services.AddRazorPages();

// Add Localization Services
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");


// Configure Supported Cultures
var supportedCultures = new[] { "en-US", "ar-SA" }; // Add your supported cultures
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0]) // Default culture
    .AddSupportedCultures(supportedCultures) // Supported cultures for date, number, etc.
    .AddSupportedUICultures(supportedCultures); // Supported UI cultures for resource files

// Add Controllers and Views
builder.Services.AddControllersWithViews()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization();


var app = builder.Build();
var localizer = app.Services.GetRequiredService<IStringLocalizer<RegistrationController>>();
EmailHelper.Initialize(app.Configuration, localizer);



// Seed data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await Seeding.Initialize(services);
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Show detailed error pages in development
}
else
{
    app.UseExceptionHandler("/Home/Error"); // Redirect to the Error action in production
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRequestLocalization(localizationOptions);
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
// Default route for /register
app.MapControllerRoute(
    name: "register",
    pattern: "register",
    defaults: new { controller = "Registration", action = "Create" });

// Fallback route for all other routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Registration}/{action=Create}/{id?}");
app.MapRazorPages();

app.Run();
