using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace MediaCollection
{
	public class DeviceController : MCControllerBase
	{
		//
		// GET: /Device/
		

		public ActionResult Index()
		{
			return View("~/Views/Devices.cshtml", SetRequest.FromPoco(DevicePersistense.List()));
		}
						
		public ActionResult Set(SetRequest rq)
		{
			if (rq == null) throw new ArgumentNullException("rq");
			rq.Persist<Device>(Device.Delete);
			return Json(SetRequest.FromPoco(DevicePersistense.List()), JsonRequestBehavior.AllowGet);
		}

		public ActionResult ListLocations(long id)
		{
			return Json(SetRequest.FromPoco(DevicePersistense.GetLocations(id)), JsonRequestBehavior.AllowGet);
		}


		public ActionResult UpdateLocations(SetRequest rq)
		{
			if (rq == null) throw new ArgumentNullException("rq");
			if (!rq.ParentId.HasValue) throw new ArgumentException("Request should have parentId");
			long pid = rq.ParentId.Value;
			if (pid <= 0) throw new ArgumentException("parentId should be positive");
			if (rq.Edits != null)
			{
				foreach (var e in rq.ToPoco<LocationBaseDeviceMapping>())
				{
					var data = new DeviceLocationMap {DeviceId = pid, LocationBaseId = e.LocationBaseId, Mapping = e.Mapping };
					GeneralPersistense.Upsert(data); 
				}
			}
			return ListLocations(pid);
		}
	}
}
