using System;
using System.Collections.Generic;
using System.Drawing;
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

		internal static string TmdbUrl = Settings.Get<string>("TMDB_URL");
		internal static string TmdbAppKey = Uri.EscapeDataString(Settings.Get<string>("TMDB_APP_KEY"));
		public static Task<TmdbData> Get(string name, bool isTv, CancellationToken cancellationToken)
		{
			string movieSearchUrl = GetUrl(name, isTv);

			return HttpHelper.MakeHttpRequest(movieSearchUrl, cancellationToken).ContinueWith<Task<TmdbData>>((t) => {
				var res = Encoding.UTF8.GetString(t.Result).FromJson<TmdbData>();
				if (res != null && res.Results != null && res.Results.Length > 0)
				{
					foreach (var item in res.Results) item.IsTv = isTv;
					return Task.FromResult(res);
				}

				movieSearchUrl = GetUrl(name, !isTv);
				return HttpHelper.MakeHttpRequest(movieSearchUrl, cancellationToken).ContinueWith<TmdbData>((t1) => {
					var res1 = Encoding.UTF8.GetString(t1.Result).FromJson<TmdbData>();
					if (res1 != null && res1.Results != null)
					{
						foreach (var item in res1.Results) item.IsTv = !isTv;
					}
					return res1;
				}, cancellationToken);
			},cancellationToken).Unwrap().ContinueWith<TmdbData>((t2, o) => {
				var res = t2.Result;
				if (res != null && res.Results != null)
				{
					Task.WaitAll(res.Results.Select((item) => item.GetPoster(false, cancellationToken)).ToArray());
				}
				return res;
			}, null, cancellationToken);
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

		public Task GetMore(CancellationToken cancellationToken)
		{
			string url = string.Format("{0}/{1}/{2}?api_key={3}", TmdbData.TmdbUrl, IsTv ? "tv" : "movie", Id,TmdbData.TmdbAppKey);

			return HttpHelper.MakeHttpRequest(url, cancellationToken).ContinueWith((t) => {
				var res = Encoding.UTF8.GetString(t.Result).FromJson<TmdbResult>();
				ImdbId = res.ImdbId;
				if (!ReleaseDate.HasValue) ReleaseDate = FirstAirDate;
			}, cancellationToken);
		}

		static string s_tmdbImageUrl = Settings.Get<string>("TMDB_IMAGE_URL");


		public Task GetPoster(bool shouldGetSmall, CancellationToken cancellationToken)
		{
			if (string.IsNullOrEmpty(PosterPath)) return Task.FromResult(false);
			string url = string.Format("{0}/{2}/{1}", s_tmdbImageUrl, PosterPath, shouldGetSmall ? "w342" : "original");

			return HttpHelper.MakeHttpRequest(url, cancellationToken).ContinueWith((t) => {
				Poster = t.Result;
			}, cancellationToken);
		}

		public Task GetBackdrop(bool shouldGetSmall, CancellationToken cancellationToken)
		{
			if (string.IsNullOrEmpty(PosterPath)) return Task.FromResult(false);
			string url = string.Format("{0}/{2}/{1}", s_tmdbImageUrl, BackdropPath, shouldGetSmall ? "w300" : "original");

			return HttpHelper.MakeHttpRequest(url, cancellationToken).ContinueWith((t) => {
				Backdrop = t.Result;
			}, cancellationToken);
		}
	}
	
}
