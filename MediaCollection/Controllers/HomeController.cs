using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace MediaCollection
{
	public class HomeController : Controller
	{
		private readonly IConfiguration _configuration;

		public HomeController(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public IActionResult Index()
		{
			ViewBag.ReadOnly = _configuration.GetValue<bool>("ReadOnly");
			return View();
		}

		public IActionResult Error()
		{
			return View();
		}
	}
}
