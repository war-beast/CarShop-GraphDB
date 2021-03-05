using CarShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;
using CarShop.BLL.Interfaces;

namespace CarShop.Controllers
{
	public class HomeController : Controller
	{
		#region private members

		private readonly ILogger<HomeController> _logger;
		private readonly IStorageService _storageService;

		#endregion

		public HomeController(ILogger<HomeController> logger, IStorageService storageService)
		{
			_logger = logger;
			_storageService = storageService;
		}

		public async Task<IActionResult> Index() => 
			View(await _storageService.GetBlocks());

		public async Task<IActionResult> Parts() => 
			View(await _storageService.GetParts());

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
