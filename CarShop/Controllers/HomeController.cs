using CarShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;
using CarShop.BLL.Interfaces;
using CarShop.BLL.Models;

namespace CarShop.Controllers
{
	public class HomeController : Controller
	{
		#region private members

		private readonly ILogger<HomeController> _logger;
		private readonly IStorageService _storageService;
		private readonly ICommentsService _commentsService;

		#endregion

		public HomeController(ILogger<HomeController> logger, 
			IStorageService storageService, 
			ICommentsService commentsService)
		{
			_logger = logger;
			_storageService = storageService;
			_commentsService = commentsService;
		}

		public async Task<IActionResult> Index() => 
			View(await _storageService.GetBlocks());

		public async Task<IActionResult> Parts() => 
			View(await _storageService.GetParts(string.Empty));

		public async Task<IActionResult> SparePartList(string blockName)
		{
			var model = new PartsSeparatedListViewModel
			{
				BlockName = blockName,
				SpareParts = await _storageService.GetParts(blockName)
			};
			return PartialView("_SparePartList", model);
		}

		public IActionResult Commentaries() =>
			View(_commentsService.GetAll());

		[HttpPost]
		public async Task<IActionResult> AddComment(PostComment newComment)
		{
			var result = await _commentsService.SaveComment(newComment);
			return RedirectToAction("Commentaries");
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
