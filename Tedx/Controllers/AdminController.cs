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

namespace Tedx.Controllers
{

    [Authorize(Roles = "Admin")] // Ensures only users with the "Admin" role can access this controller
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Admin Dashboard showing users with "مستمع" role

public async Task<IActionResult> Dashboard(int page = 1, int pageSize = 10)
    {
            ViewBag.HideFooter = true;
            try
        {
            var users = await _context.Users
                .Where(x => x.RoleAs == "مستمع")
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
    public async Task<IActionResult> SpeakerUsers(int page = 1, int pageSize = 10)
        {
            ViewBag.HideFooter = true;
            try
            {
                var users = await _context.Users
                    .Where(x => x.RoleAs == "متحدث")
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
