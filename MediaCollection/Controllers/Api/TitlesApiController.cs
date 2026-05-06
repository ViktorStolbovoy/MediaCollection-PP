using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPoco;
using MediaCollection;

namespace MediaCollection.Controllers.Api
{
	[ApiController]
	[Route("api/titles")]
	public sealed class TitlesApiController : ControllerBase
	{
		private static Title FetchTitle(IDatabase db, long id)
		{
			return db.Fetch<Title>("WHERE TITLE_ID = @0", id).FirstOrDefault();
		}

		[HttpGet("roots")]
		public ActionResult<IEnumerable<Title>> GetRoots([FromQuery] string resourceKind = "video", [FromQuery] bool includeHidden = false)
		{
			List<Title> list;
			if (string.Equals(resourceKind, "audio", StringComparison.OrdinalIgnoreCase))
				list = TitlePersistence.ListRootAudio(includeHidden);
			else
				list = TitlePersistence.ListRootVideo(includeHidden);
			list.Sort();
			return Ok(list);
		}

		[HttpGet("{parentId:long}/children")]
		public ActionResult<IEnumerable<Title>> GetChildren(long parentId)
		{
			var list = TitlePersistence.ListTitlesByParent(parentId);
			list.Sort();
			return Ok(list);
		}

		[HttpGet("{id:long}", Name = "TitleDetail")]
		public ActionResult<TitleDetailDto> GetDetail(long id)
		{
			using var db = DB.GetDatabase();
			var title = FetchTitle(db, id);
			if (title == null) return NotFound();
			var dto = new TitleDetailDto
			{
				Title = title,
				Locations = LocationPersistence.ListTitleLocations(id),
				Ratings = TitlePersistence.GetRatings(id),
				Images = MediaSamplePersistence.GetSamples(id, MediaSampleKind.Image)
			};
			return Ok(dto);
		}

		[HttpPut("{id:long}")]
		public IActionResult Update(long id, [FromBody] TitleUpdateDto body)
		{
			if (body == null) return BadRequest();
			using var db = DB.GetDatabase();
			var title = FetchTitle(db, id);
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
			GeneralPersistense.Upsert(title);
			return Ok(title);
		}

		[HttpPost]
		public ActionResult<Title> Create([FromBody] TitleCreateDto body)
		{
			if (body == null || string.IsNullOrWhiteSpace(body.TitleName)) return BadRequest();
			var now = GeneralPersistense.GetTimestamp();
			var t = TitlePersistence.AddTitle(body.TitleName.Trim(), body.Kind, body.Season, body.Disk, body.EpisodeOrTrack, body.ParentTitleId);
			return CreatedAtAction("TitleDetail", new { id = t.Id }, t);
		}

		[HttpDelete("{id:long}")]
		public IActionResult Delete(long id)
		{
			try
			{
				TitlePersistence.DeleteTitle(id);
				return NoContent();
			}
			catch (Exception ex)
			{
				return BadRequest(new { error = ex.Message });
			}
		}

		[HttpPost("{id:long}/toggle-hidden")]
		public IActionResult ToggleHidden(long id)
		{
			using var db = DB.GetDatabase();
			var title = FetchTitle(db, id);
			if (title == null) return NotFound();
			var hidden = !title.Hidden;
			TitlePersistence.SetHidden(id, hidden);
			title.Hidden = hidden;
			return Ok(title);
		}

		[HttpPost("{id:long}/move")]
		public IActionResult Move(long id, [FromBody] MoveTitleRequest body)
		{
			if (body == null) return BadRequest();
			var err = TitleHierarchyService.TryMove(id, body.ParentId);
			if (err != null) return BadRequest(new { error = err });
			using var db = DB.GetDatabase();
			return Ok(FetchTitle(db, id));
		}

		[HttpPut("{titleId:long}/locations/{locationId:long}")]
		public IActionResult UpdateLocation(long titleId, long locationId, [FromBody] LocationPatchDto body)
		{
			if (body == null) return BadRequest();
			using var db = DB.GetDatabase();
			var loc = db.Fetch<Location>("WHERE LOCATION_ID = @0 AND TITLE_ID = @1", locationId, titleId).FirstOrDefault();
			if (loc == null) return NotFound();
			if (body.LocationBaseId.HasValue) loc.LocationBaseId = body.LocationBaseId.Value;
			if (body.LocationData != null) loc.LocationData = body.LocationData;
			loc.DateModifiedUtc = GeneralPersistense.GetTimestamp();
			GeneralPersistense.Upsert(loc);
			return Ok(loc);
		}

		[HttpPost("{titleId:long}/locations")]
		public ActionResult<Location> AddLocation(long titleId, [FromBody] LocationCreateDto body)
		{
			if (body == null) return BadRequest();
			using var db = DB.GetDatabase();
			if (FetchTitle(db, titleId) == null) return NotFound();
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
			GeneralPersistense.Upsert(loc);
			return CreatedAtAction("TitleDetail", new { id = titleId }, loc);
		}

		[HttpDelete("{titleId:long}/locations/{locationId:long}")]
		public IActionResult DeleteLocation(long titleId, long locationId)
		{
			using var db = DB.GetDatabase();
			var loc = db.Fetch<Location>("WHERE LOCATION_ID = @0 AND TITLE_ID = @1", locationId, titleId).FirstOrDefault();
			if (loc == null) return NotFound();
			loc.Delete();
			return NoContent();
		}

		[HttpPut("{titleId:long}/ratings")]
		public IActionResult PutRatings(long titleId, [FromBody] List<RatingValueDto> ratings)
		{
			if (ratings == null) return BadRequest();
			using var db = DB.GetDatabase();
			if (FetchTitle(db, titleId) == null) return NotFound();
			foreach (var rv in ratings)
			{
				var existing = TitlePersistence.GetRatings(titleId).FirstOrDefault(r => r.RatingId == rv.RatingId);
				if (existing == null)
				{
					var tw = new TitleRatingWithName
					{
						RatingId = rv.RatingId,
						RatingValue = rv.Value,
						TitleId = 0
					};
					tw.Set(titleId);
				}
				else
				{
					existing.RatingValue = rv.Value;
					existing.Set(titleId);
				}
			}
			return Ok(TitlePersistence.GetRatings(titleId));
		}

		[HttpGet("{titleId:long}/images/{sampleId:long}/file")]
		public IActionResult GetImageFile(long titleId, long sampleId)
		{
			var list = MediaSamplePersistence.GetSamples(titleId, MediaSampleKind.Image);
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
		public IActionResult UploadImage(long titleId, IFormFile file)
		{
			if (file == null || file.Length == 0) return BadRequest();
			using var db = DB.GetDatabase();
			if (FetchTitle(db, titleId) == null) return NotFound();
			using var ms = new MemoryStream();
			file.CopyTo(ms);
			var ext = Path.GetExtension(file.FileName) ?? ".jpg";
			var sample = MediaSamplePersistence.AddSample(ms.ToArray(), titleId, MediaSampleKind.Image, ext);
			return Ok(sample);
		}

		[HttpDelete("{titleId:long}/images/{sampleId:long}")]
		public IActionResult DeleteImage(long titleId, long sampleId)
		{
			var list = MediaSamplePersistence.GetSamples(titleId, MediaSampleKind.Image);
			var sample = list.FirstOrDefault(s => s.Id == sampleId);
			if (sample == null) return NotFound();
			MediaSamplePersistence.RemoveSample(sample);
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
