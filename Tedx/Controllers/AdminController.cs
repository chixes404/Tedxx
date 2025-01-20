using ClosedXML.Excel;
using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tedx.Data;
using X.PagedList;
using Tedx.Models;
using X.PagedList.Extensions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Drawing.Printing;
using Tedx.Helper;
using Microsoft.Extensions.Localization;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;

namespace Tedx.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]

    [Authorize(Roles = "Admin")] // Ensures only users with the "Admin" role can access this controller
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<RegistrationController> _localizer;


        public AdminController(ApplicationDbContext context, IStringLocalizer<RegistrationController> localizer)
        {
            _context = context;
            _localizer = localizer;
        }




        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Home()
        {


            var userId = HttpContext.Session.GetString("UserId");
            var userRole = HttpContext.Session.GetString("UserRole");

            if (string.IsNullOrEmpty(userId) || userRole != "Admin")
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
            ViewBag.HideFooter = true;

            // Fetch dynamic idea categories and their counts from the Users table
            var ideaCategory = await _context.Users
                .Where(u => u.IdeaCategory != null) // Filter out users with no IdeaCategory
                .GroupBy(u => u.IdeaCategory) // Group by the IdeaCategory
                .Select(g => new
                {
                    Category = g.Key, // The category name
                    Count = g.Count() // The count of users in this category
                })
                .ToDictionaryAsync(g => g.Category, g => g.Count); // Convert to a dictionary

            // Ensure all 4 categories are included, even if their count is 0
            var allCategories = new List<string> { "التعليم", "الصحة", "الابتكار", "الاعمال", "اخرى" };
            foreach (var category in allCategories)
            {
                if (!ideaCategory.ContainsKey(category))
                {
                    ideaCategory[category] = 0; // Add the category with a count of 0 if it doesn't exist
                }
            }

            // Pass the dynamic dictionary to the view
            ViewBag.IdeaCategory = ideaCategory;

            // Get the count of listeners and speakers
            var listenersCount = await _context.Users.CountAsync(u => u.RoleAs == "مستمع");
            var speakersCount = await _context.Users.CountAsync(u => u.RoleAs == "متحدث");
            var HasPresentedBeforeCount = await _context.Users.CountAsync(u => u.HasPresentedBefore == true);
            // Pass the counts to the view
            ViewBag.ListenersCount = listenersCount;
            ViewBag.SpeakersCount = speakersCount;
            ViewBag.HasPresentedBeforeCount = HasPresentedBeforeCount;

            return View();
        }




        // Admin Dashboard showing users with "مستمع" role
        public async Task<IActionResult> Listeners(int page = 1, int pageSize = 10)
        {
            ViewBag.HideFooter = true;
            ViewBag.Page = page; // Pass the current page number to the view
            ViewBag.PageSize = pageSize; // Pass the current page size to the view
            try
            {
                var users = await _context.Users
                    .Where(x => x.RoleAs == "مستمع").OrderByDescending(x => x.CreatedAt)
                    .ToListAsync();

                if (users == null || !users.Any())
                {
                    ViewBag.Message = "لا يوجد مستخدمين مستمعين";
                    return View(new List<ApplicationUser>()); // Return an empty list if no users found
                }
                // Paginate the data
                var pagedUsers = users.ToPagedList(page, pageSize); // Don't call .ToList() here
                return View(pagedUsers);
            }
            catch (Exception ex)
            {
                // Log error (use a logger in production)
                Console.WriteLine($"حدث خطأ اثناء استرجاع المستخدمين: {ex.Message}");
                return View("Error");
            }
        }


        // Admin Dashboard showing users with "متحدث" role

        public async Task<IActionResult> Speakers(int page = 1, int pageSize = 10)
        {
            ViewBag.HideFooter = true;
            ViewBag.Page = page; // Pass the current page number to the view
            ViewBag.PageSize = pageSize; // Pass the current page size to the view

            try
            {
                var users = await _context.Users
                    .Where(x => x.RoleAs == "متحدث")
                    .OrderByDescending(x => x.CreatedAt)
                    .ToListAsync();

                if (users == null || !users.Any())
                {
                    ViewBag.Message = "لا يوجد مستخدمين متحدثين";
                    return View(new List<ApplicationUser>()); // Return an empty list if no users found
                }

                var pagedUsers = users.ToPagedList(page, pageSize); // Don't call .ToList() here
                return View(pagedUsers);
            }
            catch (Exception ex)
            {
                // Log error (use a logger in production)
                Console.WriteLine($"حدث خطأ اثناء استرجاع المستخدمين: {ex.Message}");
                return View("Error");
            }
        }



        public async Task<IActionResult> UserDetails(int id)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound(); // Return a 404 error if the user is not found
            }
            var ksaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");
            user.CreatedAt = TimeZoneInfo.ConvertTimeFromUtc(user.CreatedAt, ksaTimeZone);

            return View(user);
        }


        // ConfirmEmail Action
        //public async Task<IActionResult> ConfirmEmail(int userId, string returnUrl)
        //{
        //    // Fetch the user by userId
        //    var user = await _context.Users.FindAsync(userId);

        //    if (user == null)
        //    {
        //        return NotFound(); // Return 404 if the user is not found
        //    }

        //    var subject = "تأكيد التسجيل";
        //    string body = string.Format("\r\nتم تقديم طلبك بنجاح. سيتم إرسال تأكيد الحضور إلى البريد الإلكتروني المسجل\r\n"); // Localized body




        //     EmailHelper.SendEmail(user.Email, subject, body);

        //    // Redirect back to the same page
        //    var referer = Request.Headers["Referer"].ToString();
        //    return Redirect(referer);

        //}

        [HttpPost]
        public async Task<IActionResult> SendEmail(int id)
        {
            // Retrieve the user from the database
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Generate user details for the QR code
            string userDetails = $"الرقم التعريفي:{user.Id}\n, الاسم:{user.FullName}\n, الايميل:{user.Email}\n , ألجوال :{user.Phone}";

            // Generate QR code with the user's ID
            byte[] qrCodeImage = Helper.QrCodeGenerator.GenerateQrCode(userDetails);

            // Save the QR code image to the wwwroot/qrcode folder
            string qrCodeFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "qrcode");
            if (!Directory.Exists(qrCodeFolder))
            {
                Directory.CreateDirectory(qrCodeFolder);
            }

            // Generate a unique file name for the QR code image
            string fileName = $"QRCode_{user.Id}.png";
            string filePath = Path.Combine(qrCodeFolder, fileName);

            // Save the QR code image to the file
            await System.IO.File.WriteAllBytesAsync(filePath, qrCodeImage);

            // Save the QR code path in the database
            user.QRCodeUrl = $"/qrcode/{fileName}";
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            // Send the email with the QR code as an attachment
            var subject = "تأكيد التسجيل";
            var message = $@"
