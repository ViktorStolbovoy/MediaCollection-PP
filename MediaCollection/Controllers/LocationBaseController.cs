using System;
using Microsoft.AspNetCore.Mvc;

namespace MediaCollection
{
	public class LocationBaseController : MCControllerBase
	{
		public IActionResult Index()
		{
			return View("~/Views/LocationBases.cshtml", SetRequest.FromPoco(LocationPersistence.ListBases()));
		}

		[HttpPost]
		public IActionResult Set([FromBody] SetRequest rq)
		{
			if (rq == null) throw new ArgumentNullException(nameof(rq));
			rq.Persist<LocationBase>(LocationBase.Delete);
			return Json(SetRequest.FromPoco(LocationPersistence.ListBases()));
		}

		public IActionResult GetForDevice(int id)
		{
			var res = LocationPersistence.GetLocationsForBase(id);
			return Json(res);
		}

		public IActionResult GetLocations(int id)
		{
			var res = LocationPersistence.GetLocationsForBase(id);
			return Json(res);
		}

		public IActionResult GetLocationBaseTitles(long deviceId, long locationBaseId)
		{
			return new EmptyResult();
		}
	}
}
