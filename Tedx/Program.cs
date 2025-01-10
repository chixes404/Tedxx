using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Tedx.Data;
using Tedx.Models;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(connectionString));

// Add Identity with ApplicationUser		
builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
	.AddEntityFrameworkStores<ApplicationDbContext>()
	.AddDefaultTokenProviders();


builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true; // Makes the cookie accessible only via HTTP (not JavaScript)
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // Expiry time for the authentication cookie
    options.LoginPath = "/identity/Account/Login"; // Redirect here if not authenticated
    options.AccessDeniedPath = "/Account/AccessDenied"; // Redirect here if access is denied
    options.SlidingExpiration = true; // Extends expiration on user activity
});


// Add Razor Pages (for Identity UI)
builder.Services.AddRazorPages();

// Add MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Seed data
using (var scope = app.Services.CreateScope())
{
	var services = scope.ServiceProvider;
	await Seeding.Initialize(services);
}

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles(); // Enable serving static files

app.UseDefaultFiles(new DefaultFilesOptions
{
    DefaultFileNames = new List<string> { "index.html" }
});


app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Registration}/{action=Create}/{id?}");

app.MapRazorPages(); // Add this for Identity pages

app.Run();