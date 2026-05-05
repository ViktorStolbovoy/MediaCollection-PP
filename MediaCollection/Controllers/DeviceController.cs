using System;
using Microsoft.AspNetCore.Mvc;

namespace MediaCollection
{
	public class DeviceController : MCControllerBase
	{
		public IActionResult Index()
		{
			return View("~/Views/Devices.cshtml", SetRequest.FromPoco(DevicePersistense.List()));
		}

		[HttpPost]
		public IActionResult Set([FromBody] SetRequest rq)
		{
			if (rq == null) throw new ArgumentNullException(nameof(rq));
			rq.Persist<Device>(Device.Delete);
			return Json(SetRequest.FromPoco(DevicePersistense.List()));
		}

		public IActionResult ListLocations(long id)
		{
			return Json(SetRequest.FromPoco(DevicePersistense.GetLocations(id)));
		}

		[HttpPost]
		public IActionResult UpdateLocations([FromBody] SetRequest rq)
		{
			if (rq == null) throw new ArgumentNullException(nameof(rq));
			if (!rq.ParentId.HasValue) throw new ArgumentException("Request should have parentId");
			long pid = rq.ParentId.Value;
			if (pid <= 0) throw new ArgumentException("parentId should be positive");
			if (rq.Edits != null)
			{
				foreach (var e in rq.ToPoco<LocationBaseDeviceMapping>())
				{
					var data = new DeviceLocationMap { DeviceId = pid, LocationBaseId = e.LocationBaseId, Mapping = e.Mapping };
					GeneralPersistense.Upsert(data);
				}
			}
			return ListLocations(pid);
		}
	}
}
