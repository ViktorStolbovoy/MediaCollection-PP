using System;
using System.Text.Json;

namespace MediaCollection.WebSockets
{
	public sealed class LibrarySubscription
	{
		public string ResourceKind { get; private set; } = "video";
		public bool IncludeHidden { get; private set; }
		public long? SelectedTitleId { get; private set; }

		public static LibrarySubscription FromPayload(JsonElement payload)
		{
			var sub = new LibrarySubscription();

			if (payload.ValueKind == JsonValueKind.Object)
			{
				if (payload.TryGetProperty("ResourceKind", out var rk) || payload.TryGetProperty("resourceKind", out rk))
				{
					if (rk.ValueKind == JsonValueKind.String)
					{
						var value = rk.GetString();
						if (string.Equals(value, "audio", StringComparison.OrdinalIgnoreCase))
							sub.ResourceKind = "audio";
						else
							sub.ResourceKind = "video";
					}
				}

				if (payload.TryGetProperty("IncludeHidden", out var ih) || payload.TryGetProperty("includeHidden", out ih))
				{
					if (ih.ValueKind == JsonValueKind.True) sub.IncludeHidden = true;
					else if (ih.ValueKind == JsonValueKind.False) sub.IncludeHidden = false;
				}

				if (payload.TryGetProperty("SelectedTitleId", out var sid) || payload.TryGetProperty("selectedTitleId", out sid))
				{
					if (sid.ValueKind == JsonValueKind.Number && sid.TryGetInt64(out var id) && id > 0)
						sub.SelectedTitleId = id;
				}
			}

			return sub;
		}
	}
}
