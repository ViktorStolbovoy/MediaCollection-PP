using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Stolbovoy.Utils;

namespace MediaCollection
{
	public class TmdbData
	{
		[JsonProperty("page")]
		public int PageNumber { get; set; }

		[JsonProperty("results")]
		public TmdbResult[] Results;

		[JsonProperty("total_pages")]
		public int PageCount { get; set; }

		internal static string TmdbUrl = Settings.Get<string>("TMDB_URL").GetAwaiter().GetResult();
		internal static string TmdbAppKey = Uri.EscapeDataString(Settings.Get<string>("TMDB_APP_KEY").GetAwaiter().GetResult());
		public static async Task<TmdbData> Get(string name, bool isTv, CancellationToken cancellationToken)
		{
			string movieSearchUrl = GetUrl(name, isTv);

			var bytes = await HttpHelper.MakeHttpRequest(movieSearchUrl, cancellationToken);
			var res = Encoding.UTF8.GetString(bytes).FromJson<TmdbData>();
			if (res != null && res.Results != null && res.Results.Length > 0)
			{
				foreach (var item in res.Results) item.IsTv = isTv;
			}
			else
			{
				movieSearchUrl = GetUrl(name, !isTv);
				var bytes2 = await HttpHelper.MakeHttpRequest(movieSearchUrl, cancellationToken);
				res = Encoding.UTF8.GetString(bytes2).FromJson<TmdbData>();
				if (res != null && res.Results != null)
				{
					foreach (var item in res.Results) item.IsTv = !isTv;
				}
			}

			if (res != null && res.Results != null)
			{
				await Task.WhenAll(res.Results.Select((item) => item.GetPoster(false, cancellationToken)));
			}
			return res;
		}


		private static string GetUrl(string name, bool isTv)
		{
			return  string.Format("{0}/search/{3}?query={1}&api_key={2}", TmdbUrl, Uri.EscapeDataString(name), TmdbAppKey, isTv ? "tv" : "movie");
		}
	}


	public class TmdbResult
	{
		[JsonProperty("status_code")]
		public int StatusCode { get; set; }

		[JsonProperty("status_message")]
		public string StatusMessage { get; set; }

		[JsonProperty("adult")]
		public bool Adult { get; set; }

		[JsonProperty("backdrop_path")]
		public string BackdropPath { get; set; }

		[JsonProperty("id")]
		public int Id { get; set; }

		
		[JsonProperty("imdb_id")]
		public string ImdbId { get; set; }

		[JsonProperty("overview")]
		public string Overview { get; set; }

		[JsonProperty("popularity")]
		public double Popularity { get; set; }

		[JsonProperty("poster_path")]
		public string PosterPath { get; set; }
		
		[JsonProperty("release_date")]
		public DateTime? ReleaseDate { get; set; }

		[JsonProperty("first_air_date")]
		public DateTime? FirstAirDate { get; set; }

		[JsonProperty("runtime")]
		public int? Runtime { get; set; }

		[JsonProperty("status")]
		public string Status { get; set; }

		[JsonProperty("tagline")]
		public string Tagline { get; set; }

		[JsonProperty("title")]
		public string Title { get; set; }

		public bool IsTv { get; internal set; }

		public byte[] Poster;
		public byte[] Backdrop;
		public readonly List<byte[]> Images = new List<byte[]>();

		public async Task GetMore(CancellationToken cancellationToken)
		{
			string url = string.Format("{0}/{1}/{2}?api_key={3}", TmdbData.TmdbUrl, IsTv ? "tv" : "movie", Id,TmdbData.TmdbAppKey);

			var bytes = await HttpHelper.MakeHttpRequest(url, cancellationToken);
			var res = Encoding.UTF8.GetString(bytes).FromJson<TmdbResult>();
			ImdbId = res.ImdbId;
			if (!ReleaseDate.HasValue) ReleaseDate = FirstAirDate;
		}

		static string s_tmdbImageUrl = Settings.Get<string>("TMDB_IMAGE_URL").GetAwaiter().GetResult();


		public async Task GetPoster(bool shouldGetSmall, CancellationToken cancellationToken)
		{
			if (string.IsNullOrEmpty(PosterPath)) return;
			string url = string.Format("{0}/{2}/{1}", s_tmdbImageUrl, PosterPath, shouldGetSmall ? "w342" : "original");

			Poster = await HttpHelper.MakeHttpRequest(url, cancellationToken);
		}

		public async Task GetBackdrop(bool shouldGetSmall, CancellationToken cancellationToken)
		{
			if (string.IsNullOrEmpty(PosterPath)) return;
			string url = string.Format("{0}/{2}/{1}", s_tmdbImageUrl, BackdropPath, shouldGetSmall ? "w300" : "original");

			Backdrop = await HttpHelper.MakeHttpRequest(url, cancellationToken);
		}
	}
	
}
