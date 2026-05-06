using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MediaCollection;

namespace MediaCollection.Controllers.Api
{
	[ApiController]
	[Route("api/scan")]
	public sealed class ScanApiController : ControllerBase
	{
		private readonly ScanSessionStore _sessions;

		public ScanApiController(ScanSessionStore sessions)
		{
			_sessions = sessions;
		}

		public sealed class ScanRequestDto
		{
			public long LocationBaseId { get; set; }
			public long DeviceId { get; set; }
		}

		public sealed class ScanPreviewResponse
		{
			public Guid ScanId { get; set; }
			public List<NewFilePreview> NewFiles { get; set; }
			public List<MissingPreview> Missing { get; set; }
		}

		public sealed class NewFilePreview
		{
			public string RelativePath { get; set; }
			public string ParsedTitle { get; set; }
			public MediaType DataType { get; set; }
		}

		public sealed class MissingPreview
		{
			public long LocationId { get; set; }
			public long TitleId { get; set; }
			public string LocationData { get; set; }
			public string TitleName { get; set; }
		}

		public sealed class ScanApplyRequest
		{
			public Guid ScanId { get; set; }
			public List<long> DeleteMissingLocationIds { get; set; }
			public List<string> ImportNewRelativePaths { get; set; }
		}

		[HttpPost("preview")]
		public ActionResult<ScanPreviewResponse> Preview([FromBody] ScanRequestDto req)
		{
			if (req == null) return BadRequest();
			var res = RescanResults.Run(req.LocationBaseId, req.DeviceId);
			var id = _sessions.Put(res);
			var preview = new ScanPreviewResponse
			{
				ScanId = id,
				NewFiles = res.NewFiles.Select(f => new NewFilePreview
				{
					RelativePath = f.RelativePath,
					ParsedTitle = f.Title,
					DataType = f.DataType
				}).ToList(),
				Missing = res.MissingFiles.Select(m => new MissingPreview
				{
					LocationId = m.Id,
					TitleId = m.TitleId,
					LocationData = m.LocationData,
					TitleName = m.Name
				}).ToList()
			};
			return Ok(preview);
		}

		[HttpPost("apply")]
		public IActionResult Apply([FromBody] ScanApplyRequest req)
		{
			if (req == null) return BadRequest();
			if (!_sessions.TryGet(req.ScanId, out var res)) return BadRequest(new { error = "Scan session expired or unknown." });

			var deleteSet = new HashSet<long>(req.DeleteMissingLocationIds ?? new List<long>());
			foreach (var mf in res.MissingFiles.Where(m => deleteSet.Contains(m.Id)))
				mf.Delete();

			var importSet = new HashSet<string>((req.ImportNewRelativePaths ?? new List<string>()).Where(s => !string.IsNullOrEmpty(s)), StringComparer.OrdinalIgnoreCase);
			foreach (var nf in res.NewFiles.Where(f => importSet.Contains(f.RelativePath)))
				nf.Save();

			_sessions.Remove(req.ScanId);
			return Ok();
		}
	}
}
