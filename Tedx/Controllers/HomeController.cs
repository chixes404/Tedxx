using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Tedx.Models;

namespace Tedx.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		public IActionResult Index()
		{
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
    }
}
