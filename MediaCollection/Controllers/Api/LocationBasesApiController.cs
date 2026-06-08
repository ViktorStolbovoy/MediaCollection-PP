using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MediaCollection.Controllers.Api
{
	[ApiController]
	[Route("api/location-bases")]
	public sealed class LocationBasesApiController : ControllerBase
	{
		[HttpGet]
		public async Task<IActionResult> List()
		{
			return Ok(await LocationPersistence.ListBases());
		}

		[HttpPost]
		public async Task<ActionResult<LocationBase>> Create([FromBody] LocationBase body)
		{
			if (body == null) return BadRequest();
			body.Id = 0;
			body.Name = body.Name ?? "";
			await body.Set();
			return Ok(body);
		}

		[HttpPut("{id:long}")]
		public async Task<IActionResult> Update(long id, [FromBody] LocationBase patch)
		{
			if (patch == null) return BadRequest();
			var list = await LocationPersistence.ListBases();
			var b = list.FirstOrDefault(x => x.Id == id);
			if (b == null) return NotFound();
			b.Name = patch.Name ?? b.Name;
			b.Kind = patch.Kind;
			await b.Set();
			return Ok(b);
		}

		[HttpDelete("{id:long}")]
		public async Task<IActionResult> Delete(long id)
		{
			try
			{
				await LocationBase.Delete(id);
				return NoContent();
			}
			catch (System.Exception ex)
			{
				return BadRequest(new { error = ex.Message });
			}
		}
	}
}
