﻿using ClosedXML.Excel;
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

namespace Tedx.Controllers
{

    [Authorize(Roles = "Admin")] // Ensures only users with the "Admin" role can access this controller
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<RegistrationController> _localizer;


        public AdminController(ApplicationDbContext context , IStringLocalizer<RegistrationController> localizer)
        {
            _context = context;
            _localizer = localizer;
        }





        public async Task<IActionResult> Home()
        {
            // Get the count of listeners and speakers
            var listenersCount = await _context.Users.CountAsync(u => u.RoleAs == "مستمع");
            var speakersCount = await _context.Users.CountAsync(u => u.RoleAs == "متحدث");

            // Pass the counts to the view
            ViewBag.ListenersCount = listenersCount;
            ViewBag.SpeakersCount = speakersCount;

            return View();
        }




        // Admin Dashboard showing users with "مستمع" role
        public async Task<IActionResult> Listeners(int page = 1, int pageSize = 10)
    {
            ViewBag.HideFooter = true;
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
            try
            {
                var users = await _context.Users
                    .Where(x => x.RoleAs == "متحدث").OrderByDescending(x=>x.CreatedAt)
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

            return View(user);
        }


        // ConfirmEmail Action
        public async Task<IActionResult> ConfirmEmail(int userId, string returnUrl)
        {
            // Fetch the user by userId
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return NotFound(); // Return 404 if the user is not found
            }

            var subject = "تأكيد التسجيل";
            string body = string.Format("\r\nتم تقديم طلبك بنجاح. سيتم إرسال تأكيد الحضور إلى البريد الإلكتروني المسجل\r\n"); // Localized body


         

             EmailHelper.SendEmail(user.Email, subject, body);

            // Redirect back to the same page
            var referer = Request.Headers["Referer"].ToString();
            return Redirect(referer);

        }


        [HttpPost]
        public async Task<IActionResult> SendEmail(int id)
        {
            // Retrieve the user from the database
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Send an email to the user
            var subject = "تأكيد التسجيل";
            var message = "\r\nتم تقديم طلبك بنجاح. سيتم إرسال تأكيد الحضور إلى البريد الإلكتروني المسجل\r\n";
             EmailHelper.SendEmail(user.Email, subject, message);

            // Redirect back to the referring page
            var referer = Request.Headers["Referer"].ToString();
            if (string.IsNullOrEmpty(referer))
            {
                referer = Url.Action("Listeners", "Admin"); // Fallback to a default page
            }
            return Redirect(referer);
        }

        public async Task<IActionResult> SpeakerExportToExcel(int page = 1, int pageSize = 10)
        {
            var users = await _context.Users
                           .Where(x => x.RoleAs == "متحدث")
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
                worksheet.Cell(currentRow, 7).Value = "حضر من قبل";
                worksheet.Cell(currentRow, 8).Value = "التصنيف";
                worksheet.Cell(currentRow, 9).Value = "وصف الفكره";
                worksheet.Cell(currentRow, 10).Value = "تاريخ التسجيل";

                // Add Data
                foreach (var user in users)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = user.FullName;
                    worksheet.Cell(currentRow, 2).Value = user.Email;
                    worksheet.Cell(currentRow, 3).Value = user.Phone;
                    worksheet.Cell(currentRow, 4).Value = user.Age;
                    worksheet.Cell(currentRow, 5).Value = user.RoleAs;
                    worksheet.Cell(currentRow, 6).Value = user.Job;
                    worksheet.Cell(currentRow, 7).Value = (bool)user.HasPresentedBefore ? "نعم" : "لا";
                    worksheet.Cell(currentRow, 8).Value = user.IdeaCategory;
                    worksheet.Cell(currentRow, 9).Value = user.IdeaDescription;
                    worksheet.Cell(currentRow, 10).Value = user.CreatedAt.ToString("yyyy-MM-dd");
                }

                // Prepare Excel file for download
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

                // Add Data
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
                }

                // Prepare Excel file for download
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Users.xlsx");
                }
            }
        }


    }
}
