using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MediaCollection
{
	public class TitleController : MCControllerBase
    {
        //
        // GET: /Title/

        public ActionResult Index()
        {
            return View();
        }


		public ActionResult GetLocations(long id)
		{
			var res = LocationPersistence.ListTitleLocations(id);
			return Json(res, JsonRequestBehavior.AllowGet);
		}

		public ActionResult UpdateTitles()
		{
			var devices = DevicePersistense.ListForTitleUpdate();
			return View("~/Views/UpdateTitles.cshtml", devices);
		}

		public ActionResult GetLocationBases(long deviceid)
		{
			return Json(DevicePersistense.GetLocationsForTitleUpdate(deviceid), JsonRequestBehavior.AllowGet);
		}

    }
}
