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

        // GET: Registration/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Registration/Create
        [HttpPost]
        public IActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                // Save the user to the database
                _context.Users.Add(user);
                _context.SaveChanges();

                return Json(new { success = true });
            }

            // If the model is invalid, return validation errors
            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
        }
    }
}