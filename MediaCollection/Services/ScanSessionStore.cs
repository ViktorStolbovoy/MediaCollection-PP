using System;
using Microsoft.Extensions.Caching.Memory;

namespace MediaCollection
{
	public sealed class ScanSessionStore
	{
		private readonly IMemoryCache _cache;
		private static readonly TimeSpan Ttl = TimeSpan.FromMinutes(30);

		public ScanSessionStore(IMemoryCache cache)
		{
			_cache = cache;
		}

		public Guid Put(RescanResults results)
		{
			var id = Guid.NewGuid();
			_cache.Set(id, results, Ttl);
			return id;
		}

		public bool TryGet(Guid id, out RescanResults results)
		{
			if (_cache.TryGetValue(id, out object obj) && obj is RescanResults r)
			{
				results = r;
				return true;
			}
			results = null;
			return false;
		}

		public void Remove(Guid id)
		{
			_cache.Remove(id);
		}
	}
}
