using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MediaCollection.Controllers.Api
{
	[ApiController]
	[Route("api/rating-providers")]
	public sealed class RatingProvidersApiController : ControllerBase
	{
		[HttpGet]
		public async Task<IActionResult> List()
		{
			return Ok(await GeneralPersistense.FetchAll<RatingProvider>());
		}

		[HttpPost]
		public async Task<ActionResult<RatingProvider>> Create([FromBody] RatingProvider body)
		{
			if (body == null) return BadRequest();
			body.Id = 0;
			body.RatingName = body.RatingName ?? "New Rating";
			await body.Set();
			return Ok(body);
		}

		[HttpPut("{id:long}")]
		public async Task<IActionResult> Update(long id, [FromBody] RatingProvider patch)
		{
			if (patch == null) return BadRequest();
			var all = await GeneralPersistense.FetchAll<RatingProvider>();
			var r = all.FirstOrDefault(x => x.Id == id);
			if (r == null) return NotFound();
			r.RatingName = patch.RatingName ?? r.RatingName;
			r.RatingKind = patch.RatingKind;
			r.RatingMin = patch.RatingMin;
			r.RatingMax = patch.RatingMax;
			r.RatingStep = patch.RatingStep;
			await r.Set();
			return Ok(r);
		}

		[HttpDelete("{id:long}")]
		public async Task<IActionResult> Delete(long id)
		{
			try
			{
				await RatingProvider.Delete(id);
				return NoContent();
			}
			catch (System.Exception ex)
			{
				return BadRequest(new { error = ex.Message });
			}
		}
	}
}
