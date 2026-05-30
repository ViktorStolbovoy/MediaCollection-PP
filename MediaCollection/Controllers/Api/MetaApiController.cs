using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace MediaCollection.Controllers.Api
{
	[ApiController]
	[Route("api/meta")]
	public sealed class MetaApiController : ControllerBase
	{
		[HttpGet("devices-for-title-update")]
		public IActionResult DevicesForTitleUpdate()
		{
			return Ok(DevicePersistense.ListForTitleUpdate());
		}

		[HttpGet("devices-playback")]
		public IActionResult DevicesPlayback()
		{
			var devices = DevicePersistense.ListForPalyback()
				.Where(d => d.Kind != DeviceType.Local)
				.ToList();
			return Ok(devices);
		}

		[HttpGet("locations-for-device/{deviceId:long}")]
		public IActionResult LocationsForDevice(long deviceId)
		{
			var bases = new List<LocationBase> { new LocationBase { Id = -1, Kind = LocationBaseKind.Local, Name = "All" } };
			bases.AddRange(DevicePersistense.GetLocationsForTitleUpdate(deviceId));
			return Ok(bases);
		}
	}
}
