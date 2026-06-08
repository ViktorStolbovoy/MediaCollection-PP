using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

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
		public async Task<IActionResult> Run([FromBody] PlaybackRequest req)
		{
			if (req == null) return BadRequest();
			var result = await LocationPersistence.GetTitleLocationFull(req.DeviceId, req.TitleId);
			if (result == null) return NotFound();
			if (result.DeviceKind == DeviceType.Local)
				return BadRequest(new { error = "Local playback is available only from MediaCollectionDesktop." });
			await result.Run();
			return Ok();
		}
	}
}
