using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MediaCollection;

namespace MediaCollection.Controllers.Api
{
	[ApiController]
	[Route("api/location-bases")]
	public sealed class LocationBasesApiController : ControllerBase
	{
		[HttpGet]
		public IActionResult List()
		{
			return Ok(LocationPersistence.ListBases());
		}

		[HttpPost]
		public ActionResult<LocationBase> Create([FromBody] LocationBase body)
		{
			if (body == null) return BadRequest();
			body.Id = 0;
			body.Name = body.Name ?? "";
			body.Set();
			return Ok(body);
		}

		[HttpPut("{id:long}")]
		public IActionResult Update(long id, [FromBody] LocationBase patch)
		{
			if (patch == null) return BadRequest();
			var list = LocationPersistence.ListBases();
			var b = list.FirstOrDefault(x => x.Id == id);
			if (b == null) return NotFound();
			b.Name = patch.Name ?? b.Name;
			b.Kind = patch.Kind;
			b.Set();
			return Ok(b);
		}

		[HttpDelete("{id:long}")]
		public IActionResult Delete(long id)
		{
			try
			{
				LocationBase.Delete(id);
				return NoContent();
			}
			catch (System.Exception ex)
			{
				return BadRequest(new { error = ex.Message });
			}
		}
	}
}
