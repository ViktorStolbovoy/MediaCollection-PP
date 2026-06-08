using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPoco;

namespace MediaCollection.Controllers.Api
{
	[ApiController]
	[Route("api/titles")]
	public sealed class TitlesApiController : ControllerBase
	{
		private static async Task<Title> FetchTitle(IDatabase db, long id)
		{
			var rows = await db.FetchAsync<Title>("WHERE TITLE_ID = @0", id);
			return rows.FirstOrDefault();
		}

		[HttpGet("roots")]
		public async Task<ActionResult<IEnumerable<Title>>> GetRoots([FromQuery] string resourceKind = "video", [FromQuery] bool includeHidden = false)
		{
			List<Title> list;
			if (string.Equals(resourceKind, "audio", StringComparison.OrdinalIgnoreCase))
				list = await TitlePersistence.ListRootAudio(includeHidden);
			else
				list = await TitlePersistence.ListRootVideo(includeHidden);
			list.Sort();
			return Ok(list);
		}

		[HttpGet("{parentId:long}/children")]
		public async Task<ActionResult<IEnumerable<Title>>> GetChildren(long parentId)
		{
			var list = await TitlePersistence.ListTitlesByParent(parentId);
			list.Sort();
			return Ok(list);
		}

		[HttpGet("{id:long}", Name = "TitleDetail")]
		public async Task<ActionResult<TitleDetailDto>> GetDetail(long id)
		{
			using var db = DB.GetDatabase();
			var title = await FetchTitle(db, id);
			if (title == null) return NotFound();
			var dto = new TitleDetailDto
			{
				Title = title,
				Locations = await LocationPersistence.ListTitleLocations(id),
				Ratings = await TitlePersistence.GetRatings(id),
				Images = await MediaSamplePersistence.GetSamples(id, MediaSampleKind.Image)
			};
			return Ok(dto);
		}

		[HttpPut("{id:long}")]
		public async Task<IActionResult> Update(long id, [FromBody] TitleUpdateDto body)
		{
			if (body == null) return BadRequest();
			using var db = DB.GetDatabase();
			var title = await FetchTitle(db, id);
			if (title == null) return NotFound();
			title.TitleName = body.TitleName ?? title.TitleName;
			title.Kind = body.Kind;
			title.Year = body.Year;
			title.Description = body.Description ?? "";
			title.ImdbId = body.ImdbId ?? "";
			title.Season = body.Season;
			title.Disk = body.Disk;
			title.EpisodeOrTrack = body.EpisodeOrTrack;
			title.DateModifiedUtc = GeneralPersistense.GetTimestamp();
			await GeneralPersistense.Upsert(title);
			return Ok(title);
		}

		[HttpPost]
		public async Task<ActionResult<Title>> Create([FromBody] TitleCreateDto body)
		{
			if (body == null || string.IsNullOrWhiteSpace(body.TitleName)) return BadRequest();
			var now = GeneralPersistense.GetTimestamp();
			var t = await TitlePersistence.AddTitle(body.TitleName.Trim(), body.Kind, body.Season, body.Disk, body.EpisodeOrTrack, body.ParentTitleId);
			return CreatedAtAction("TitleDetail", new { id = t.Id }, t);
		}

		[HttpDelete("{id:long}")]
		public async Task<IActionResult> Delete(long id)
		{
			try
			{
				await TitlePersistence.DeleteTitle(id);
				return NoContent();
			}
			catch (Exception ex)
			{
				return BadRequest(new { error = ex.Message });
			}
		}

		[HttpPost("{id:long}/toggle-hidden")]
		public async Task<IActionResult> ToggleHidden(long id)
		{
			using var db = DB.GetDatabase();
			var title = await FetchTitle(db, id);
			if (title == null) return NotFound();
			var hidden = !title.Hidden;
			await TitlePersistence.SetHidden(id, hidden);
			title.Hidden = hidden;
			return Ok(title);
		}

