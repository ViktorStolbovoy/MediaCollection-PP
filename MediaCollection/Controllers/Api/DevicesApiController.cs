using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

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
		public async Task<ActionResult<DeviceWorkspaceDto>> GetWorkspace()
		{
			var devices = await DevicePersistense.List();
			var map = new Dictionary<long, List<LocationBaseDeviceMapping>>();
			foreach (var d in devices)
			{
				map[d.Id] = await DevicePersistense.GetLocations(d.Id);
			}
			return Ok(new DeviceWorkspaceDto { Devices = devices, MappingsByDeviceId = map });
		}

		[HttpPost]
		public async Task<ActionResult<Device>> Create([FromBody] Device device)
		{
			if (device == null || string.IsNullOrWhiteSpace(device.Name)) return BadRequest();
			device.Id = 0;
			await device.Set();
			return Ok(device);
		}

		[HttpPut("{id:long}")]
		public async Task<IActionResult> Update(long id, [FromBody] Device patch)
		{
			if (patch == null) return BadRequest();
			var list = await DevicePersistense.List();
			var d = list.FirstOrDefault(x => x.Id == id);
			if (d == null) return NotFound();
			if (!string.IsNullOrEmpty(patch.Name)) d.Name = patch.Name;
			if (patch.Data != null) d.Data = patch.Data;
			d.Kind = patch.Kind;
			d.IsDefault = patch.IsDefault;
			await d.Set();
			return Ok(d);
		}

		[HttpDelete("{id:long}")]
		public async Task<IActionResult> Delete(long id)
		{
			try
			{
				await Device.Delete(id);
				return NoContent();
			}
			catch (System.Exception ex)
			{
				return BadRequest(new { error = ex.Message });
			}
		}

		[HttpPut("{deviceId:long}/mappings")]
		public async Task<IActionResult> PutMappings(long deviceId, [FromBody] List<MappingPatchDto> mappings)
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
				await row.Set();
			}
			return Ok(await DevicePersistense.GetLocations(deviceId));
		}
	}

	public sealed class MappingPatchDto
	{
		public long LocationBaseId { get; set; }
		public string Mapping { get; set; }
	}
}
