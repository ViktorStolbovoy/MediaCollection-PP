using Microsoft.AspNetCore.Mvc;

namespace MediaCollection
{
	public class TitleController : MCControllerBase
	{
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult GetLocations(long id)
		{
			var res = LocationPersistence.ListTitleLocations(id);
			return Json(res);
		}

		public IActionResult UpdateTitles()
		{
			var devices = DevicePersistense.ListForTitleUpdate();
			return View("~/Views/UpdateTitles.cshtml", devices);
		}

		public IActionResult GetLocationBases(long deviceId)
		{
			return Json(DevicePersistense.GetLocationsForTitleUpdate(deviceId));
		}
	}
}
