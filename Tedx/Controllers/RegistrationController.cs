using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Text.RegularExpressions;
using Tedx.Data;
using Tedx.Helper;
using Tedx.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Tedx.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<RegistrationController> _localizer;

        public RegistrationController(ApplicationDbContext context, IStringLocalizer<RegistrationController> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create([Bind("FullName,Age,RoleAs,Job,Email,Phone,ListenAboutEvent , HasChildIn, IdeaCategory,IdeaDescription,WhyIdea,HasPresentedBefore")] User user)
        {
            if (ModelState.IsValid)
            {


                // Normalize Arabic numbers to English numbers (if any)
                user.Age = NormalizeNumbers(user.Age.ToString());

                // Validate age range
                if (!int.TryParse(user.Age, out int age) || age < 0 || age > 150)
                {
                    return Json(new { success = false, errors = new Dictionary<string, string> { { "Age", _localizer["AgeInvalid"].Value } } });
                }

                // Check if email already exists
                var existingEmail = _context.Users.Any(u => u.Email == user.Email);
                if (existingEmail)
                {
                    return Json(new { success = false, errors = new Dictionary<string, string> { { "Email", _localizer["EmailAlreadyExists"].Value } } });
                }

                // Check if phone number already exists
                var existingPhone = _context.Users.Any(u => u.Phone == user.Phone);
                if (existingPhone)
                {
                    return Json(new { success = false, errors = new Dictionary<string, string> { { "Phone", _localizer["PhoneAlreadyExists"].Value } } });
                }

                // Add the user to the database
                _context.Users.Add(user);
                _context.SaveChanges();
             

                //// Generate user details for the QR code
                //string userDetails = $"الرقم التعريفي:{user.Id}\n, الاسم:{user.FullName}\n, الايميل:{user.Email}\n , ألجوال :{user.Phone}";

                //// Generate QR code with the user's ID
                //byte[] qrCodeImage = Helper.QrCodeGenerator.GenerateQrCode(userDetails);
                //string qrCodeImageBase64 = Convert.ToBase64String(qrCodeImage);

                //string subject = _localizer["EmailSubject"];
                //string body = string.Format(_localizer["EmailBody"], user.FullName);
                //EmailHelper.SendEmail(user.Email, subject, body);


                return Json(new { success = true});
            
            }

            // If ModelState is invalid, return validation errors
            var errors = ModelState.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Errors.Select(e => _localizer[e.ErrorMessage].Value).FirstOrDefault() ?? string.Empty // Ensure no null values
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

        // Helper method to normalize Arabic numbers to English numbers
        private string NormalizeNumbers(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            // Map Arabic numerals to English numerals
            var arabicToEnglishMap = new Dictionary<char, char>
            {
                {'٠', '0'}, {'١', '1'}, {'٢', '2'}, {'٣', '3'}, {'٤', '4'},
                {'٥', '5'}, {'٦', '6'}, {'٧', '7'}, {'٨', '8'}, {'٩', '9'}
            };

            // Convert each Arabic numeral to English
            var normalizedInput = new System.Text.StringBuilder();
            foreach (var c in input)
            {
                normalizedInput.Append(arabicToEnglishMap.ContainsKey(c) ? arabicToEnglishMap[c] : c);
            }

            return normalizedInput.ToString();
        }
    }
}