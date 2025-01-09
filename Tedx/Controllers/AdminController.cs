using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tedx.Data;

namespace Tedx.Controllers
{
  
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task <IActionResult> Dashboard()
        {
            var users = await _context.Users.Where(x=>x.RoleAs== "مستمع").ToListAsync(); 
            return View(users);
        }

        public async Task <IActionResult> SpeakerUsers()
        {
            var users = await  _context.Users.Where(x => x.RoleAs == "متحدث").ToListAsync();
            return View(users);
        }


    }
}
