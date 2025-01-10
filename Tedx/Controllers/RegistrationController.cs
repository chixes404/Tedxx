using Microsoft.AspNetCore.Mvc;
using Tedx.Data;
using Tedx.Models;

namespace Tedx.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RegistrationController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create([Bind("FullName,Age,RoleAs,Job,Email,Phone,IdeaCategory,IdeaDescription,WhyIdea,HasPresentedBefore")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Users.Add(user);
                _context.SaveChanges();

                // Return a JSON response indicating success
                return Json(new { success = true });
            }

            // If ModelState is invalid, return validation errors
            var errors = ModelState.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).FirstOrDefault()
            );

            return Json(new { success = false, errors });
        }
    }
}