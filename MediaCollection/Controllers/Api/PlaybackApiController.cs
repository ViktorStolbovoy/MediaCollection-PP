using Microsoft.AspNetCore.Mvc;
using MediaCollection;

namespace MediaCollection.Controllers.Api
{
	[ApiController]
	[Route("api/playback")]
	public sealed class PlaybackApiController : ControllerBase
	{
		public sealed class PlaybackRequest
		{
			public long DeviceId { get; set; }
			public long TitleId { get; set; }
		}

		[HttpPost("run")]
		public IActionResult Run([FromBody] PlaybackRequest req)
		{
			if (req == null) return BadRequest();
			var result = LocationPersistence.GetTitleLocationFull(req.DeviceId, req.TitleId);
			if (result == null) return NotFound();
			if (result.DeviceKind == DeviceType.Local)
				return BadRequest(new { error = "Local playback is available only from MediaCollectionDesktop." });
			result.Run();
			return Ok();
		}
	}
}
