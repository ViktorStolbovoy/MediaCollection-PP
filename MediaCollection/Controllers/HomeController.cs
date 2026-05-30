using Microsoft.AspNetCore.Mvc;

namespace MediaCollection
{
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			ViewBag.ReadOnly = !(User?.Identity?.IsAuthenticated ?? false);
			return View();
		}

		public IActionResult Error()
		{
			return View();
		}
	}
}
