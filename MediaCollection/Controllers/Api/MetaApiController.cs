using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MediaCollection;

namespace MediaCollection.Controllers.Api
{
	[ApiController]
	[Route("api/meta")]
	public sealed class MetaApiController : ControllerBase
	{
		[HttpGet("title-kinds")]
		public ActionResult<IEnumerable<object>> TitleKinds([FromQuery] string resourceKind = "video")
		{
			IEnumerable<TitleKind> kinds;
			if (string.Equals(resourceKind, "audio", StringComparison.OrdinalIgnoreCase))
				kinds = new[] { TitleKind.AlbumArtist, TitleKind.Album, TitleKind.Track };
			else
				kinds = new[] { TitleKind.Title, TitleKind.Series, TitleKind.Season, TitleKind.Disk, TitleKind.Episode };

			var list = new List<object>();
			foreach (var k in kinds)
				list.Add(new { value = (int)k, name = k.ToString() });
			return Ok(list);
		}

		[HttpGet("devices-for-title-update")]
		public IActionResult DevicesForTitleUpdate()
		{
			return Ok(DevicePersistense.ListForTitleUpdate());
		}

		[HttpGet("devices-playback")]
		public IActionResult DevicesPlayback()
		{
			return Ok(DevicePersistense.ListForPalyback());
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