		[HttpPost("{id:long}/move")]
		public async Task<ActionResult<MoveTitleResponse>> Move(long id, [FromBody] MoveTitleRequest body)
		{
			if (body == null) return BadRequest();
			var outcome = await TitleHierarchyService.TryMove(id, body.ParentId);
			if (outcome.Error != null) return BadRequest(new { error = outcome.Error });

			var affectedParentIds = new HashSet<long> { outcome.DestinationParentId };
			if (outcome.SourceParentId.HasValue)
				affectedParentIds.Add(outcome.SourceParentId.Value);
			if (outcome.DropTargetParentId != outcome.DestinationParentId)
				affectedParentIds.Add(outcome.DropTargetParentId);

			var refreshedParents = new List<MoveTitleParentChildren>();
			foreach (var parentId in affectedParentIds)
			{
				var children = await TitlePersistence.ListTitlesByParent(parentId);
				refreshedParents.Add(new MoveTitleParentChildren { ParentId = parentId, Children = children });
			}

			return Ok(new MoveTitleResponse
			{
				Title = outcome.Title,
				RefreshedParents = refreshedParents
			});
		}

		[HttpPut("{titleId:long}/locations/{locationId:long}")]
		public async Task<IActionResult> UpdateLocation(long titleId, long locationId, [FromBody] LocationPatchDto body)
		{
			if (body == null) return BadRequest();
			using var db = DB.GetDatabase();
			var loc = (await db.FetchAsync<Location>("WHERE LOCATION_ID = @0 AND TITLE_ID = @1", locationId, titleId)).FirstOrDefault();
			if (loc == null) return NotFound();
			if (body.LocationBaseId.HasValue) loc.LocationBaseId = body.LocationBaseId.Value;
			if (body.LocationData != null) loc.LocationData = body.LocationData;
			loc.DateModifiedUtc = GeneralPersistense.GetTimestamp();
			await GeneralPersistense.Upsert(loc);
			return Ok(loc);
		}

		[HttpPost("{titleId:long}/locations")]
		public async Task<ActionResult<Location>> AddLocation(long titleId, [FromBody] LocationCreateDto body)
		{
			if (body == null) return BadRequest();
			using var db = DB.GetDatabase();
			if (await FetchTitle(db, titleId) == null) return NotFound();
			var now = GeneralPersistense.GetTimestamp();
			var loc = new Location
			{
				TitleId = titleId,
				LocationBaseId = body.LocationBaseId,
				LocationData = body.LocationData ?? "",
				MediaKind = body.MediaKind,
				DateAddedUtc = now,
				DateModifiedUtc = now
			};
			await GeneralPersistense.Upsert(loc);
			return CreatedAtAction("TitleDetail", new { id = titleId }, loc);
		}

		[HttpDelete("{titleId:long}/locations/{locationId:long}")]
		public async Task<IActionResult> DeleteLocation(long titleId, long locationId)
		{
			using var db = DB.GetDatabase();
			var loc = (await db.FetchAsync<Location>("WHERE LOCATION_ID = @0 AND TITLE_ID = @1", locationId, titleId)).FirstOrDefault();
			if (loc == null) return NotFound();
			await loc.Delete();
			return NoContent();
		}

		[HttpPut("{titleId:long}/ratings")]
		public async Task<IActionResult> PutRatings(long titleId, [FromBody] List<RatingValueDto> ratings)
		{
			if (ratings == null) return BadRequest();
			using var db = DB.GetDatabase();
			if (await FetchTitle(db, titleId) == null) return NotFound();
			foreach (var rv in ratings)
			{
				var current = await TitlePersistence.GetRatings(titleId);
				var existing = current.FirstOrDefault(r => r.RatingId == rv.RatingId);
				if (existing == null)
				{
					var tw = new TitleRatingWithName
					{
						RatingId = rv.RatingId,
						RatingValue = rv.Value,
						TitleId = 0
					};
					await tw.Set(titleId);
				}
				else
				{
					existing.RatingValue = rv.Value;
					await existing.Set(titleId);
				}
			}
			return Ok(await TitlePersistence.GetRatings(titleId));
		}

