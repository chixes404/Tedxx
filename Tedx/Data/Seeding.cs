using Microsoft.AspNetCore.Identity;
using Tedx.Models;

namespace Tedx.Data
{
	public class Seeding
	{
		public static async Task Initialize(IServiceProvider serviceProvider)
		{
			var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
			var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

			// Seed roles
			string[] roleNames = { "Admin", "User" };
			foreach (var roleName in roleNames)
			{
				if (!await roleManager.RoleExistsAsync(roleName))
				{
					await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
				}
			}

			// Seed admin user
			var adminUser = new ApplicationUser
			{
				UserName = "admin@tedx.com",
				Email = "admin@tedx.com"
			};

			string adminPassword = "Admin@123";
			var user = await userManager.FindByEmailAsync(adminUser.Email);

			if (user == null)
			{
				var createUser = await userManager.CreateAsync(adminUser, adminPassword);
				if (createUser.Succeeded)
				{
					await userManager.AddToRoleAsync(adminUser, "Admin");
				}
			}
		}
	}
}