<html>
    <body>
        <p>تم تأكيد الحضور بنجاح </p>
        <p>يرجى الاطلاع على المرفق للحصول على رمز الاستجابة السريعة (QR Code).</p>
    </body>
</html>
";
            bool emailSent = EmailHelper.SendEmail(user.Email, subject, message, qrCodeImage, "QRCode.png", isBodyHtml: true);

            if (!emailSent)
            {
                return StatusCode(500, "Failed to send email.");
            }

            // Redirect back to the referring page
            var referer = Request.Headers["Referer"].ToString();
            if (string.IsNullOrEmpty(referer))
            {
                referer = Url.Action("Listeners", "Admin");
            }
            return Redirect(referer);
        }


        public async Task<IActionResult> SpeakerExportToExcel(int page = 1, int pageSize = 10)
        {
            var users = await _context.Users
                 .Where(x => x.RoleAs == "متحدث")
                .OrderByDescending(x => x.CreatedAt) 
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Users");
                int currentRow = 1;

                // Add Headers
                worksheet.Cell(currentRow, 1).Value = "الاسم";
                worksheet.Cell(currentRow, 2).Value = "الايميل";
                worksheet.Cell(currentRow, 3).Value = "رقم الهاتف";
                worksheet.Cell(currentRow, 4).Value = "العمر";
                worksheet.Cell(currentRow, 5).Value = "الحاله";
                worksheet.Cell(currentRow, 6).Value = "الوظيفه";
                worksheet.Cell(currentRow, 7).Value = "تاريخ التسجيل";
                worksheet.Cell(currentRow, 8).Value = "رمز الاستجابة السريعة (QR Code)";

                // Style headers
                var headerRange = worksheet.Range(1, 1, 1, 8);
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
                headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                worksheet.Row(currentRow).Height = 12; 

                foreach (var user in users)
                {
                    currentRow++;

                    worksheet.Cell(currentRow, 1).Value = user.FullName;
                    worksheet.Cell(currentRow, 2).Value = user.Email;
                    worksheet.Cell(currentRow, 3).Value = user.Phone;
                    worksheet.Cell(currentRow, 4).Value = user.Age;
                    worksheet.Cell(currentRow, 5).Value = user.RoleAs;
                    worksheet.Cell(currentRow, 6).Value = user.Job;
                    worksheet.Cell(currentRow, 7).Value = user.CreatedAt.ToString("yyyy-MM-dd");

                    if (!string.IsNullOrEmpty(user.QRCodeUrl))
                    {
                        string qrCodeFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", user.QRCodeUrl.TrimStart('/'));
                        if (System.IO.File.Exists(qrCodeFilePath))
                        {
                            using (var imageStream = new FileStream(qrCodeFilePath, FileMode.Open, FileAccess.Read))
                            {
                                var image = worksheet.AddPicture(imageStream)
                                    .MoveTo(worksheet.Cell(currentRow, 8)) 
                                    .WithSize(80, 80); 

                                worksheet.Row(currentRow).Height = 60; 
                            }
                        }
                        else
                        {
                            worksheet.Cell(currentRow, 8).Value = "رمز الاستجابة السريعة غير موجود";
                           
                        }
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 8).Value = "لا يوجد رمز استجابة سريعة";
                    }
                }

                worksheet.Column(1).Width = 20; 
                worksheet.Column(2).Width = 25;
                worksheet.Column(3).Width = 15; 
                worksheet.Column(8).Width = 20; 
                worksheet.Columns().AdjustToContents(); 

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Users.xlsx");
                }
            }
        }

        public async Task<IActionResult> ListenerExportToExcel(int page = 1, int pageSize = 10)
        {
            var users = await _context.Users
                .Where(x => x.RoleAs == "مستمع")
                .OrderByDescending(x => x.CreatedAt) 
                .Skip((page - 1) * pageSize) 
                .Take(pageSize)
                .ToListAsync();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Users");
                int currentRow = 1;

                worksheet.Cell(currentRow, 1).Value = "الاسم";
                worksheet.Cell(currentRow, 2).Value = "الايميل";
                worksheet.Cell(currentRow, 3).Value = "رقم الهاتف";
                worksheet.Cell(currentRow, 4).Value = "العمر";
                worksheet.Cell(currentRow, 5).Value = "الحاله";
                worksheet.Cell(currentRow, 6).Value = "الوظيفه";
                worksheet.Cell(currentRow, 7).Value = "تاريخ التسجيل";
                worksheet.Cell(currentRow, 8).Value = "رمز الاستجابة السريعة (QR Code)";

                // Style headers
                var headerRange = worksheet.Range(1, 1, 1, 8);
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
                headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                worksheet.Row(currentRow).Height = 12;

                foreach (var user in users)
                {
                    currentRow++;

                    worksheet.Cell(currentRow, 1).Value = user.FullName;
                    worksheet.Cell(currentRow, 2).Value = user.Email;
                    worksheet.Cell(currentRow, 3).Value = user.Phone;
                    worksheet.Cell(currentRow, 4).Value = user.Age;
                    worksheet.Cell(currentRow, 5).Value = user.RoleAs;
                    worksheet.Cell(currentRow, 6).Value = user.Job;
                    worksheet.Cell(currentRow, 7).Value = user.CreatedAt.ToString("yyyy-MM-dd");

                    if (!string.IsNullOrEmpty(user.QRCodeUrl))
                    {
                        string qrCodeFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", user.QRCodeUrl.TrimStart('/'));
                        if (System.IO.File.Exists(qrCodeFilePath))
                        {
                            using (var imageStream = new FileStream(qrCodeFilePath, FileMode.Open, FileAccess.Read))
                            {
                                var image = worksheet.AddPicture(imageStream)
                                    .MoveTo(worksheet.Cell(currentRow, 8)) 
                                    .WithSize(80, 80); // Resize the image to a consistent size

                                worksheet.Row(currentRow).Height = 60; // Match the image height
                            }
                        }
                        else
                        {
                            worksheet.Cell(currentRow, 8).Value = "رمز الاستجابة السريعة غير موجود";
                        }
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 8).Value = "لا يوجد رمز استجابة سريعة";
                    }
                }

                worksheet.Column(1).Width = 20; // Name
                worksheet.Column(2).Width = 25; // Email
                worksheet.Column(3).Width = 15; // Phone
                worksheet.Column(8).Width = 20; // QR Code
                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Users.xlsx");
                }
            }
        }
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToPage("/Account/Login", new { area = "Identity" });
        }

    }
}