		[HttpGet("{titleId:long}/images/{sampleId:long}/file")]
		public async Task<IActionResult> GetImageFile(long titleId, long sampleId)
		{
			var list = await MediaSamplePersistence.GetSamples(titleId, MediaSampleKind.Image);
			var sample = list.FirstOrDefault(s => s.Id == sampleId);
			if (sample == null) return NotFound();
			var stream = sample.GetData();
			var ext = (sample.Extension ?? "").TrimStart('.').ToLowerInvariant();
			var contentType = ext switch
			{
				"png" => "image/png",
				"gif" => "image/gif",
				"bmp" => "image/bmp",
				"webp" => "image/webp",
				_ => "image/jpeg"
			};
			return File(stream, contentType, enableRangeProcessing: true);
		}

		[HttpPost("{titleId:long}/images")]
		[RequestSizeLimit(52_428_800)]
		public async Task<IActionResult> UploadImage(long titleId, IFormFile file)
		{
			if (file == null || file.Length == 0) return BadRequest();
			using var db = DB.GetDatabase();
			if (await FetchTitle(db, titleId) == null) return NotFound();
			using var ms = new MemoryStream();
			file.CopyTo(ms);
			var ext = Path.GetExtension(file.FileName) ?? ".jpg";
			var sample = await MediaSamplePersistence.AddSample(ms.ToArray(), titleId, MediaSampleKind.Image, ext);
			return Ok(sample);
		}

		[HttpDelete("{titleId:long}/images/{sampleId:long}")]
		public async Task<IActionResult> DeleteImage(long titleId, long sampleId)
		{
			var list = await MediaSamplePersistence.GetSamples(titleId, MediaSampleKind.Image);
			var sample = list.FirstOrDefault(s => s.Id == sampleId);
			if (sample == null) return NotFound();
			await MediaSamplePersistence.RemoveSample(sample);
			return NoContent();
		}
	}

	public sealed class TitleDetailDto
	{
		public Title Title { get; set; }
		public List<LocationForDisplay> Locations { get; set; }
		public List<TitleRatingWithName> Ratings { get; set; }
		public List<MediaSample> Images { get; set; }
	}

	public sealed class TitleUpdateDto
	{
		public string TitleName { get; set; }
		public TitleKind Kind { get; set; }
		public int Year { get; set; }
		public string Description { get; set; }
		public string ImdbId { get; set; }
		public int Season { get; set; }
		public int Disk { get; set; }
		public int EpisodeOrTrack { get; set; }
	}

	public sealed class TitleCreateDto
	{
		public string TitleName { get; set; }
		public TitleKind Kind { get; set; }
		public long? ParentTitleId { get; set; }
		public int Season { get; set; }
		public int Disk { get; set; }
		public int EpisodeOrTrack { get; set; }
	}

	public sealed class MoveTitleRequest
	{
		public long ParentId { get; set; }
	}

	public sealed class MoveTitleResponse
	{
		public Title Title { get; set; }
		public List<MoveTitleParentChildren> RefreshedParents { get; set; }
	}

	public sealed class MoveTitleParentChildren
	{
		public long ParentId { get; set; }
		public List<Title> Children { get; set; }
	}

	public sealed class LocationPatchDto
	{
		public long? LocationBaseId { get; set; }
		public string LocationData { get; set; }
	}

	public sealed class LocationCreateDto
	{
		public long LocationBaseId { get; set; }
		public string LocationData { get; set; }
		public MediaType MediaKind { get; set; }
	}

	public sealed class RatingValueDto
	{
		public long RatingId { get; set; }
		public float Value { get; set; }
	}
}
