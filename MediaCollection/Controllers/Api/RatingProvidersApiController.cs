using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MediaCollection;

namespace MediaCollection.Controllers.Api
{
	[ApiController]
	[Route("api/rating-providers")]
	public sealed class RatingProvidersApiController : ControllerBase
	{
		[HttpGet]
		public IActionResult List()
		{
			return Ok(GeneralPersistense.FetchAll<RatingProvider>());
		}

		[HttpPost]
		public ActionResult<RatingProvider> Create([FromBody] RatingProvider body)
		{
			if (body == null) return BadRequest();
			body.Id = 0;
			body.RatingName = body.RatingName ?? "New Rating";
			body.Set();
			return Ok(body);
		}

		[HttpPut("{id:long}")]
		public IActionResult Update(long id, [FromBody] RatingProvider patch)
		{
			if (patch == null) return BadRequest();
			var all = GeneralPersistense.FetchAll<RatingProvider>();
			var r = all.FirstOrDefault(x => x.Id == id);
			if (r == null) return NotFound();
			r.RatingName = patch.RatingName ?? r.RatingName;
			r.RatingKind = patch.RatingKind;
			r.RatingMin = patch.RatingMin;
			r.RatingMax = patch.RatingMax;
			r.RatingStep = patch.RatingStep;
			r.Set();
			return Ok(r);
		}

		[HttpDelete("{id:long}")]
		public IActionResult Delete(long id)
		{
			try
			{
				RatingProvider.Delete(id);
				return NoContent();
			}
			catch (System.Exception ex)
			{
				return BadRequest(new { error = ex.Message });
			}
		}
	}
}
