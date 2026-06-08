using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MediaCollection.Controllers.Api
{
	[ApiController]
	[Route("api/meta")]
	public sealed class MetaApiController : ControllerBase
	{
		[HttpGet("devices-for-title-update")]
		public async Task<IActionResult> DevicesForTitleUpdate()
		{
			return Ok(await DevicePersistense.ListForTitleUpdate());
		}

		[HttpGet("devices-playback")]
		public async Task<IActionResult> DevicesPlayback()
		{
			var all = await DevicePersistense.ListForPalyback();
			var devices = all
				.Where(d => d.Kind != DeviceType.Local)
				.ToList();
			return Ok(devices);
		}

		[HttpGet("locations-for-device/{deviceId:long}")]
		public async Task<IActionResult> LocationsForDevice(long deviceId)
		{
			var bases = new List<LocationBase> { new LocationBase { Id = -1, Kind = LocationBaseKind.Local, Name = "All" } };
			bases.AddRange(await DevicePersistense.GetLocationsForTitleUpdate(deviceId));
			return Ok(bases);
		}
	}
}
