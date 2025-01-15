using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Diagnostics;
using Tedx.Models;

namespace Tedx.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
        private readonly IStringLocalizer<HomeController> _localizer;

        public HomeController(ILogger<HomeController> logger , IStringLocalizer<HomeController> localizer)
		{
			_logger = logger;
            _localizer = localizer;
		}

		public IActionResult Index()
		{
            // Pass localized strings to the view
            ViewBag.Date = _localizer["Date"];
            ViewBag.AttendButton = _localizer["AttendButton"];
            ViewBag.DaysLabel = _localizer["DaysLabel"];
            ViewBag.HoursLabel = _localizer["HoursLabel"];
            ViewBag.MinutesLabel = _localizer["MinutesLabel"];
            ViewBag.SecondsLabel = _localizer["SecondsLabel"];
            return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            // Create an instance of ErrorViewModel
            var errorViewModel = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier // Set the Request ID
            };

            // Log the error (optional)
            _logger.LogError("An error occurred. Request ID: {RequestId}", errorViewModel.RequestId);

            // Pass the model to the view
            return View(errorViewModel);
        }


        [HttpPost]
        public IActionResult SetCulture(string culture)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect("~/");
        }

    }
}
