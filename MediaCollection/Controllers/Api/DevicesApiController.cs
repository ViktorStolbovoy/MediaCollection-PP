using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MediaCollection;

namespace MediaCollection.Controllers.Api
{
	[ApiController]
	[Route("api/devices")]
	public sealed class DevicesApiController : ControllerBase
	{
		public sealed class DeviceWorkspaceDto
		{
			public List<Device> Devices { get; set; }
			public Dictionary<long, List<LocationBaseDeviceMapping>> MappingsByDeviceId { get; set; }
		}

		[HttpGet]
		public ActionResult<DeviceWorkspaceDto> GetWorkspace()
		{
			var devices = DevicePersistense.List();
			var map = devices.ToDictionary(d => d.Id, d => DevicePersistense.GetLocations(d.Id));
			return Ok(new DeviceWorkspaceDto { Devices = devices, MappingsByDeviceId = map });
		}

		[HttpPost]
		public ActionResult<Device> Create([FromBody] Device device)
		{
			if (device == null || string.IsNullOrWhiteSpace(device.Name)) return BadRequest();
			device.Id = 0;
			device.Set();
			return Ok(device);
		}

		[HttpPut("{id:long}")]
		public IActionResult Update(long id, [FromBody] Device patch)
		{
			if (patch == null) return BadRequest();
			var list = DevicePersistense.List();
			var d = list.FirstOrDefault(x => x.Id == id);
			if (d == null) return NotFound();
			if (!string.IsNullOrEmpty(patch.Name)) d.Name = patch.Name;
			if (patch.Data != null) d.Data = patch.Data;
			d.Kind = patch.Kind;
			d.IsDefault = patch.IsDefault;
			d.Set();
			return Ok(d);
		}

		[HttpDelete("{id:long}")]
		public IActionResult Delete(long id)
		{
			try
			{
				Device.Delete(id);
				return NoContent();
			}
			catch (System.Exception ex)
			{
				return BadRequest(new { error = ex.Message });
			}
		}

		[HttpPut("{deviceId:long}/mappings")]
		public IActionResult PutMappings(long deviceId, [FromBody] List<MappingPatchDto> mappings)
		{
			if (mappings == null) return BadRequest();
			foreach (var m in mappings)
			{
				var row = new LocationBaseDeviceMapping
				{
					DeviceId = deviceId,
					LocationBaseId = m.LocationBaseId,
					Mapping = m.Mapping ?? ""
				};
				row.Set();
			}
			return Ok(DevicePersistense.GetLocations(deviceId));
		}
	}

	public sealed class MappingPatchDto
	{
		public long LocationBaseId { get; set; }
		public string Mapping { get; set; }
	}
}
