using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;

namespace MediaCollection
{
	public static class HttpHelper
	{
		public static async Task<byte[]> MakeHttpRequest(string url, CancellationToken cancellationToken)
		{
			System.Diagnostics.Debug.WriteLine(url);
			using (var client = new HttpClient())
			using (var res = await client.GetAsync(url, cancellationToken))
			{
				var body = await res.Content.ReadAsByteArrayAsync(cancellationToken);
				if (res.StatusCode == System.Net.HttpStatusCode.OK)
				{
					return body;
				}

				throw new HttpException(Encoding.UTF8.GetString(body), null, res.StatusCode);
			}
		}
	}

	public class HttpException : Exception
	{
		public System.Net.HttpStatusCode StatusCode;
		public HttpException(string message, Exception innerException, System.Net.HttpStatusCode statusCode) : base(message, innerException)
		{
			StatusCode = statusCode;
		}
	}
}
