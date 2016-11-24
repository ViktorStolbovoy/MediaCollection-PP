using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MediaCollection
{
	public class LocationBaseController : MCControllerBase
	{
		//
		// GET: /Location/

		public ActionResult Index()
		{
			return View("~/Views/LocationBases.cshtml", SetRequest.FromPoco(LocationPersistence.ListBases()));
		}

		public ActionResult Set(SetRequest rq)
		{
			if (rq == null) throw new ArgumentNullException("rq");
			rq.Persist<LocationBase>(LocationBase.Delete);
			return Json(SetRequest.FromPoco(LocationPersistence.ListBases()), JsonRequestBehavior.AllowGet);
		}


		public ActionResult GetForDevice(int id)
		{
			var res = LocationPersistence.GetLocationsForBase(id);
			return Json(res, JsonRequestBehavior.AllowGet);
		}

		public ActionResult GetLocations(int id)
		{
			var res = LocationPersistence.GetLocationsForBase(id);
			return Json(res, JsonRequestBehavior.AllowGet);
		}

		public ActionResult GetLocationBaseTitles(long deviceId, long locationBaseId)
		{
			return null;
		}
	}
}
