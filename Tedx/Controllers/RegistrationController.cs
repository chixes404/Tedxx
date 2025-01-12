using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Tedx.Data;
using Tedx.Helper;
using Tedx.Models;

namespace Tedx.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public RegistrationController(ApplicationDbContext context, IStringLocalizer<SharedResources> localizer)
        {
            _context = context;
            _localizer = localizer;
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

                string userDetails = $"الرقم التعريفي:{user.Id}\n, الاسم:{user.FullName}\n, الايميل:{user.Email}\n , ألجوال :{user.Phone}";

                // Generate QR code with the user's ID
                byte[] qrCodeImage = Helper.QrCodeGenerator.GenerateQrCode(userDetails);
                string qrCodeImageBase64 = Convert.ToBase64String(qrCodeImage);

                return Json(new { success = true, qrCode = qrCodeImageBase64 });
            }

            // If ModelState is invalid, return validation errors
            var errors = ModelState.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Errors.Select(e => _localizer[e.ErrorMessage]).FirstOrDefault()
            );

            return Json(new { success = false, errors });
        }

        [HttpGet]
        public IActionResult SetCulture(string culture)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                Response.Cookies.Append(
                    CookieRequestCultureProvider.DefaultCookieName,
                    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                    new CookieOptions { Expires = DateTimeOffset.UtcNow.AddDays(30) }
                );
            }
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}